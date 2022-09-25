using FluentValidation;
using Otp.Application.Common.Interfaces;
using Otp.Application.Common.Utils;
using PhoneNumbers;

namespace Otp.Application.Otp.Commands.RequestSmsOtp;

public class RequestSmsOtpValidator : AbstractValidator<RequestSmsOtp>
{
	public RequestSmsOtpValidator(IEnumerable<ISmsProvider> smsProviders)
	{
		RuleFor(c => c.PhoneNumber)
			.NotEmpty()
			.MaximumLength(24)
			.Must(IsValidPhoneNumber)
			.WithMessage("Phone number must use international format. Example: +63xxxxxxxxxx");
		var supportedCountryCodes = smsProviders.SelectMany(c => c.SupportedCountryCodes);
		Transform(from: c => c.PhoneNumber, to: PhoneUtils.ExtractCountryCode)
			.Must(c => supportedCountryCodes.Contains(c))
			.WithMessage("Phone number is currently not supported.");
		RuleFor(c => c.SuccessUrl).NotEmpty().Must(url => Uri.TryCreate(url, UriKind.Absolute, out _));
		RuleFor(c => c.CancelUrl).NotEmpty().Must(url => Uri.TryCreate(url, UriKind.Absolute, out _));
	}

	private static bool IsValidPhoneNumber(string value)
	{
		var phoneNumberUtil = PhoneNumberUtil.GetInstance();

		try
		{
			var parsedNumber = phoneNumberUtil.Parse(value, "ZZ");
			return phoneNumberUtil.IsValidNumber(parsedNumber) && parsedNumber.HasCountryCode;
		}
		catch (NumberParseException e)
		{
			return false;
		}
	}
}