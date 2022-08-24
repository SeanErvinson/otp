using FluentValidation;
using Otp.Application.Common.Interfaces;
using Otp.Application.Common.Utils;
using PhoneNumbers;

namespace Otp.Application.Otp.Commands.RequestOtp;

public class RequestOtpSmsRequestCommandValidator : AbstractValidator<RequestOtpSmsRequest>
{
	public RequestOtpSmsRequestCommandValidator(IEnumerable<ISmsProvider> smsProviders)
	{
		RuleFor(c => c.PhoneNumber).NotEmpty().MaximumLength(24).Must(IsValidPhoneNumber);
		var supportedCountryCodes = smsProviders.SelectMany(c => c.SupportedCountryCodes);
		RuleFor(c => PhoneUtils.ExtractCountryCode(c.PhoneNumber))
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

public class RequestOtpEmailRequestCommandValidator : AbstractValidator<RequestOtpEmailRequest>
{
	public RequestOtpEmailRequestCommandValidator()
	{
		RuleFor(c => c.EmailAddress).NotEmpty().MinimumLength(3).MaximumLength(320).EmailAddress().WithMessage("Email provided is not a valid one.");
		RuleFor(c => c.SuccessUrl).NotEmpty().Must(url => Uri.TryCreate(url, UriKind.Absolute, out _));
		RuleFor(c => c.CancelUrl).NotEmpty().Must(url => Uri.TryCreate(url, UriKind.Absolute, out _));
	}
}