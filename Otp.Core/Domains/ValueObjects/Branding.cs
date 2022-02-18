using Otp.Core.Domains.Common.Models;

namespace Otp.Core.Domains.ValueObjects;

public class Branding : ValueObject
{
	public string? SmsMessageTemplate { get; private set; }
	public string? BackgroundUrl { get; private set; }
	public string? LogoUrl { get; private set; }

	public void UpdateSmsMessageTemplate(string? smsMessageTemplate)
	{
		SmsMessageTemplate = smsMessageTemplate;
	}

	public void UpdateBackgoundUrl(string? backgroundUrl)
	{
		BackgroundUrl = backgroundUrl;
	}

	public void UpdateLogoUrl(string? logoUrl)
	{
		LogoUrl = logoUrl;
	}
}