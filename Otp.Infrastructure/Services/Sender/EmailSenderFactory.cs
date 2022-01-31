using Otp.Application.Common.Interfaces;
using Otp.Application.Services;
using Otp.Core.Domains;
using Otp.Core.Domains.Common;
using Otp.Core.Domains.Entities;
using Otp.Infrastructure.Services.Sender.EmailProviders;

namespace Otp.Infrastructure.Services.Sender;

public class EmailSenderFactory : ISenderFactory
{
	private readonly IEmailProvider _emailProvider;

	public EmailSenderFactory(IEmailProvider emailProvider)
	{
		_emailProvider = emailProvider;
	}

	public Mode SupportedMode => Mode.Email;

	public async Task Send(OtpRequest request, CancellationToken cancellationToken)
	{
		await _emailProvider.Send(request, cancellationToken);
	}
}