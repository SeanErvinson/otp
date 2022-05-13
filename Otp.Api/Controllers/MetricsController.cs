using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Otp.Application.Metric.Queries.GetMetric;

namespace Otp.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
public class MetricsController : ControllerBase
{
	private readonly IMediator _mediator;

	public MetricsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetMetricResponse>))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> GetMetrics([FromQuery] GetMetricQuery query)
	{
		var result = await _mediator.Send(query);
		return Ok(result);
	}
}