using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Otp.Application.Common.Utils;

namespace Otp.Api.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public abstract class BasicAuthAuthorize : Attribute, IAsyncAuthorizationFilter
{
	private const string AuthorizationHeaderKey = "Authorization";
	private const string AuthorizationType = "Basic";

	public Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
		if (!context.HttpContext.Request.Headers.TryGetValue(AuthorizationHeaderKey, out var encodedBasicAuth))
		{
			context.Result = new ContentResult
			{
				StatusCode = StatusCodes.Status401Unauthorized,
				Content = "Authorization is required",
				ContentType = MediaTypeNames.Application.Json
			};
			return Task.CompletedTask;
		}
		var encodedAuth = ExtractAuth(encodedBasicAuth[0]);

		if (!StringUtils.TryBase64Decode(encodedAuth, out var basicAuth))
		{
			context.Result = new ContentResult
			{
				StatusCode = StatusCodes.Status401Unauthorized,
				Content = "Could not decode basic auth",
				ContentType = MediaTypeNames.Application.Json
			};
			return Task.CompletedTask;
		}
		var (parsedUsername, parsedPassword) = ParseBasicAuth(basicAuth);
		var (username, password) = GetCredential(context);

		if (username != parsedUsername || password != parsedPassword)
		{
			context.Result = new ContentResult
			{
				StatusCode = StatusCodes.Status401Unauthorized,
				Content = "Incorrect credentials",
				ContentType = MediaTypeNames.Application.Json
			};
		}
		return Task.CompletedTask;
	}

	protected abstract (string username, string password) GetCredential(AuthorizationFilterContext context);

	private static string ExtractAuth(string basicAuth) => basicAuth.Replace(AuthorizationType, string.Empty).Trim();

	private static (string username, string password) ParseBasicAuth(string basicAuth)
	{
		var values = basicAuth.Split(":");
		return (values[0], values[1]);
	}
}