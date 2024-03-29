﻿using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Interfaces;

namespace Otp.Api.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthorize : Attribute, IAsyncAuthorizationFilter
{
	public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
		var appContext = context.HttpContext.RequestServices.GetRequiredService<IAppContextService>();

		if (string.IsNullOrEmpty(appContext.HashApiKey))
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

		if (await applicationDbContext.Apps.CountAsync(c => c.HashedApiKey == appContext.HashApiKey) == 0)
		{
			context.Result = new ContentResult
			{
				StatusCode = StatusCodes.Status401Unauthorized,
				Content = "Invalid api key",
				ContentType = MediaTypeNames.Application.Json
			};
		}
	}
}