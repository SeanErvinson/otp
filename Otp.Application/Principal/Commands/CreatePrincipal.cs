using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;
using Serilog;

namespace Otp.Application.Principal.Commands;

public record CreatePrincipal : IRequest<CreatePrincipalResponse>
{
	public string ObjectId { get; init; } = default!;
	public string DisplayName { get; init; } = default!;

	public class Handler : IRequestHandler<CreatePrincipal, CreatePrincipalResponse>
	{
		private readonly IApplicationDbContext _applicationDbContext;

		public Handler(IApplicationDbContext applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
		}

		public async Task<CreatePrincipalResponse> Handle(CreatePrincipal request, CancellationToken cancellationToken)
		{
			Log.Information("Creating a new principal");
			var userExists = await _applicationDbContext.Principals.AnyAsync(principal =>
					principal.UserId == request.ObjectId &&
					(principal.Status == PrincipalStatus.Active || principal.Status == PrincipalStatus.Inactive),
				cancellationToken: cancellationToken);

			if (userExists)
			{
				return new CreatePrincipalResponse
				{
					Action = CreatePrincipalCommandActions.ValidationError,
					UserMessage = "User already exists",
					StatusCode = (int)HttpStatusCode.BadRequest,
				};
			}

			await _applicationDbContext.Principals.AddAsync(
				new Core.Domains.Entities.Principal(request.DisplayName, request.ObjectId),
				cancellationToken);

			await _applicationDbContext.SaveChangesAsync(cancellationToken);

			return new CreatePrincipalResponse
			{
				Action = CreatePrincipalCommandActions.Continue
			};
		}
	}
}

public enum CreatePrincipalCommandActions
{
	Continue,
	ShowBlockPage,
	ValidationError
}

public record CreatePrincipalResponse
{
	public string Version => "1.0.0";
	public CreatePrincipalCommandActions Action { get; init; }
	public string? UserMessage { get; init; }
	public int? StatusCode { get; init; }
}