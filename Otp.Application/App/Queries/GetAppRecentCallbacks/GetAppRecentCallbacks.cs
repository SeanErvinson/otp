﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Entities;

namespace Otp.Application.App.Queries.GetAppRecentCallbacks;

public sealed record GetAppRecentCallbacks(Guid Id) : IRequest<IEnumerable<GetAppRecentCallbacksResponse>>;

public class GetAppRecentCallbacksHandler : IRequestHandler<GetAppRecentCallbacks, IEnumerable<GetAppRecentCallbacksResponse>>
{
	private const int MaxCallbackEventCount = 30;
	private readonly IApplicationDbContext _applicationDbContext;
	private readonly ICurrentUserService _currentUserService;

	public GetAppRecentCallbacksHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
	{
		_applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
		_currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
	}

	public async Task<IEnumerable<GetAppRecentCallbacksResponse>> Handle(GetAppRecentCallbacks request,
		CancellationToken cancellationToken)
	{
		var app = await _applicationDbContext.Apps.AsNoTracking()
			.CountAsync(app => app.Id == request.Id &&
					app.PrincipalId == _currentUserService.PrincipalId &&
					app.Status != AppStatus.Deleted,
				cancellationToken);

		if (app == 0)
		{
			throw new NotFoundException(nameof(app));
		}
		var callbackEvents = _applicationDbContext.CallbackEvents.AsNoTracking()
			.Where(callback => callback.AppId == request.Id)
			.OrderByDescending(callback => callback.CreatedAt)
			.Take(MaxCallbackEventCount)
			.ToList();
		return callbackEvents.Select(c => new GetAppRecentCallbacksResponse
		{
			Channel = c.Channel,
			RequestId = c.RequestId,
			CreatedAt = c.CreatedAt,
			ResponseMessage = c.ResponseMessage,
			StatusCode = c.StatusCode
		});
	}
}

public sealed record GetAppRecentCallbacksResponse
{
	public Guid RequestId { get; init; }
	public DateTime CreatedAt { get; init; }
	public int StatusCode { get; init; }
	public Channel Channel { get; init; }
	public string? ResponseMessage { get; init; }
}