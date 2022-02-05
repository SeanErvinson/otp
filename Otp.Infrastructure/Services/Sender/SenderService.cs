using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Common.Enums;

namespace Otp.Infrastructure.Services.Sender;

public class SenderService : ISenderService
{
	private readonly IEnumerable<ISenderFactory> _senderFactories;

	public SenderService(IEnumerable<ISenderFactory> senderFactories)
	{
		_senderFactories = senderFactories;
	}

	public ISenderFactory GetSenderFactory(Channel channel)
	{
		var senderFactory = _senderFactories.SingleOrDefault(sender => sender.SupportedChannel == channel);

		if (senderFactory is null)
			throw new NotSupportedException($"{channel} is not supported by factory");
		
		return senderFactory;
	}
}