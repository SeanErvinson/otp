using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.ValueObjects;

namespace Otp.Application.Otp.Queries.GetOtp;

public record GetOtp(Guid Id) : IRequest<GetOtpResponse>
{
	public class Handler : IRequestHandler<GetOtp, GetOtpResponse>
	{
		private readonly IApplicationDbContext _dbContext;
		private readonly ICurrentUserService _currentUserService;

		public Handler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
		{
			_dbContext = dbContext;
			_currentUserService = currentUserService;
		}

		public async Task<GetOtpResponse> Handle(GetOtp requestConfig,
			CancellationToken cancellationToken)
		{
			var otpRequest =
				await _dbContext.OtpRequests.AsNoTracking()
					.FirstOrDefaultAsync(otpRequest =>
							otpRequest.Id == requestConfig.Id,
						cancellationToken);

			if (otpRequest is null)
			{
				throw new NotFoundException($"Otp request {requestConfig.Id} does not exist");
			}
			var app = await _dbContext.Apps.AsNoTracking()
				.SingleOrDefaultAsync(app =>
						app.Id == otpRequest.AppId &&
						app.PrincipalId == _currentUserService.PrincipalId,
					cancellationToken);

			if (app is null)
			{
				throw new UnauthorizedAccessException("Resource does not belong to the user");
			}
			return new GetOtpResponse
			{
				Id = otpRequest.Id,
				Attempts =
					otpRequest.OtpAttempts.Select(attempt => new OtpAttemptResponse
					{
						AttemptedOn = attempt.AttemptedOn, AttemptStatus = attempt.AttemptStatus
					}),
				Channel = otpRequest.Channel,
				ClientInfo = new ClientInfoResponse
				{
					IpAddress = otpRequest.ClientInfo?.IpAddress,
					Referrer = otpRequest.ClientInfo?.Referrer,
					UserAgent = otpRequest.ClientInfo?.UserAgent
				},
				ExpiresOn = otpRequest.ExpiresOn,
				MaxAttempts = otpRequest.MaxAttempts,
				Recipient = otpRequest.Recipient,
				RequestedAt = otpRequest.CreatedAt,
				ResendCount = otpRequest.ResendCount,
				Timeline =
					otpRequest.Timeline.Select(@event => new OtpEventResponse
					{
						State = @event.State,
						OccuredAt = @event.OccuredAt,
						Response = @event.Response,
						Status = @event.Status
					}),
			};
		}
	}
}

public record GetOtpResponse
{
	public Guid Id { get; init; }
	public Channel Channel { get; init; }
	public string Recipient { get; init; } = default!;
	public DateTime RequestedAt { get; init; }
	public IEnumerable<OtpEventResponse> Timeline { get; init; } = default!;
	public int ResendCount { get; init; }
	public int MaxAttempts { get; init; }
	public IEnumerable<OtpAttemptResponse> Attempts { get; init; } = default!;
	public DateTime ExpiresOn { get; init; }
	public ClientInfoResponse ClientInfo { get; init; } = default!;
}

public record ClientInfoResponse
{
	public string? IpAddress { get; init; }
	public string? Referrer { get; init; }
	public string? UserAgent { get; init; }
}

public record OtpEventResponse
{
	public EventState State { get; init; }
	public DateTime OccuredAt { get; init; }
	public string? Response { get; init; }
	public EventStatus Status { get; init; }
}

public record OtpAttemptResponse
{
	public DateTime AttemptedOn { get; init; }
	public OtpAttemptStatus AttemptStatus { get; init; }
}