using Otp.Application.Common.Interfaces;
using Otp.Core.Utils;

namespace Otp.Api.Services;

public class AppContextService : IAppContextService
{
	private const string ApiHeaderKey = "api-key";
	private readonly IHttpContextAccessor _httpContextAccessor;

	public AppContextService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public string? HashApiKey
	{
		get
		{
			if(_httpContextAccessor.HttpContext is null)
				return string.Empty;
			return _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(ApiHeaderKey, out var apiKey) ? CryptoUtil.HashKey(apiKey) : string.Empty;
		}
	}
}