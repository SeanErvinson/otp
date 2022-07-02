using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;

namespace Otp.Application.App.Commands.DeleteApp;

public record DeleteApp(Guid Id) : IRequest
{
	public class Handler : IRequestHandler<DeleteApp>
	{
		private readonly IApplicationDbContext _applicationDbContext;
		private readonly ICurrentUserService _currentUserService;

		public Handler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
		{
			_applicationDbContext = applicationDbContext;
			_currentUserService = currentUserService;
		}

		public async Task<Unit> Handle(DeleteApp request, CancellationToken cancellationToken)
		{
			var app = await _applicationDbContext.Apps.SingleOrDefaultAsync(app => app.Id == request.Id && app.PrincipalId == _currentUserService.PrincipalId,
																			cancellationToken);
			if (app is null) throw new NotFoundException(nameof(app));

			app.MarkAsDeleted();
			await _applicationDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}