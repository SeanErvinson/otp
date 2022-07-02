using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Common.Enums;

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
				Channel = otpRequest.Channel,
				Recipient = otpRequest.Recipient,
				RequestedAt = otpRequest.CreatedAt,
				Timeline = Array.Empty<RequestEventResponse>(),
				ResendCount = otpRequest.ResendCount,
				MaxAttempts = otpRequest.MaxAttempts,
				ExpiresOn = otpRequest.ExpiresOn,
				ClientInfo = new ClientInfoResponse
				{
					IpAddress = otpRequest.ClientInfo?.IpAddress,
					Referrer = otpRequest.ClientInfo?.Referrer,
					UserAgent = otpRequest.ClientInfo?.UserAgent,
				},
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
	public IEnumerable<RequestEventResponse> Timeline { get; init; } = default!;
	public int ResendCount { get; init; }
	public int MaxAttempts { get; init; }
	public string? ErrorMessage { get; init; }
	public DateTime ExpiresOn { get; init; }
	public ClientInfoResponse ClientInfo { get; init; } = default!;
}

public record ClientInfoResponse
{
	public string? IpAddress { get; init; }
	public string? Referrer { get; init; }
	public string? UserAgent { get; init; }
}

public record RequestEventResponse
{

}