using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Interfaces;

namespace Otp.Api.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class OtpKeyAuthorize : Attribute, IAsyncAuthorizationFilter
{
	public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
		var otpContextService = context.HttpContext.RequestServices.GetRequiredService<IOtpContextService>();
		if (string.IsNullOrEmpty(otpContextService.AuthenticityKey))
		{
			context.Result = new ContentResult
			{
				StatusCode = StatusCodes.Status401Unauthorized,
				Content = "Otp key header was missing",
				ContentType = MediaTypeNames.Application.Json
			};
			return;
		}

		var applicationDbContext = context.HttpContext.RequestServices.GetRequiredService<IApplicationDbContext>();
		if (await applicationDbContext.OtpRequests.CountAsync(c => c.AuthenticityKey == otpContextService.AuthenticityKey) == 0)
			context.Result = new ContentResult
			{
				StatusCode = StatusCodes.Status401Unauthorized,
				Content = "Invalid otp request",
				ContentType = MediaTypeNames.Application.Json
			};
	}
}