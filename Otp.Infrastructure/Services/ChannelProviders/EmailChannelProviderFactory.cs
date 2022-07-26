using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Services.ChannelProviders;

public class EmailChannelProviderFactory : IChannelProviderFactory
{
	private readonly IEmailProvider _emailProvider;

	public EmailChannelProviderFactory(IEmailProvider emailProvider)
	{
		_emailProvider = emailProvider;
	}

	public Channel SupportedChannel => Channel.Email;

	public async Task<string> Send(OtpRequest request, CancellationToken cancellationToken) =>
		await _emailProvider.Send(request, cancellationToken);
}