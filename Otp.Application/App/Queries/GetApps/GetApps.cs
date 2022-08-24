using MediatR;
using Otp.Application.Common.Extensions;
using Otp.Application.Common.Interfaces;
using Otp.Application.Common.Models;
using Otp.Core.Domains.Entities;

namespace Otp.Application.App.Queries.GetApps;

public record GetApps(int PageIndex, int PageSize) : IRequest<PaginatedResult<GetAppSimpleResponse>>
{
	public class Handler : IRequestHandler<GetApps, PaginatedResult<GetAppSimpleResponse>>
	{
		private readonly IApplicationDbContext _applicationDbContext;
		private readonly ICurrentUserService _currentUserService;

		public Handler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
		{
			_applicationDbContext = applicationDbContext;
			_currentUserService = currentUserService;
		}

		public async Task<PaginatedResult<GetAppSimpleResponse>> Handle(GetApps request, CancellationToken cancellationToken)
		{
			var apps = await _applicationDbContext.Apps
				.Where(app => app.PrincipalId == _currentUserService.PrincipalId && app.Status != AppStatus.Deleted)
				.OrderByDescending(app => app.CreatedAt)
				.Select(app => new GetAppSimpleResponse(app.Id, app.Name, app.Description, app.CreatedAt, app.Tags))
				.PaginatedResultAsync(request.PageIndex, request.PageSize);
			return apps;
		}
	}
}

public record GetAppSimpleResponse(Guid Id,
	string Name,
	string? Description,
	DateTime CreatedAt,
	IEnumerable<string>? Tags);