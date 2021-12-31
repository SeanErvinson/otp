using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Otp.Application.Principal.Commands;

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
	
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> CreatePrincipal([FromBody] CreatePrincipalCommand request)
	{
		var result = await _mediator.Send(request);
		return Ok(result);
	}
}