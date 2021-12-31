using MediatR;
using Otp.Application.Common;
using Otp.Application.Common.Extensions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains;

namespace Otp.Application.App.Queries.GetApps;

public record GetAppsQuery(int PageIndex, int PageSize) : IRequest<PaginatedResult<GetAppSimpleDto>>
{
	public class Handler : IRequestHandler<GetAppsQuery, PaginatedResult<GetAppSimpleDto>>
	{
		private readonly IApplicationDbContext _applicationDbContext;
		private readonly ICurrentUserService _currentUserService;

		public Handler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
		{
			_applicationDbContext = applicationDbContext;
			_currentUserService = currentUserService;
		}

		public async Task<PaginatedResult<GetAppSimpleDto>> Handle(GetAppsQuery request, CancellationToken cancellationToken)
		{
			var apps = await _applicationDbContext.Apps.Where(app => app.PrincipalId == _currentUserService.PrincipalId 
																	&& app.Status != AppStatus.Deleted)
											.Select(app => new GetAppSimpleDto(app.Id, app.Name, app.Description, app.CreatedAt, app.Tags))
											.PaginatedResultAsync(request.PageIndex, request.PageSize);


			return apps;
		}
	}
}

public record GetAppsQueryDto(IEnumerable<GetAppSimpleDto> Apps);

public record GetAppSimpleDto(Guid Id, string Name, string? Description, DateTime CreatedAt, IEnumerable<string> Tags);