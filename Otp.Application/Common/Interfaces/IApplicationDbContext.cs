using Microsoft.EntityFrameworkCore;
using Otp.Core.Domains;
using Otp.Core.Domains.Entities;

namespace Otp.Application.Common.Interfaces;

public interface IApplicationDbContext
{
	DbSet<Core.Domains.Entities.App> Apps { get; set; }

	DbSet<Core.Domains.Entities.Principal> Principals { get; set; }
	DbSet<OtpRequest> OtpRequests { get; set; }
	DbSet<CallbackEvent> CallbackEvents { get; set; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}