using Microsoft.EntityFrameworkCore;

namespace Otp.Application.Common.Interfaces;

public interface IApplicationDbContext
{
	DbSet<Core.Domains.App> Apps { get; set; }

	DbSet<Core.Domains.Principal> Principals { get; set; }
	// DbSet<Domains.OtpRequest> OtpRequests { get; set; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}