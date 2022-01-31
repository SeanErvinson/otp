using Otp.Application.Services;
using Otp.Core.Domains.Common;

namespace Otp.Infrastructure.Services.Sender;

public class SenderService : ISenderService
{
	private readonly IEnumerable<ISenderFactory> _senderFactories;

	public SenderService(IEnumerable<ISenderFactory> senderFactories)
	{
		_senderFactories = senderFactories;
	}

	public ISenderFactory GetSenderFactory(Mode mode)
	{
		var senderFactory = _senderFactories.SingleOrDefault(sender => sender.SupportedMode == mode);

		if (senderFactory is null)
			throw new NotSupportedException($"{mode} is not supported by factory");
		
		return senderFactory;
	}
}