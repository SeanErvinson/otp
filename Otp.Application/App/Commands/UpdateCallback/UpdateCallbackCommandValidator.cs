using FluentValidation;

namespace Otp.Application.App.Commands.UpdateCallback;

public class UpdateCallbackCommandValidator : AbstractValidator<UpdateCallbackCommand>
{
	public UpdateCallbackCommandValidator()
	{
		RuleFor(request => request.CallbackUrl).NotEmpty().Must(c => Uri.TryCreate(c, UriKind.Absolute, out var value) 
																	&& (value.Scheme == Uri.UriSchemeHttp || value.Scheme == Uri.UriSchemeHttps));
	}
}