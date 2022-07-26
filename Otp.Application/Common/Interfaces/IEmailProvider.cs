using Otp.Core.Domains.Entities;

namespace Otp.Application.Common.Interfaces;

public interface IEmailProvider
{
	Task<string> Send(OtpRequest request, CancellationToken cancellationToken = default);
}