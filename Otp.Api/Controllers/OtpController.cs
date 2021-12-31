using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Otp.Api.Filters;
using Otp.Application.Otp.Commands.RequestOtp;

namespace Otp.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
public class OtpController : ControllerBase
{
	private readonly IMediator _mediator;

	public OtpController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost("email")]
	[ApiKeyAuthorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequestOtpDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> RequestEmail([FromBody] RequestOtpEmailRequest request)
	{
		var result = await _mediator.Send(RequestOtpCommand.Email(request.EmailAddress, request.SuccessUrl, request.CancelUrl));
		return Ok(result);
	}

	[HttpPost("sms")]
	[ApiKeyAuthorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequestOtpDto))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> RequestSms([FromBody] RequestOtpSmsRequest request)
	{
		var result = await _mediator.Send(RequestOtpCommand.Sms(request.PhoneNumber, request.SuccessUrl, request.CancelUrl));
		return Ok(result);
	}
	
	[HttpPost("send")]
	[Authorize]
	public async Task<IActionResult> SendMode()
	{

		return Ok();
	}

	[HttpPost("resend")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequestOtpDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)] //Return bad request if resend too fast
	public async Task<IActionResult> RequestResend()
	{

		return Ok();
	}
	
	[HttpPost("change-email")]
	[Authorize]
	public async Task<IActionResult> ChangeEmail()
	{

		return Ok();
	}
}