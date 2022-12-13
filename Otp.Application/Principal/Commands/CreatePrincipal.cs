using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;
using Serilog;

namespace Otp.Application.Principal.Commands;

public sealed record CreatePrincipal(string ObjectId, string DisplayName) : IRequest<CreatePrincipalResponse>;

public sealed class CreatePrincipalHandler : IRequestHandler<CreatePrincipal, CreatePrincipalResponse>
{
	private readonly IApplicationDbContext _applicationDbContext;
	private readonly IChargebeeService _chargebeeService;
	private readonly ILogger _logger;

	public CreatePrincipalHandler(IApplicationDbContext applicationDbContext,
		IChargebeeService chargebeeService,
		ILogger logger)
	{
		_applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
		_chargebeeService = chargebeeService ?? throw new ArgumentNullException(nameof(chargebeeService));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public async Task<CreatePrincipalResponse> Handle(CreatePrincipal request, CancellationToken cancellationToken)
	{
		Log.Information("Creating a new principal");
		var userExists = await _applicationDbContext.Principals.AnyAsync(principal =>
				principal.UserId == request.ObjectId &&
				(principal.Status == PrincipalStatus.Active ||
					principal.Status ==
					PrincipalStatus.Inactive),
			cancellationToken);

		if (userExists)
		{
			return new CreatePrincipalResponse
			{
				Action = CreatePrincipalResponse.CreatePrincipalCommandActions.ValidationError,
				UserMessage = "User already exists",
				StatusCode = (int)HttpStatusCode.BadRequest
			};
		}

		string customerId;

		try
		{
			// TODO Add a retry policy
			var response = await _chargebeeService.CreateCustomerAsync(cancellationToken);
			customerId = response.Customer.Id;
		}
		catch (Exception e)
		{
			_logger.Error(e, "Chargebee failed to create a customer");
			return new CreatePrincipalResponse
			{
				Action = CreatePrincipalResponse.CreatePrincipalCommandActions.ShowBlockPage,
				UserMessage = "There was a problem with your request. You are not able to sign up at this time."
			};
		}

		var principal = new Core.Domains.Entities.Principal(request.DisplayName, request.ObjectId, customerId);
		await _applicationDbContext.Principals.AddAsync(principal, cancellationToken);
		await _applicationDbContext.SaveChangesAsync(cancellationToken);

		return new CreatePrincipalResponse { Action = CreatePrincipalResponse.CreatePrincipalCommandActions.Continue };
	}
}

/// <summary>
/// B2C API Connector expected continuation response
/// Check <see href="https://learn.microsoft.com/en-us/azure/active-directory-b2c/add-api-connector?pivots=b2c-user-flow#continuation-response-2">here</see>
/// </summary>
public sealed record CreatePrincipalResponse
{
	public string Version => "1.0.0";
	public CreatePrincipalCommandActions Action { get; init; }
	public string? UserMessage { get; init; }
	public int? StatusCode { get; init; }

	public enum CreatePrincipalCommandActions
	{
		Continue,
		ShowBlockPage,
		ValidationError
	}
}