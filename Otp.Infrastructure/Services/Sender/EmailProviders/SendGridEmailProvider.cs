using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Services.Sender.EmailProviders;

public class SendGridEmailProvider : IEmailProvider
{
	public Task Send(OtpRequest request, CancellationToken cancellationToken = default)
	{
		Console.WriteLine($"Sending Email from SendGridProvider {request.Contact}");
		return Task.CompletedTask;
	}
}