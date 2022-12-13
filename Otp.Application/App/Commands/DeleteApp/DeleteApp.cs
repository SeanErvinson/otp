using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;

namespace Otp.Application.App.Commands.DeleteApp;

public sealed record DeleteApp(Guid Id) : IRequest;

public class DeleteAppHandler : IRequestHandler<DeleteApp>
{
	private readonly IApplicationDbContext _applicationDbContext;
	private readonly ICurrentUserService _currentUserService;

	public DeleteAppHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
	{
		_applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
		_currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
	}

	public async Task<Unit> Handle(DeleteApp request, CancellationToken cancellationToken)
	{
		var app = await _applicationDbContext.Apps.SingleOrDefaultAsync(app => app.Id == request.Id &&
				app.PrincipalId ==
				_currentUserService.PrincipalId,
			cancellationToken);

		if (app is null)
		{
			throw new NotFoundException(nameof(app));
		}
		app.MarkAsDeleted();
		await _applicationDbContext.SaveChangesAsync(cancellationToken);
		return Unit.Value;
	}
}