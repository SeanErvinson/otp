using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.App.Common.Responses;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;

namespace Otp.Application.App.Queries.GetApp;

public sealed record GetApp(Guid Id) : IRequest<AppResponse>;

public class GetAppHandler : IRequestHandler<GetApp, AppResponse>
{
	private readonly IApplicationDbContext _applicationDbContext;
	private readonly ICurrentUserService _currentUserService;

	public GetAppHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
	{
		_applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
		_currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
	}

	public async Task<AppResponse> Handle(GetApp request, CancellationToken cancellationToken)
	{
		var app = await _applicationDbContext.Apps.SingleOrDefaultAsync(app => app.Id == request.Id &&
				app.PrincipalId ==
				_currentUserService.PrincipalId &&
				app.Status != AppStatus.Deleted,
			cancellationToken);

		if (app is null)
		{
			throw new NotFoundException("AppNotFound", "App was not found");
		}
		return new AppResponse
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
		};
	}
}