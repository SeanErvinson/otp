using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Otp.Application.Common.Exceptions;

namespace Otp.Api.Middlewares;

public class CustomExceptionMiddleware
{
	private readonly IDictionary<Type, int> _exceptionHandlers;
	private readonly ILogger<CustomExceptionMiddleware> _logger;
	private readonly RequestDelegate _next;
	private readonly ProblemDetailsFactory _problemDetailsFactory;

	public CustomExceptionMiddleware(RequestDelegate next,
		ILogger<CustomExceptionMiddleware> logger,
		ProblemDetailsFactory problemDetailsFactory)
	{
		_next = next;
		_logger = logger;
		_problemDetailsFactory = problemDetailsFactory;
		_exceptionHandlers = new Dictionary<Type, int>
		{
			{ typeof(NotFoundException), StatusCodes.Status404NotFound },
			{ typeof(InvalidRequestException), StatusCodes.Status400BadRequest },
			{ typeof(ExpiredResourceException), StatusCodes.Status410Gone },
			{ typeof(UnauthorizedAccessException), StatusCodes.Status401Unauthorized }
		};
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex);
		}
	}

	private async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/problem+json";
		var type = exception.GetType();

		if (_exceptionHandlers.TryGetValue(type, out var statusCode) && exception is BusinessException ex)
		{
			await WriteProblemDetails(context, statusCode, ex);
		}
		else
		{
			_logger.LogError(exception, "Unexpected error occured: {ConnectionId}", context.Connection.Id);
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			await context.Response.WriteAsync(exception.Message);
		}
	}

	private async Task WriteProblemDetails(HttpContext context, int statusCode, BusinessException ex)
	{
		var problemDetails = _problemDetailsFactory.CreateProblemDetails(context, statusCode, ex.Title, detail: ex.Detail);

		if (ex.AdditionalProperties is not null)
		{
			problemDetails.Extensions[JsonNamingPolicy.CamelCase.ConvertName(nameof(ex.AdditionalProperties))] =
				ex.AdditionalProperties;
		}
		await Results.Problem(problemDetails).ExecuteAsync(context);
	}
}