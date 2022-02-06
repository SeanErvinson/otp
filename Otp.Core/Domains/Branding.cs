using Otp.Core.Domains.Entities;

namespace Otp.Core.Domains;

public class Branding
{
	public Guid PrincipalId { get; set; }
	public string? BackgroundUrl { get; set; }
	public string? LogoUrl { get; set; }

	public Principal Principal { get; set; }
}