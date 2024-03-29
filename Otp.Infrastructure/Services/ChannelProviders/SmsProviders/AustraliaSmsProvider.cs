using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Services.ChannelProviders.SmsProviders;

public class AustraliaSmsProvider : ISmsProvider
{
	public IEnumerable<string> SupportedCountryCodes => new[] { "61" };

	public Task<string> Send(OtpRequest request, CancellationToken cancellationToken = default)
	{
		Console.WriteLine($"Sending SMS from AU provider {request.Recipient}");
		return Task.FromResult("AU-MessageId");
	}
}