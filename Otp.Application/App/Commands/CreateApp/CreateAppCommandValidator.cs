using FluentValidation;

namespace Otp.Application.App.Commands.CreateApp;

public class CreateAppCommandValidator : AbstractValidator<CreateAppCommand>
{
	public CreateAppCommandValidator()
	{
		RuleFor(request => request.Name).NotEmpty().Matches(@"[\w-]");
		RuleFor(request => request.Name.Length).InclusiveBetween(5, 64);
		When(request => !string.IsNullOrEmpty(request.Description), 
			() => RuleFor(request => request.Description!.Length).LessThanOrEqualTo(128));
	}
}