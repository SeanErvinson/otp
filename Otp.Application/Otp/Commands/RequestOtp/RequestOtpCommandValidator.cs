using FluentValidation;
using Otp.Application.Common.Interfaces;
using Otp.Application.Common.Utils;
using Otp.Core.Domains.Common;

namespace Otp.Application.Otp.Commands.RequestOtp;

public class RequestOtpCommandValidator : AbstractValidator<RequestOtpCommand>
{
	public RequestOtpCommandValidator(IEnumerable<ISmsProvider> smsProviders)
	{
		var supportedCountryCodes = smsProviders.SelectMany(c => c.SupportedCountryCodes);
		RuleFor(c => c.Contact).NotEmpty();
		When(c => c.Mode == Mode.Email, () => { RuleFor(c => c.Contact).EmailAddress().WithMessage("Email provided is not a valid one."); });
		When(c => c.Mode == Mode.SMS,
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