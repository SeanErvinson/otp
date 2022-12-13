using FluentValidation;

namespace Otp.Application.App.Commands.CreateApp;

public class CreateAppValidator : AbstractValidator<CreateApp>
{
	public CreateAppValidator()
	{
		RuleFor(request => request.Name).NotEmpty().Matches(@"[\w-]").Length(5, 64);
		RuleFor(request => request.Description).MaximumLength(128);
	}
}