using Otp.Application.Common.Interfaces;

namespace Otp.Api.Services;

public class OtpContextService : IOtpContextService
{
	private const string OtpKeyHeader = "OTP_KEY";
	private readonly IHttpContextAccessor _httpContextAccessor;

	public OtpContextService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public string? AuthenticityKey
	{
		get
		{
			if (_httpContextAccessor.HttpContext is null)
			{
				return string.Empty;
			}
			return _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(OtpKeyHeader, out var key) ? key : string.Empty;
		}
	}
}