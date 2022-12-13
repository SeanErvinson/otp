using FluentValidation;

namespace Otp.Application.Otp.Commands.VerifyCode;

public class VerifyCodeValidator : AbstractValidator<VerifyCode>
{
	public VerifyCodeValidator()
	{
		RuleFor(request => request.Code).NotEmpty().Matches(@"\d").MaximumLength(10);
		RuleFor(request => request.Id).NotEmpty();
	}
}