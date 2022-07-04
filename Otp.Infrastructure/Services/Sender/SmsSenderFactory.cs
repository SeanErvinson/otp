using Otp.Application.Common.Interfaces;
using Otp.Application.Common.Utils;
using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Entities;

namespace Otp.Infrastructure.Services.Sender;

public class SmsSenderFactory : ISenderFactory
{
	private readonly IEnumerable<ISmsProvider> _smsProviders;

	public SmsSenderFactory(IEnumerable<ISmsProvider> smsProviders)
	{
		_smsProviders = smsProviders;
	}

	public Channel SupportedChannel => Channel.Sms;

	public async Task Send(OtpRequest request, CancellationToken cancellationToken = default)
	{
		var countryCode = PhoneUtils.ExtractCountryCode(request.Recipient);
		var provider =
			_smsProviders.FirstOrDefault(provider =>
				provider.SupportedCountryCodes.Contains(countryCode,
					StringComparer.InvariantCultureIgnoreCase));

		if (provider is null)
		{
			throw new NotSupportedException($"{request.Recipient} is not supported by any SMS provider.");
		}
		await provider.Send(request, cancellationToken);
	}
}