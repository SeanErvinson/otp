using FluentValidation;

namespace Otp.Application.Otp.Commands.RequestEmailOtp;

public class RequestEmailOtpValidator : AbstractValidator<RequestEmailOtp>
{
	public RequestEmailOtpValidator()
	{
		RuleFor(c => c.EmailAddress).NotEmpty().MinimumLength(3).MaximumLength(320).EmailAddress().WithMessage("Email provided is not a valid one.");
		RuleFor(c => c.SuccessUrl).NotEmpty().Must(url => Uri.TryCreate(url, UriKind.Absolute, out _));
		RuleFor(c => c.CancelUrl).NotEmpty().Must(url => Uri.TryCreate(url, UriKind.Absolute, out _));
	}
}