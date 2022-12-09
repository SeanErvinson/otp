using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;

namespace Otp.Application.App.Commands.RegenerateApiKey;

public sealed record RegenerateApiKey(Guid Id) : IRequest<RegenerateApiKeyResponse>;

public class RegenerateApiKeyHandler : IRequestHandler<RegenerateApiKey, RegenerateApiKeyResponse>
{
	private readonly IApplicationDbContext _applicationDbContext;
	private readonly ICurrentUserService _currentUserService;

	public RegenerateApiKeyHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
	{
		_applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
		_currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
	}

	public async Task<RegenerateApiKeyResponse> Handle(RegenerateApiKey request, CancellationToken cancellationToken)
	{
		var app = await _applicationDbContext.Apps.SingleOrDefaultAsync(app => app.Id == request.Id &&
				app.PrincipalId ==
				_currentUserService.PrincipalId,
			cancellationToken);

		if (app is null)
		{
			throw new NotFoundException(nameof(app));
		}
		var apiKey = app.RegenerateApiKey();
		await _applicationDbContext.SaveChangesAsync(cancellationToken);
		return new RegenerateApiKeyResponse(apiKey);
	}
}

public sealed record RegenerateApiKeyResponse(string ApiKey);