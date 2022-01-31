using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains;
using Otp.Core.Domains.Entities;
using Otp.Core.Utils;

namespace Otp.Application.App.Commands.CreateApp;

public record CreateAppCommand(string Name, string? Description, IEnumerable<string>? Tags) : IRequest<CreateAppCommandDto>
{
	public class Handler : IRequestHandler<CreateAppCommand, CreateAppCommandDto>
	{
		private readonly IApplicationDbContext _applicationDbContext;
		private readonly ICurrentUserService _currentUserService;

		public Handler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
		{
			_applicationDbContext = applicationDbContext;
			_currentUserService = currentUserService;
		}

		public async Task<CreateAppCommandDto> Handle(CreateAppCommand request, CancellationToken cancellationToken)
		{
			var count = await _applicationDbContext.Apps.CountAsync(app => app.PrincipalId == _currentUserService.PrincipalId
																			&& app.Name == request.Name
																			&& app.Status != AppStatus.Deleted, cancellationToken);
			if (count != 0)
				throw new InvalidRequestException("App already exists.");

			var generatedApiKey = CryptoUtil.GenerateKey();
			var newApp = new Core.Domains.Entities.App(_currentUserService.PrincipalId, request.Name, generatedApiKey, request.Tags?.ToList(), request.Description);
			var result = await _applicationDbContext.Apps.AddAsync(newApp, cancellationToken);
			await _applicationDbContext.SaveChangesAsync(cancellationToken);

			return new CreateAppCommandDto(result.Entity.Id, generatedApiKey);
		}
	}
}

public record CreateAppCommandDto(Guid Id, string ApiKey);