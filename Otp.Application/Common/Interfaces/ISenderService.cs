using Otp.Core.Domains.Common;
using Otp.Core.Domains.Entities;

namespace Otp.Application.Services;

public interface ISenderService
{
	ISenderFactory GetSenderFactory(Mode mode);
}

public interface ISenderFactory
{
	Mode SupportedMode { get; }
	Task Send(OtpRequest request, CancellationToken cancellationToken = default);
}