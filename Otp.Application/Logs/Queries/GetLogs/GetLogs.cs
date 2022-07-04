using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Application.Common.Models;
using Otp.Core.Domains.Common.Enums;

namespace Otp.Application.Logs.Queries.GetLogs;

public record GetLogs(string? Before, string? After) : IRequest<CursorResult<GetLogsResponse>>
{
	public class Handler : IRequestHandler<GetLogs, CursorResult<GetLogsResponse>>
	{
		private const int ItemsPerPage = 10;
		private readonly IApplicationDbContext _applicationDbContext;
		private readonly ICurrentUserService _currentUserService;
		private readonly JsonSerializerOptions _jsonSerializerOptions;

		public Handler(IApplicationDbContext applicationDbContext,
			ICurrentUserService currentUserService,
			JsonSerializerOptions jsonSerializerOptions)
		{
			_applicationDbContext = applicationDbContext;
			_currentUserService = currentUserService;
			_jsonSerializerOptions = jsonSerializerOptions;
		}

		public async Task<CursorResult<GetLogsResponse>> Handle(GetLogs request,
			CancellationToken cancellationToken)
		{
			var encodedCursor = request.After ?? request.Before ?? string.Empty;
			LogCursor? cursor = null;

			if (!string.IsNullOrWhiteSpace(encodedCursor))
			{
				// if (!StringUtils.TryBase64Decode(encodedCursor, out var cursorDirection))
				// {
				// 	throw new InvalidRequestException("Invalid cursor");
				// }
				cursor = JsonSerializer.Deserialize<LogCursor>(encodedCursor, _jsonSerializerOptions);

				if (cursor is null)
				{
					throw new InvalidRequestException("Invalid cursor");
				}
			}
			var apps = _applicationDbContext.Apps.AsNoTracking()
				.Where(app => app.Principal.UserId == _currentUserService.UserId)
				.Select(app => app.Id)
				.ToList();
			var xxx = (IQueryable<GetLogsResponse> queryable) =>
				queryable.OrderByDescending(otpRequest => otpRequest.EventDate);
			var cursorQuery = xxx;

			if (cursor is not null)
			{
				if (!string.IsNullOrWhiteSpace(request.Before))
				{
					cursorQuery = queryable => queryable.Where(otpRequest => otpRequest.EventDate < cursor.CreatedAt ||
							(otpRequest.EventDate == cursor.CreatedAt &&
								otpRequest.Id == cursor.Id))
						.OrderByDescending(otpRequest => otpRequest.EventDate);
				}
				else
				{
					cursorQuery = queryable => queryable.Where(otpRequest => otpRequest.EventDate > cursor.CreatedAt ||
							(otpRequest.EventDate == cursor.CreatedAt &&
								otpRequest.Id == cursor.Id))
						.OrderBy(otpRequest => otpRequest.EventDate);
				}
			}
			var appLogs = _applicationDbContext.OtpRequests
				.Where(otpRequest => apps.Contains(otpRequest.AppId))
				.Select(otpRequest => new GetLogsResponse
				{
					Id = otpRequest.Id,
					App = new GetLogsAppResponse(otpRequest.AppId, otpRequest.App.Name),
					Channel = otpRequest.Channel,
					Receiver = otpRequest.Recipient,
					EventDate = otpRequest.CreatedAt
				});
			var resultQuery = cursorQuery.Invoke(appLogs);
			var beforeAfterFunc = (GetLogsResponse? firstItem, GetLogsResponse? lastItem) =>
			{
				var result = xxx(appLogs)
					.GroupBy(x => 1)
					.Select(g => new
					{
						before = g.OrderByDescending(otpRequest => otpRequest.EventDate)
							.FirstOrDefault(c => lastItem != null && c.EventDate < lastItem.EventDate),
						after = g.OrderByDescending(otpRequest => otpRequest.EventDate)
							.LastOrDefault(c => firstItem != null && c.EventDate > firstItem.EventDate)
					})
					.Single();
				return (result.before, result.after);
			};
			return await CursorResult<GetLogsResponse>.CreateAsync(resultQuery,
				ItemsPerPage,
				otp => otp.EventDate,
				beforeAfterFunc,
				dto => new LogCursor { Id = dto.Id, CreatedAt = dto.EventDate });
		}
	}

	private record LogCursor : ICursor
	{
		public Guid Id { get; init; }
		public DateTime CreatedAt { get; init; }
	};
}

public record GetLogsResponse
{
	public Guid Id { get; init; }
	public GetLogsAppResponse App { get; init; } = default!;
	public DateTime EventDate { get; init; }
	public string Receiver { get; init; } = default!;
	public Channel Channel { get; init; }
}

public record GetLogsAppResponse(Guid AppId, string AppName);