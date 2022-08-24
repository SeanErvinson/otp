using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Common.Enums;

namespace Otp.Infrastructure.Services.ChannelProviders;

public class ChannelProviderService : IChannelProviderService
{
	private readonly IEnumerable<IChannelProviderFactory> _senderFactories;

	public ChannelProviderService(IEnumerable<IChannelProviderFactory> senderFactories)
	{
		_senderFactories = senderFactories;
	}

	public IChannelProviderFactory GetChannelFactory(Channel channel)
	{
		var senderFactory = _senderFactories.SingleOrDefault(sender => sender.SupportedChannel == channel);

		if (senderFactory is null)
		{
			throw new NotSupportedException($"{channel} is not supported by factory");
		}
		return senderFactory;
	}
}