using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Common;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IDomainEventService _publisher;

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService, IDomainEventService publisher) :
		base(options)
	{
		_currentUserService = currentUserService;
		_publisher = publisher;
	}

	public DbSet<App> Apps { get; set; } = default!;
	public DbSet<Principal> Principals { get; set; } = default!;
	public DbSet<OtpRequest> OtpRequests { get; set; } = default!;
	public DbSet<CallbackEvent> CallbackEvents { get; set; } = default!;

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		foreach (var entry in ChangeTracker.Entries<TimedEntity>())
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedAt = DateTime.UtcNow;
					break;
				case EntityState.Modified:
					entry.Entity.UpdatedAt = DateTime.UtcNow;
					break;
			}

		foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedBy = _currentUserService.PrincipalId.ToString();
					break;
				case EntityState.Modified:
					entry.Entity.CreatedBy = _currentUserService.PrincipalId.ToString();
					break;
			}

		var domainEvents = ChangeTracker.Entries<IHasDomainEvent>()
										.Select(entry => entry.Entity)
										.SelectMany(hasDomainEvent => hasDomainEvent.DomainEvents)
										.ToList();

		var result = await base.SaveChangesAsync(cancellationToken);

		await PublishDomainEvents(domainEvents);

		return result;
	}

	private async Task PublishDomainEvents(List<DomainEvent> domainEvents)
	{
		foreach (var domainEvent in domainEvents)
		{
			await _publisher.Publish(domainEvent);
		}
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		base.OnModelCreating(builder);
	}
}