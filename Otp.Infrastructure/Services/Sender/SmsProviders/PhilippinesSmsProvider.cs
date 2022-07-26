using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Services.Sender.SmsProviders;

public class PhilippinesSmsProvider : ISmsProvider
{
	public IEnumerable<string> SupportedCountryCodes => new[] { "63" };

	public Task<string> Send(OtpRequest request, CancellationToken cancellationToken = default)
	{
		Console.WriteLine($"Sending SMS from PH provider {request.Recipient}");
		return Task.FromResult("PH-MessageId");
	}
}