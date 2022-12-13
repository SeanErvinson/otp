using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Otp.Application.Site.Queries.GetEmailPricingTable;
using Otp.Application.Site.Queries.GetSmsPricingTable;

namespace Otp.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
public class SiteController : ControllerBase
{
	private readonly IMediator _mediator;

	public SiteController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("sms-pricing")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetSmsPricingResponse))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetSmsPricingTable()
	{
		var result = await _mediator.Send(new GetSmsPricingTable());
		return Ok(result);
	}

	[HttpGet("email-pricing")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEmailPricingResponse))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetEmailPricing()
	{
		var result = await _mediator.Send(new GetEmailPricingTable());
		return Ok(result);
	}
}