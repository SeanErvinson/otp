using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Application.Common.Models;
using Otp.Application.Common.Utils;
using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Entities;

namespace Otp.Application.Logs.Queries.GetLogs;

public record GetLogsQuery(string? Before, string? After) : IRequest<CursorResult<GetLogsQueryDto>>
{
	public class Handler : IRequestHandler<GetLogsQuery, CursorResult<GetLogsQueryDto>>
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

		public async Task<CursorResult<GetLogsQueryDto>> Handle(GetLogsQuery request,
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

			Func<IQueryable<OtpRequest>, IOrderedQueryable<OtpRequest>> query = queryable =>
				queryable.OrderByDescending(otpRequest => otpRequest.CreatedAt);

			Func<IQueryable<OtpRequest>, IQueryable<OtpRequest>>? cursorQuery = null;
			if (cursor is not null)
			{
				if (!string.IsNullOrWhiteSpace(request.Before))
				{
					cursorQuery = queryable => queryable.Where(otpRequest => otpRequest.CreatedAt < cursor.CreatedAt ||
							(otpRequest.CreatedAt == cursor.CreatedAt && otpRequest.Id == cursor.Id))
						.OrderByDescending(otpRequest => otpRequest.CreatedAt)
						.Skip(1);
				}
				else
				{
					cursorQuery = queryable => queryable.Where(otpRequest => otpRequest.CreatedAt > cursor.CreatedAt ||
							(otpRequest.CreatedAt == cursor.CreatedAt && otpRequest.Id == cursor.Id))
						.OrderBy(otpRequest => otpRequest.CreatedAt)
						.Skip(1);
				}
			}

			var appLogs = _applicationDbContext.OtpRequests
				.Where(otpRequest => apps.Contains(otpRequest.AppId));
			var orderQuery = cursorQuery ?? query;
			var logs = orderQuery.Invoke(appLogs)
				.Select(otpRequest => new GetLogsQueryDto
				{
					Id = otpRequest.Id,
					App = new GetLogsQueryAppDto(otpRequest.AppId, otpRequest.App.Name),
					Channel = otpRequest.Channel,
					Receiver = otpRequest.Contact,
					EventDate = otpRequest.CreatedAt,
				});

			(bool, bool) HasPredicate(GetLogsQueryDto? firstItem, GetLogsQueryDto? lastItem)
			{
				if (firstItem is null || lastItem is null)
				{
					return (false, false);
				}

				var hasBeforeAfter = query(appLogs)
					.GroupBy(x => 1)
					.Select(g => new
					{
						hasBefore = g.Any(l => l.CreatedAt < lastItem.EventDate),
						hasAfter = g.Any(l => l.CreatedAt > firstItem.EventDate),
					})
					.Single();
				return (hasBeforeAfter.hasBefore, hasBeforeAfter.hasAfter);
			}

			var result = await CursorResult<GetLogsQueryDto>.CreateAsync(logs,
				ItemsPerPage,
				dto => dto.EventDate,
				HasPredicate,
				dto => dto is not null
					? JsonSerializer.Serialize(new LogCursor(dto.Id, dto.EventDate),
						_jsonSerializerOptions)
					: string.Empty);

			return result;
		}
	}

	private record LogCursor(Guid Id, DateTime CreatedAt);
}

public record GetLogsQueryDto()
{
	public Guid Id { get; init; }
	public GetLogsQueryAppDto App { get; init; }
	public DateTime EventDate { get; init; }
	public string Receiver { get; init; }
	public Channel Channel { get; init; }
}

public record GetLogsQueryAppDto(Guid AppId, string AppName);