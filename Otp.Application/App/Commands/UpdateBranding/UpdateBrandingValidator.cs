using System.Net.Mime;
using FluentValidation;
using Otp.Core.Domains.ValueObjects;

namespace Otp.Application.App.Commands.UpdateBranding;

public class UpdateBrandingValidator : AbstractValidator<UpdateBranding>
{
	private const int MbInBytes = 1048576;
	private const int MaxBackgroundSize = 5 * MbInBytes;
	private const int MaxLogoSize = 2 * MbInBytes;
	private readonly string[] _allowedImageContentType = { MediaTypeNames.Image.Jpeg, "image/png", "image/svg+xml" };

	public UpdateBrandingValidator()
	{
		When(request => request.BackgroundImage is not null,
			() =>
			{
				RuleFor(request => request.BackgroundImage!.Length)
					.LessThanOrEqualTo(MaxBackgroundSize)
					.WithMessage($"Background image must be smaller than {MaxBackgroundSize / MbInBytes}MB");
				RuleFor(request => request.BackgroundImage!.ContentType)
					.Must(type => _allowedImageContentType.Any(allowed => allowed == type))
					.WithMessage($"Content type must be one of {string.Join(",", _allowedImageContentType)}");
			});
		When(request => request.LogoImage is not null,
			() =>
			{
				RuleFor(request => request.LogoImage!.Length)
					.LessThanOrEqualTo(MaxLogoSize)
					.WithMessage($"Logo image must be smaller than {MaxLogoSize / MbInBytes}MB");
				RuleFor(request => request.LogoImage!.ContentType)
					.Must(type => _allowedImageContentType.Any(allowed => allowed == type))
					.WithMessage($"Content type must be one of {string.Join(",", _allowedImageContentType)}");
			});
		When(request => !string.IsNullOrEmpty(request.SmsMessageTemplate),
			() => { RuleFor(request => request.SmsMessageTemplate!).Must(template => template.Contains(Branding.MessageKeyword)); });
	}
}