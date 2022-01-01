using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains;

namespace Otp.Application.App.Commands.UpdateCallback;

public record UpdateCallbackCommand(Guid Id, string CallbackUrl, string? EndpointSecret) : IRequest
{
	public class Handler : IRequestHandler<UpdateCallbackCommand>
	{
		private readonly IApplicationDbContext _applicationDbContext;
		private readonly ICurrentUserService _currentUserService;

		public Handler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
		{
			_applicationDbContext = applicationDbContext;
			_currentUserService = currentUserService;
		}

		public async Task<Unit> Handle(UpdateCallbackCommand request, CancellationToken cancellationToken)
		{
			
			var app = await _applicationDbContext.Apps.SingleOrDefaultAsync(app => app.Id == request.Id 
																					&& app.PrincipalId == _currentUserService.PrincipalId 
																					&& app.Status != AppStatus.Deleted,
																			cancellationToken);
			if (app is null) throw new NotFoundException(nameof(app));

			app.UpdateCallbackUrl(request.CallbackUrl, request.EndpointSecret);
			await _applicationDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}