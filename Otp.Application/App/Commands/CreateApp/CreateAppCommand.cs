using MediatR;
using Otp.Application.Common.Interfaces;
using Otp.Core.Utils;

namespace Otp.Application.App.Commands.CreateApp;

public record CreateAppCommand(string Name, string? Description) : IRequest<CreateAppCommandDto>
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
			var generatedApiKey = CryptoUtil.GenerateKey();
			var newApp = new Core.Domains.App(_currentUserService.PrincipalId, request.Name, generatedApiKey, request.Description);
			var result = await _applicationDbContext.Apps.AddAsync(newApp, cancellationToken);
			await _applicationDbContext.SaveChangesAsync(cancellationToken);

			return new CreateAppCommandDto(result.Entity.Id, generatedApiKey);
		}
	}
}

public record CreateAppCommandDto(Guid Id, string ApiKey);