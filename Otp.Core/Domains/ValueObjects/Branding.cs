using Otp.Core.Domains.Common.Exceptions;
using Otp.Core.Domains.Common.Models;

namespace Otp.Core.Domains.ValueObjects;

public class Branding : ValueObject
{
	public string? SmsMessageTemplate { get; private set; }
	public string? BackgroundUrl { get; private set; }
	public string? LogoUrl { get; private set; }

	public const string MessageKeyword = "{code}";
	
	public void UpdateBranding(string? logoUrl, string? backgroundUrl, string? smsMessageTemplate)
	{
		if (!string.IsNullOrEmpty(smsMessageTemplate))
		{
			if (!smsMessageTemplate.Contains(MessageKeyword))
			{
				throw new BrandingException($"Sms message template should contain keyword: {MessageKeyword}");
			}
		}
		LogoUrl = logoUrl;
		BackgroundUrl = backgroundUrl;
		SmsMessageTemplate = smsMessageTemplate;
	}
}