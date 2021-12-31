using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Interfaces;
using Otp.Core.Utils;

namespace Otp.Api.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthorize : Attribute, IAsyncAuthorizationFilter
{
	private const string ApiHeaderKey = "api-key";

	public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
		if (!context.HttpContext.Request.Headers.TryGetValue(ApiHeaderKey, out var apiKey))
		{
			context.Result = new ContentResult
			{
				StatusCode = StatusCodes.Status401Unauthorized,
				Content = "Api key header was missing",
				ContentType = MediaTypeNames.Application.Json
			};
			return;
		}

		var applicationDbContext = context.HttpContext.RequestServices.GetRequiredService<IApplicationDbContext>();
		if (await applicationDbContext.Apps.SingleOrDefaultAsync(c => c.HashedApiKey == CryptoUtil.HashKey(apiKey)) is null)
			context.Result = new ContentResult
			{
				StatusCode = StatusCodes.Status401Unauthorized,
				Content = "Invalid api key",
				ContentType = MediaTypeNames.Application.Json
			};
	}
}