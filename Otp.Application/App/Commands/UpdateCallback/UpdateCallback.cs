using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.App.Common.Responses;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;

namespace Otp.Application.App.Commands.UpdateCallback;

public sealed record UpdateCallbackRequest(string CallbackUrl, string? EndpointSecret);
public sealed record UpdateCallback(Guid Id, string CallbackUrl, string? EndpointSecret) : IRequest<AppResponse>;

public class UpdateCallbackHandler : IRequestHandler<UpdateCallback, AppResponse>
{
	private readonly IApplicationDbContext _applicationDbContext;
	private readonly ICurrentUserService _currentUserService;

	public UpdateCallbackHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
	{
		_applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
		_currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
	}

	public async Task<AppResponse> Handle(UpdateCallback request, CancellationToken cancellationToken)
	{
		var app = await _applicationDbContext.Apps.SingleOrDefaultAsync(app => app.Id == request.Id &&
				app.PrincipalId ==
				_currentUserService.PrincipalId &&
				app.Status != AppStatus.Deleted,
			cancellationToken);

		if (app is null)
		{
			throw new NotFoundException(nameof(app));
		}
		app.UpdateCallbackUrl(request.CallbackUrl, request.EndpointSecret);
		await _applicationDbContext.SaveChangesAsync(cancellationToken);
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