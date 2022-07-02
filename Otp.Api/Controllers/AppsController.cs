﻿using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Otp.Application.App.Commands.CreateApp;
using Otp.Application.App.Commands.DeleteApp;
using Otp.Application.App.Commands.RegenerateApiKey;
using Otp.Application.App.Commands.UpdateBranding;
using Otp.Application.App.Commands.UpdateCallback;
using Otp.Application.App.Queries.GetApp;
using Otp.Application.App.Queries.GetAppRecentCallbacks;
using Otp.Application.App.Queries.GetApps;
using Otp.Application.Common.Models;
using Otp.Application.Logs.Queries.GetLogs;
using Otp.Application.Otp.Commands.RequestOtp;

namespace Otp.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
public class AppsController : ControllerBase
{
	private readonly IMediator _mediator;

	public AppsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateAppResponse))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> CreateApp([FromBody] CreateApp request)
	{
		var result = await _mediator.Send(request);
		return Created(Url.RouteUrl(nameof(GetApp), new { id = result.Id }), result);
	}

	[HttpPut("{id:guid}/callback")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> UpdateCallback(Guid id, [FromBody] UpdateCallbackRequest request)
	{
		await _mediator.Send(new UpdateCallback(id, request.CallbackUrl, request.EndpointSecret));
		return NoContent();
	}

	[HttpGet("{id:guid}/recent-callbacks")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetAppRecentCallbacksResponse>))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> GetAppRecentCallbacks(Guid id)
	{
		var result = await _mediator.Send(new GetAppRecentCallbacks(id));
		return Ok(result);
	}

	[HttpPatch("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequestOtpResponse))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> UpdateApp(Guid id, [FromBody] RequestOtpEmailRequest request)
	{
		var result = await _mediator.Send(RequestOtp.Email(request.EmailAddress, request.SuccessUrl, request.CancelUrl));
		return Ok(result);
	}

	[HttpPost("{id:guid}/regenerate-api-key")]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RequestOtpResponse))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> RegenerateApiKey(Guid id)
	{
		var result = await _mediator.Send(new RegenerateApiKey(id));
		return Ok(result);
	}

	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> DeleteApp(Guid id)
	{
		await _mediator.Send(new DeleteApp(id));
		return NoContent();
	}

	[HttpPut("{id:guid}/branding")]
	[Consumes(MediaTypeNames.Application.Json, "multipart/form-data")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> UpdateBranding([FromRoute] Guid id, [FromForm] UpdateBrandingRequest request)
	{
		await _mediator.Send(new UpdateBranding
		{
			Id = id,
			BackgroundImage = request.BackgroundImage,
			LogoImage = request.LogoImage,
			SmsMessageTemplate = request.SmsMessageTemplate
		});
		return NoContent();
	}


	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResult<GetAppSimpleResponse>))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> GetApps([FromQuery] GetApps request)
	{
		var result = await _mediator.Send(request);
		return Ok(result);
	}

	[HttpGet("{id:guid}", Name = nameof(GetApp))]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAppResponse))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> GetApp(Guid id)
	{
		var result = await _mediator.Send(new GetApp(id));
		return Ok(result);
	}
	
	[HttpGet("logs")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAppResponse))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> GetLogs(GetLogs request)
	{
		var result = await _mediator.Send(request);
		return Ok(result);
	}
}