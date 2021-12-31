using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Otp.Application.App.Commands.CreateApp;
using Otp.Application.App.Commands.DeleteApp;
using Otp.Application.App.Commands.RegenerateApiKey;
using Otp.Application.App.Queries.GetApp;
using Otp.Application.App.Queries.GetApps;
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
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateAppCommandDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> CreateApp([FromBody] CreateAppCommand request)
	{
		var result = await _mediator.Send(request);
		return Created(Url.RouteUrl(nameof(GetApp), new { id = result.Id }), result);
	}

	[HttpPatch("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequestOtpDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> UpdateApp(Guid Id, [FromBody] RequestOtpEmailRequest request)
	{
		var result = await _mediator.Send(RequestOtpCommand.Email(request.EmailAddress, request.SuccessUrl, request.CancelUrl));
		return Ok(result);
	}

	[HttpPost("{id:guid}/regenerate-api-key")]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RequestOtpDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> RegenerateApiKey(Guid Id)
	{
		var result = await _mediator.Send(new RegenerateApiKeyCommand(Id));
		return Ok(result);
	}

	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> DeleteApp(Guid id)
	{
		await _mediator.Send(new DeleteAppCommand(id));
		return NoContent();
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAppsQueryDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> GetApps([FromQuery] GetAppsQuery request)
	{
		var result = await _mediator.Send(request);
		return Ok(result);
	}

	[HttpGet("{id:guid}", Name = nameof(GetApp))]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAppQueryDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> GetApp(Guid id)
	{
		var result = await _mediator.Send(new GetAppQuery(id));
		return Ok(result);
	}
}