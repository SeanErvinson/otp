using FluentValidation;

namespace Otp.Application.App.Commands.CreateApp;

public class CreateAppValidator : AbstractValidator<CreateApp>
{
	public CreateAppValidator()
	{
		RuleFor(request => request.Name).NotEmpty().Matches(@"[\w-]");
		RuleFor(request => request.Name.Length).InclusiveBetween(5, 64);
		When(request => !string.IsNullOrEmpty(request.Description), 
			() => RuleFor(request => request.Description!.Length).LessThanOrEqualTo(128));
	}
}