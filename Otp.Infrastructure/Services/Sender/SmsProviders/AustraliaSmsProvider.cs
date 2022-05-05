using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Services.Sender.SmsProviders;

public class AustraliaSmsProvider : ISmsProvider
{
	public IEnumerable<string> SupportedCountryCodes => new[] { "61" };

	public Task Send(OtpRequest request, CancellationToken cancellationToken = default)
	{
		Console.WriteLine($"Sending SMS from AU provider {request.Contact}");
		return Task.CompletedTask;
	}
}