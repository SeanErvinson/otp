using Otp.Core.Domains.Common;
using Otp.Core.Utils;

namespace Otp.Core.Domains;

public class OtpRequest : BaseEntity
{
	public OtpRequest(string contact, Mode mode, string successUrl, string cancelUrl)
	{
		Contact = contact;
		Mode = mode;
		SuccessUrl = successUrl;
		CancelUrl = cancelUrl;
		Secret = CryptoUtil.HashKey(CryptoUtil.GenerateKey());
	}

	public string SuccessUrl { get; }
	public string CancelUrl { get; }
	public string Contact { get; }
	public Mode Mode { get; }

	public string Secret { get; set; }

	public bool IsValid { get; set; }

	public string RequestPath => new($"/{Enum.GetName(Mode)?.ToLower()}/{Id.ToString()}#{Secret}/");
}