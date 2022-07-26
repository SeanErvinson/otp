using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Entities;

namespace Otp.Application.Common.Interfaces;

public interface IChannelProviderService
{
	IChannelProviderFactory GetChannelFactory(Channel channel);
}

public interface IChannelProviderFactory
{
	Channel SupportedChannel { get; }
	Task<string> Send(OtpRequest request, CancellationToken cancellationToken = default);
}