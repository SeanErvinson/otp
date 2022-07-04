using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;
using Otp.Core.Domains.ValueObjects;
using Serilog;
using Serilog.Context;

namespace Otp.Application.Otp.Commands.CancelOtp;

public record CancelOtp(Guid Id) : IRequest<CancelOtpResponse>
{
	public class Handler : IRequestHandler<CancelOtp, CancelOtpResponse>
	{
		private readonly IApplicationDbContext _dbContext;
		private readonly IOtpContextService _otpContextService;
		private readonly ICurrentUserService _currentUserService;

		public Handler(IApplicationDbContext dbContext,
			IOtpContextService otpContextService,
			ICurrentUserService currentUserService)
		{
			_dbContext = dbContext;
			_otpContextService = otpContextService;
			_currentUserService = currentUserService;
		}

		public async Task<CancelOtpResponse> Handle(CancelOtp otp,
			CancellationToken cancellationToken)
		{
			using (LogContext.PushProperty("OtpRequestId", otp.Id))
			{
				var otpRequest =
					await _dbContext.OtpRequests
						.Include(otpRequest => otpRequest.App)
						.FirstOrDefaultAsync(req =>
								req.Id == otp.Id &&
								req.AuthenticityKey == _otpContextService.AuthenticityKey &&
								req.Availability == OtpRequestAvailability.Available &&
								req.Timeline.Any(@event =>
									@event.State == EventState.Deliver &&
									@event.Status == EventStatus.Success),
							cancellationToken);

				if (otpRequest is null)
				{
					throw new NotFoundException("Otp request was not found");
				}

				if (otpRequest.ExpiresOn < DateTime.UtcNow)
				{
					throw new ExpiredResourceException("Otp request has expired");
				}

				if (otpRequest.App.IsDeleted)
				{
					throw new NotFoundException($"App {otpRequest.AppId} does not exist or has already been deleted");
				}
				Log.Information("Cancelling otp request");
				otpRequest.AddAttempt(OtpAttempt.Cancel(),
					new ClientInfo(_currentUserService.IpAddress,
						_currentUserService.UserAgent,
						_currentUserService.Referrer));
				await _dbContext.SaveChangesAsync(cancellationToken);
				return new CancelOtpResponse(otpRequest.CancelUrl);
			}
		}
	}
}

public record CancelOtpResponse(string CancelUrl);