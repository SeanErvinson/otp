using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Services.Sender;

public class EmailSenderFactory : ISenderFactory
{
	private readonly IEmailProvider _emailProvider;

	public EmailSenderFactory(IEmailProvider emailProvider)
	{
		_emailProvider = emailProvider;
	}

	public Channel SupportedChannel => Channel.Email;

	public async Task Send(OtpRequest request, CancellationToken cancellationToken)
	{
		await _emailProvider.Send(request, cancellationToken);
	}
}