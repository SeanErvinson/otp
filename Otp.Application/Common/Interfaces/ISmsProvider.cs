using Otp.Core.Domains.Entities;

namespace Otp.Application.Common.Interfaces;

public interface ISmsProvider
{
	IEnumerable<string> SupportedCountryCodes { get; }
	Task Send(OtpRequest request, CancellationToken cancellationToken = default);
}