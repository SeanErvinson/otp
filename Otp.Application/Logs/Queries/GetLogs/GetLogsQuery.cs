using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
		private const int ItemsPerPage = 2;
		private readonly IApplicationDbContext _applicationDbContext;
		private readonly ICurrentUserService _currentUserService;
		private readonly JsonSerializerOptions _jsonSerializerOptions;
		private readonly ILogger<Handler> _logger;

		public Handler(IApplicationDbContext applicationDbContext,
			ICurrentUserService currentUserService,
			JsonSerializerOptions jsonSerializerOptions,
			ILogger<Handler> logger)
		{
			_applicationDbContext = applicationDbContext;
			_currentUserService = currentUserService;
			_jsonSerializerOptions = jsonSerializerOptions;
			_logger = logger;
		}

		public async Task<CursorResult<GetLogsQueryDto>> Handle(GetLogsQuery request,
			CancellationToken cancellationToken)
		{
			var encodedCursor = request.After ?? request.Before ?? string.Empty;
			LogCursor? cursor = null;
			
			if (!string.IsNullOrWhiteSpace(encodedCursor))
			{
				if (!StringUtils.TryBase64Decode(encodedCursor, out var cursorDirection))
				{
					throw new InvalidRequestException("Invalid cursor");
				}

				cursor = JsonSerializer.Deserialize<LogCursor>(cursorDirection, _jsonSerializerOptions);

				if (cursor is null)
				{
					throw new InvalidRequestException("Invalid cursor");
				}
			}

			var apps = _applicationDbContext.Apps.AsNoTracking()
				.Where(app => app.Principal.UserId == _currentUserService.UserId)
				.Select(app => app.Id)
				.ToList();

			Func<IQueryable<OtpRequest>, IOrderedQueryable<OtpRequest>> query = queryable => queryable.OrderBy(otpRequest => otpRequest.CreatedAt);

			if (cursor is not null)
			{
				if (!string.IsNullOrWhiteSpace(request.Before))
				{
					query = queryable => queryable.Where(otpRequest => otpRequest.CreatedAt < cursor.CreatedAt ||
							(otpRequest.CreatedAt == cursor.CreatedAt && otpRequest.Id == cursor.Id))
						.OrderByDescending(otpRequest => otpRequest.CreatedAt);
				}
				else
				{
					query = queryable => queryable.Where(otpRequest => otpRequest.CreatedAt > cursor.CreatedAt ||
							(otpRequest.CreatedAt == cursor.CreatedAt && otpRequest.Id == cursor.Id))
						.OrderBy(otpRequest => otpRequest.CreatedAt);
				}
			}
			
			var appLogs = _applicationDbContext.OtpRequests
				.Where(otpRequest => apps.Contains(otpRequest.AppId));
			var logs = query.Invoke(appLogs)
				.Select(otpRequest => new GetLogsQueryDto
				{
					Id = otpRequest.Id,
					App = new GetLogsQueryAppDto(otpRequest.AppId, otpRequest.App.Name),
					Channel = otpRequest.Channel,
					Receiver = otpRequest.Contact,
					EventDate = otpRequest.CreatedAt,
				});

			var result = await CursorResult<GetLogsQueryDto>.CreateAsync(logs,
				ItemsPerPage,
				dto => StringUtils.Base64Encode(JsonSerializer.Serialize(new LogCursor(dto.Id, dto.EventDate),
					_jsonSerializerOptions)));

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