using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Otp.Api.Filters;
using Otp.Application.Otp.Commands.CancelOtp;
using Otp.Application.Otp.Commands.RequestOtp;
using Otp.Application.Otp.Commands.ResendOtp;
using Otp.Application.Otp.Commands.VerifyCode;
using Otp.Application.Otp.Queries.GetOtp;
using Otp.Application.Otp.Queries.GetOtpRequestConfig;

namespace Otp.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class OtpController : ControllerBase
{
	private readonly IMediator _mediator;

	public OtpController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	/// Request an email OTP.
	/// </summary>
	/// <param name="request"></param>
	/// <returns>The request id and the uri to the OTP verification page.</returns>
	/// <remarks>
	/// Sample request:
	///
	///     POST /api/otp/email
	///     {
	///        "successUrl": "example.com/success",
	///        "cancelUrl": "example.com/cancel",
	///        "emailAddress": hello@example.com
	///     }
	/// 
	/// </remarks>
	/// <response code="200">Returns an OTP response</response>
	/// <response code="400">If success or cancel is/are invalid and/or email is invalid.</response>
	/// <response code="401">If request is not unauthorized</response>
	[HttpPost("email")]
	[ApiKeyAuthorize]
	[ApiExplorerSettings(IgnoreApi = false)]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequestOtpResponse))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> RequestEmail([FromBody] RequestOtpEmailRequest request)
	{
		var result = await _mediator.Send(RequestOtp.Email(request.EmailAddress, request.SuccessUrl, request.CancelUrl));
		return Ok(result);
	}

	/// <summary>
	/// Request an sms OTP.
	/// </summary>
	/// <param name="request"></param>
	/// <returns>The request id and the uri to the OTP verification page.</returns>
	/// <remarks>
	/// Sample request:
	///
	///     POST /api/otp/sms
	///     {
	///        "successUrl": "example.com/success",
	///        "cancelUrl": "example.com/cancel",
	///        "phoneNumber": 639123456789
	///     }
	/// 
	/// </remarks>
	/// <response code="200">Returns an OTP response</response>
	/// <response code="400">If success or cancel url is/are invalid and/or phone number is invalid.</response>
	/// <response code="401">If request is not unauthorized</response>
	[HttpPost("sms")]
	[ApiKeyAuthorize]
	[ApiExplorerSettings(IgnoreApi = false)]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequestOtpResponse))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> RequestSms([FromBody] RequestOtpSmsRequest request)
	{
		var result = await _mediator.Send(RequestOtp.Sms(request.PhoneNumber, request.SuccessUrl, request.CancelUrl));
		return Ok(result);
	}

	[HttpGet("{id:guid}/config")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOtpConfigResponse))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetRequestConfig([FromRoute] Guid id,
		[FromQuery] GetOtpRequestConfigQueryRequest requestConfig)
	{
		var result = await _mediator.Send(new GetOtpConfig(id, requestConfig.Key));
		return Ok(result);
	}

	[HttpGet("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOtpConfigResponse))]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetRequest([FromRoute] GetOtp request)
	{
		var result = await _mediator.Send(request);
		return Ok(result);
	}

	[HttpPost("verify")]
	[OtpKeyAuthorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VerifyCodeResponse))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> VerifyCode([FromBody] VerifyCode request)
	{
		var result = await _mediator.Send(request);
		return Ok(result);
	}

	[HttpPost("cancel")]
	[OtpKeyAuthorize]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CancelOtpResponse))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Cancel([FromBody] CancelOtp otp)
	{
		var result = await _mediator.Send(otp);
		return Ok(result);
	}

	[HttpPost("resend")]
	[OtpKeyAuthorize]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)] //TODO: Return bad request if resend too fast
	public async Task<IActionResult> RequestResend([FromBody] ResendOtp request)
	{
		await _mediator.Send(request);
		return NoContent();
	}
}