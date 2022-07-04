using FluentValidation;

namespace Otp.Application.App.Commands.UpdateCallback;

public class UpdateCallbackValidator : AbstractValidator<UpdateCallback>
{
	public UpdateCallbackValidator()
	{
		RuleFor(request => request.CallbackUrl)
			.NotEmpty()
			.Must(c => Uri.TryCreate(c, UriKind.Absolute, out var value) &&
				(value.Scheme == Uri.UriSchemeHttp || value.Scheme == Uri.UriSchemeHttps));
	}
}