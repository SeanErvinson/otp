using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;

namespace Otp.Application.Principal.Queries.GetCurrentPrincipal;

public sealed record GetCurrentPrincipal : IRequest<GetCurrentPrincipalResponse>;

public class GetCurrentPrincipalHandler : IRequestHandler<GetCurrentPrincipal, GetCurrentPrincipalResponse>
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IApplicationDbContext _applicationDbContext;

	public GetCurrentPrincipalHandler(ICurrentUserService currentUserService, IApplicationDbContext applicationDbContext)
	{
		_currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
		_applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
	}

	public async Task<GetCurrentPrincipalResponse> Handle(GetCurrentPrincipal request, CancellationToken cancellationToken)
	{
		var principal = await _applicationDbContext.Principals.AsNoTracking()
			.FirstOrDefaultAsync(principal => principal.UserId == _currentUserService.UserId &&
					principal.Status != PrincipalStatus.Deleted,
				cancellationToken);

		if (principal is null)
		{
			throw new NotFoundException("Principal does not exists");
		}
		return new GetCurrentPrincipalResponse();
	}
}

public sealed record GetCurrentPrincipalResponse;