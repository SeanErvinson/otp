using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains;
using Otp.Core.Domains.Common;

namespace Otp.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
	private readonly ICurrentUserService _currentUserService;

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : base(options)
	{
		_currentUserService = currentUserService;
	}

	public DbSet<App> Apps { get; set; }
	public DbSet<Principal> Principals { get; set; }

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedBy = _currentUserService.PrincipalId.ToString();
					entry.Entity.CreatedAt = DateTime.UtcNow;
					break;
				case EntityState.Modified:
					entry.Entity.CreatedBy = _currentUserService.PrincipalId.ToString();
					entry.Entity.UpdatedAt = DateTime.UtcNow;
					break;
			}

		var result = await base.SaveChangesAsync(cancellationToken);

		return result;
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		base.OnModelCreating(builder);
	}
}