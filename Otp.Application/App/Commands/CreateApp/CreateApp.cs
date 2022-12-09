using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.App.Common.Responses;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;
using Otp.Core.Utils;

namespace Otp.Application.App.Commands.CreateApp;

public sealed record CreateApp(string Name, string? Description, IEnumerable<string>? Tags) : IRequest<CreateAppResponse>;

public class CreateAppHandler : IRequestHandler<CreateApp, CreateAppResponse>
{
	private readonly IApplicationDbContext _applicationDbContext;
	private readonly ICurrentUserService _currentUserService;

	public CreateAppHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
	{
		_applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
		_currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
	}

	public async Task<CreateAppResponse> Handle(CreateApp request, CancellationToken cancellationToken)
	{
		var count = await _applicationDbContext.Apps.CountAsync(app => app.PrincipalId == _currentUserService.PrincipalId &&
				app.Name == request.Name &&
				app.Status != AppStatus.Deleted,
			cancellationToken);

		if (count != 0)
		{
			throw new InvalidRequestException("App already exists.");
		}

		var generatedApiKey = CryptoUtil.GenerateKey();
		var app = new Core.Domains.Entities.App(_currentUserService.PrincipalId,
			request.Name,
			generatedApiKey,
			request.Tags?.ToList(),
			request.Description);

		await _applicationDbContext.Apps.AddAsync(app, cancellationToken);
		await _applicationDbContext.SaveChangesAsync(cancellationToken);
		return new CreateAppResponse(new AppResponse
			{
				Id = app.Id,
				Name = app.Name,
				Tags = app.Tags,
				BackgroundUrl = app.Branding.BackgroundUrl,
				LogoUrl = app.Branding.LogoUrl,
				Description = app.Description,
				CallbackUrl = app.CallbackUrl,
				CreatedAt = app.CreatedAt,
				UpdatedAt = app.UpdatedAt
			},
			generatedApiKey);
	}
}

public sealed record CreateAppResponse(AppResponse App, string ApiKey);