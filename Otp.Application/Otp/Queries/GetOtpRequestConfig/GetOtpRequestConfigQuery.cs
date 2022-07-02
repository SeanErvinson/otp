using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;
using Otp.Core.Domains.ValueObjects;

namespace Otp.Application.Otp.Queries.GetOtpRequest;

public record GetOtpRequestConfigQueryRequest(string Key);

public record GetOtpRequestConfigQuery(Guid Id, string Key) : IRequest<GetOtpRequestConfigQueryResponse>
{
	public class Handler : IRequestHandler<GetOtpRequestConfigQuery, GetOtpRequestConfigQueryResponse>
	{
		private readonly IApplicationDbContext _dbContext;

		public Handler(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<GetOtpRequestConfigQueryResponse> Handle(GetOtpRequestConfigQuery requestConfig,
			CancellationToken cancellationToken)
		{
			var otpRequest =
				await _dbContext.OtpRequests.AsNoTracking()
					.Include(otpRequest => otpRequest.App)
					.FirstOrDefaultAsync(req =>
							req.Id == requestConfig.Id &&
							req.AuthenticityKey == requestConfig.Key &&
							req.Availability == OtpRequestAvailability.Available &&
							req.Timeline.Any(@event =>
								@event.State == EventState.Deliver && @event.Status == EventStatus.Success),
						cancellationToken);

			if (otpRequest is null)
			{
				throw new NotFoundException($"Otp request {requestConfig.Id} does not exist");
			}

			if (otpRequest.ExpiresOn < DateTime.UtcNow)
			{
				throw new ExpiredResourceException("Otp request has expired");
			}

			if (otpRequest.App.IsDeleted)
			{
				throw new NotFoundException($"App {otpRequest.AppId} does not exist or has already been deleted");
			}

			return new GetOtpRequestConfigQueryResponse
			{
				BackgroundUrl = otpRequest.App.Branding.BackgroundUrl,
				LogoUrl = otpRequest.App.Branding.LogoUrl,
				Contact = otpRequest.Recipient
			};
		}
	}
}

public record GetOtpRequestConfigQueryResponse
{
	public string? BackgroundUrl { get; init; }
	public string? LogoUrl { get; init; }
	public string Contact { get; init; } = default!;
}