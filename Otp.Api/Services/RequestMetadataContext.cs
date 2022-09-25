using Otp.Application.Common.Interfaces;

namespace Otp.Api.Services;

public class RequestMetadataContext : IRequestMetadataContext
{
	public string? Country { get; }

	private const string CloudFlareCountryHeader = "CF-IPCountry";
	
	public RequestMetadataContext(IHttpContextAccessor httpContextAccessor)
	{
		ArgumentNullException.ThrowIfNull(httpContextAccessor.HttpContext);
		
		if(httpContextAccessor.HttpContext.Request.Headers.TryGetValue(CloudFlareCountryHeader, out var country))
			Country = country;
	}
}