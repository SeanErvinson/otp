using FluentValidation;
using Otp.Application.Common.Interfaces;
using Otp.Application.Common.Utils;
using Otp.Core.Domains.Common.Enums;

namespace Otp.Application.Otp.Commands.RequestOtp;

public class RequestOtpCommandValidator : AbstractValidator<RequestOtp>
{
	public RequestOtpCommandValidator(IEnumerable<ISmsProvider> smsProviders)
	{
		var supportedCountryCodes = smsProviders.SelectMany(c => c.SupportedCountryCodes);
		RuleFor(c => c.Contact).NotEmpty();
		When(c => c.Channel == Channel.Email,
			() => { RuleFor(c => c.Contact).EmailAddress().WithMessage("Email provided is not a valid one."); });
		When(c => c.Channel == Channel.Sms,
			() =>
			{
				RuleFor(c => PhoneUtils.ExtractCountryCode(c.Contact))
					.Must(c => supportedCountryCodes.Contains(c))
					.WithMessage("Phone number is currently not supported.");
			});
		RuleFor(c => c.SuccessUrl).NotEmpty().Must(url => Uri.TryCreate(url, UriKind.Absolute, out _));
		RuleFor(c => c.CancelUrl).NotEmpty().Must(url => Uri.TryCreate(url, UriKind.Absolute, out _));
	}
}