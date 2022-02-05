using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;

namespace Otp.Application.App.Commands.RegenerateApiKey;

public record RegenerateApiKeyCommand(Guid Id) : IRequest<RegenerateApiKeyDto>
{
	public class Handler : IRequestHandler<RegenerateApiKeyCommand, RegenerateApiKeyDto>
	{
		private readonly IApplicationDbContext _applicationDbContext;
		private readonly ICurrentUserService _currentUserService;

		public Handler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
		{
			_applicationDbContext = applicationDbContext;
			_currentUserService = currentUserService;
		}

		public async Task<RegenerateApiKeyDto> Handle(RegenerateApiKeyCommand request, CancellationToken cancellationToken)
		{
			var app = await _applicationDbContext.Apps.SingleOrDefaultAsync(app => app.Id == request.Id && app.PrincipalId == _currentUserService.PrincipalId,
																			cancellationToken);
			if (app is null) throw new NotFoundException(nameof(app));

			var apiKey = app.RegenerateApiKey();

			await _applicationDbContext.SaveChangesAsync(cancellationToken);

			return new RegenerateApiKeyDto(apiKey);
		}
	}
}

public record RegenerateApiKeyDto(string ApiKey);