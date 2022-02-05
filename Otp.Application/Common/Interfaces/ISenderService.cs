using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Entities;

namespace Otp.Application.Common.Interfaces;

public interface ISenderService
{
	ISenderFactory GetSenderFactory(Channel channel);
}

public interface ISenderFactory
{
	Channel SupportedChannel { get; }
	Task Send(OtpRequest request, CancellationToken cancellationToken = default);
}