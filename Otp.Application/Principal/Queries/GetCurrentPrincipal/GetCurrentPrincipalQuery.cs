using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Entities;

namespace Otp.Application.Principal.Queries.GetCurrentPrincipal;

public record GetCurrentPrincipalQuery : IRequest<GetCurrentPrincipalQueryDto>
{
	public class Handler : IRequestHandler<GetCurrentPrincipalQuery, GetCurrentPrincipalQueryDto>
	{
		private readonly ICurrentUserService _currentUserService;
		private readonly IApplicationDbContext _applicationDbContext;

		public Handler(ICurrentUserService currentUserService, IApplicationDbContext applicationDbContext)
		{
			_currentUserService = currentUserService;
			_applicationDbContext = applicationDbContext;
		}

		public async Task<GetCurrentPrincipalQueryDto> Handle(GetCurrentPrincipalQuery request, CancellationToken cancellationToken)
		{
			var principal = await _applicationDbContext.Principals.AsNoTracking()
				.FirstOrDefaultAsync(principal => principal.UserId == _currentUserService.UserId &&
						principal.Status != PrincipalStatus.Deleted,
					cancellationToken);
			if (principal is null)
			{
				throw new NotFoundException("Principal does not exists");
			}

			return new GetCurrentPrincipalQueryDto();
		}
	}
}

public record GetCurrentPrincipalQueryDto();