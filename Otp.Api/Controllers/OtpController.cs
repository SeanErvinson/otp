﻿using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Otp.Api.Filters;
using Otp.Application.Otp.Commands.RequestOtp;
using Otp.Application.Otp.Commands.ResendOtpRequest;
using Otp.Application.Otp.Commands.VerifyCode;
using Otp.Application.Otp.Queries.GetOtpRequest;

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
	
	[HttpGet("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOtpRequestQueryDto))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetRequest([FromRoute] Guid id, [FromQuery] GetOtpRequestQueryRequest request)
	{
		var result = await _mediator.Send(new GetOtpRequestQuery(id, request.Secret));
		return Ok(result);
	}
	
    [HttpPost("verify")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeCommand request)
    {
        await _mediator.Send(request);
        return NoContent();
    }

	[HttpPost("resend")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)] //TODO: Return bad request if resend too fast
	public async Task<IActionResult> RequestResend([FromBody] ResendOtpRequest request)
	{
		await _mediator.Send(request);
		return NoContent();
	}
}