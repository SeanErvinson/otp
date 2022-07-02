using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Otp.Application.Principal.Commands;
using Otp.Application.Principal.Queries.GetCurrentPrincipal;

namespace Otp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
public class PrincipalController : ControllerBase
{
	private readonly IMediator _mediator;
	
	public PrincipalController(IMediator mediator)
	{
		_mediator = mediator;
	}
	
	[HttpGet("current")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCurrentPrincipalResponse))]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetCurrentPrincipal()
	{
		var result = await _mediator.Send(new GetCurrentPrincipal());
		return Ok(result);
	}
	
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> CreatePrincipal([FromBody] CreatePrincipal request)
	{
		var result = await _mediator.Send(request);
		return Ok(result);
	}
}