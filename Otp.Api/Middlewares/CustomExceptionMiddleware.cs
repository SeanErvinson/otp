using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Otp.Application.Common.Exceptions;

namespace Otp.Api.Middlewares;

public class CustomExceptionMiddleware
{
	private readonly IDictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;
	private readonly ILogger<CustomExceptionMiddleware> _logger;
	private readonly RequestDelegate _next;

	public CustomExceptionMiddleware(RequestDelegate next,
									ILogger<CustomExceptionMiddleware> logger)
	{
		_next = next;
		_logger = logger;
		_exceptionHandlers = new Dictionary<Type, Func<HttpContext, Exception, Task>>
		{
			{ typeof(NotFoundException), HandleNotFoundException },
			{ typeof(InvalidRequestException), HandleInvalidRequestException },
			{ typeof(ExpiredResourceException), HandleExpiredResourceException },
			{ typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException }
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
		context.Response.ContentType = MediaTypeNames.Application.Json;
		var type = exception.GetType();
		if (_exceptionHandlers.ContainsKey(type))
		{
			await _exceptionHandlers[type].Invoke(context, exception);
		}
		else
		{
			_logger.LogError(exception, $"Unexpected error occured: {context.Connection.Id}");
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			await context.Response.WriteAsync(exception.Message);
		}
	}

	private async Task HandleNotFoundException(HttpContext context, Exception ex)
	{
		context.Response.StatusCode = StatusCodes.Status404NotFound;
		var exception = ex as NotFoundException;

		var details = new ProblemDetails
		{
			Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
			Title = "The specified resource was not found.",
			Detail = exception.Message
		};
		var result = JsonSerializer.Serialize(details, new JsonSerializerOptions { WriteIndented = true });
		await context.Response.WriteAsync(result);
	}

	private async Task HandleInvalidRequestException(HttpContext context, Exception ex)
	{
		context.Response.StatusCode = StatusCodes.Status400BadRequest;
		var exception = ex as InvalidRequestException;

		var details = new ValidationProblemDetails
		{
			Detail = exception.Message
		};
		var result = JsonSerializer.Serialize(details, new JsonSerializerOptions { WriteIndented = true });
		await context.Response.WriteAsync(result);
	}
	
	private async Task HandleUnauthorizedAccessException(HttpContext context, Exception ex)
	{
		context.Response.StatusCode = StatusCodes.Status401Unauthorized;
		var exception = ex as UnauthorizedAccessException;

		var details = new ValidationProblemDetails
		{
			Title = "Unauthorized access",
			Detail = exception.Message
		};
		var result = JsonSerializer.Serialize(details, new JsonSerializerOptions { WriteIndented = true });
		await context.Response.WriteAsync(result);
	}	
	
	private async Task HandleExpiredResourceException(HttpContext context, Exception ex)
	{
		context.Response.StatusCode = StatusCodes.Status410Gone;
		var exception = ex as ExpiredResourceException;

		var details = new ValidationProblemDetails
		{
			Type = "https://tools.ietf.org/html/rfc7231#section-6.5.9",
			Title = "Resource has expired or is no longer available.",
			Detail = exception.Message
		};
		var result = JsonSerializer.Serialize(details, new JsonSerializerOptions { WriteIndented = true });
		await context.Response.WriteAsync(result);
	}
}