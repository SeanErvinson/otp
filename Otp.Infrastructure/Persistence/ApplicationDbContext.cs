using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Common.Models;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IMediator _mediator;

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
		ICurrentUserService currentUserService,
		IMediator mediator) :
		base(options)
	{
		_currentUserService = currentUserService;
		_mediator = mediator;
	}

	public DbSet<App> Apps { get; set; } = default!;
	public DbSet<Principal> Principals { get; set; } = default!;
	public DbSet<OtpRequest> OtpRequests { get; set; } = default!;
	public DbSet<CallbackEvent> CallbackEvents { get; set; } = default!;
	public DbSet<SmsPrice> SmsPrices { get; set; } = default!;
	public DbSet<EmailPrice> EmailPrices { get; set; } = default!;
	// public DbSet<ChannelPrice> ChannelPrices { get; set; } = default!;
	// public DbSet<Discount> Discounts { get; set; } = default!;

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		foreach (var entry in ChangeTracker.Entries<ITimedEntity>())
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedAt = DateTime.UtcNow;
					break;
				case EntityState.Modified:
					entry.Entity.UpdatedAt = DateTime.UtcNow;
					break;
			}
		}

		foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedBy = _currentUserService.PrincipalId.ToString();
					break;
				case EntityState.Modified:
					entry.Entity.UpdatedBy = _currentUserService.PrincipalId.ToString();
					break;
			}
		}
		var entities = ChangeTracker.Entries<IHasDomainEvent>()
			.Select(entry => entry.Entity);
		await PublishDomainEvents(entities, cancellationToken);
		var result = await base.SaveChangesAsync(cancellationToken);
		return result;
	}

	private Task PublishDomainEvents(IEnumerable<IHasDomainEvent> entities, CancellationToken cancellationToken)
	{
		var domainEventTasks = new List<Task>();

		foreach (var entity in entities)
		{
			var domainEvents = entity.DomainEvents.ToList();
			entity.ClearDomainEvents();
			domainEventTasks.AddRange(domainEvents.Select(domainEvent =>
				_mediator.Publish(domainEvent, cancellationToken)));
		}
		return Task.WhenAll(domainEventTasks);
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(builder);
	}
}