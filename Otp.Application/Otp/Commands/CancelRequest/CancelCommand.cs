using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;
using Otp.Core.Domains.ValueObjects;
using Serilog;
using Serilog.Context;

namespace Otp.Application.Otp.Commands.CancelRequest;

public record CancelRequestCommand(Guid Id) : IRequest<CancelRequestCommandResponse>
{
	public class Handler : IRequestHandler<CancelRequestCommand, CancelRequestCommandResponse>
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

		public async Task<CancelRequestCommandResponse> Handle(CancelRequestCommand request,
			CancellationToken cancellationToken)
		{
			using (LogContext.PushProperty("OtpRequestId", request.Id))
			{
				var otpRequest =
					await _dbContext.OtpRequests
						.Include(otpRequest => otpRequest.App)
						.FirstOrDefaultAsync(req =>
								req.Id == request.Id &&
								req.Key == _otpContextService.Key &&
								req.Availability == OtpRequestAvailability.Available &&
								req.Timeline.Any(@event =>
									@event.State == EventState.Deliver && @event.Status == EventStatus.Success),
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

				return new CancelRequestCommandResponse(otpRequest.CancelUrl);
			}
		}
	}
}

public record CancelRequestCommandResponse(string CancelUrl);