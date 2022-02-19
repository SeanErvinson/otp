using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Entities;

namespace Otp.Application.App.Queries.GetAppRecentCallbacks;

public record GetAppRecentCallbacks(Guid Id) : IRequest<IEnumerable<GetAppRecentCallbacksDto>>
{
	public class Handler : IRequestHandler<GetAppRecentCallbacks, IEnumerable<GetAppRecentCallbacksDto>>
	{
		private const int MaxCallbackEventCount = 30;
		private readonly IApplicationDbContext _applicationDbContext;
		private readonly ICurrentUserService _currentUserService;

		public Handler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
		{
			_applicationDbContext = applicationDbContext;
			_currentUserService = currentUserService;
		}

		public async Task<IEnumerable<GetAppRecentCallbacksDto>> Handle(GetAppRecentCallbacks request, CancellationToken cancellationToken)
		{
			var app = await _applicationDbContext.Apps.AsNoTracking()
												.CountAsync(app => app.Id == request.Id
																	&& app.PrincipalId == _currentUserService.PrincipalId
																	&& app.Status != AppStatus.Deleted,
															cancellationToken);
			if (app == 0) throw new NotFoundException(nameof(app));

			var callbackEvents = _applicationDbContext.CallbackEvents.AsNoTracking()
													.Where(callback => callback.AppId == request.Id)
													.OrderByDescending(callback => callback.CreatedAt)
													.Take(MaxCallbackEventCount)
													.ToList();

			return callbackEvents.Select(c => new GetAppRecentCallbacksDto
			{
				Channel = c.Channel,
				RequestId = c.RequestId,
				CreatedAt = c.CreatedAt,
				ResponseMessage = c.ResponseMessage,
				StatusCode = c.StatusCode,
			});
		}
	}
}

public record GetAppRecentCallbacksDto
{
	public Guid RequestId { get; init; }
	public DateTime CreatedAt { get; init; }
	public int StatusCode { get; init; }
	public Channel Channel { get; init; }
	public string? ResponseMessage { get; init; }
}