using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Otp.Application.Otp.Commands.RequestOtp;

namespace Otp.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
public class StatsController : ControllerBase
{
	private readonly IMediator _mediator;

	public StatsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("callbacks")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequestOtpDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> GetAppRecentCallbacks()
	{
		// var result = await _mediator.Send();
		return Ok();
	}
}