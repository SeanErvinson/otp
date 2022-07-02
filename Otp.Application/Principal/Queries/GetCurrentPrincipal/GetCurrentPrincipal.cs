using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;

namespace Otp.Application.Principal.Queries.GetCurrentPrincipal;

public record GetCurrentPrincipal : IRequest<GetCurrentPrincipalResponse>
{
	public class Handler : IRequestHandler<GetCurrentPrincipal, GetCurrentPrincipalResponse>
	{
		private readonly ICurrentUserService _currentUserService;
		private readonly IApplicationDbContext _applicationDbContext;

		public Handler(ICurrentUserService currentUserService, IApplicationDbContext applicationDbContext)
		{
			_currentUserService = currentUserService;
			_applicationDbContext = applicationDbContext;
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
}

public record GetCurrentPrincipalResponse();