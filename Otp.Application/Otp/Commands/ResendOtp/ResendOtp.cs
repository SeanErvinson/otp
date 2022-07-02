using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.ValueObjects;
using Serilog.Context;

namespace Otp.Application.Otp.Commands.ResendOtp;

public record ResendOtp(Guid Id) : IRequest
{
	public class Handler : IRequestHandler<ResendOtp>
	{
		private readonly IApplicationDbContext _dbContext;
		private readonly IOtpContextService _otpContextService;

		public Handler(IApplicationDbContext dbContext, IOtpContextService otpContextService)
		{
			_dbContext = dbContext;
			_otpContextService = otpContextService;
		}

		public async Task<Unit> Handle(ResendOtp request, CancellationToken cancellationToken)
		{
			using (LogContext.PushProperty("OtpRequestId", request.Id))
			{
				var otpRequest =
					await _dbContext.OtpRequests.FirstOrDefaultAsync(req =>
							req.Id == request.Id &&
							req.AuthenticityKey == _otpContextService.AuthenticityKey &&
							req.ExpiresOn > DateTime.UtcNow &&
							req.Timeline.Any(@event =>
								@event.State == EventState.Deliver && @event.Status == EventStatus.Success),
						cancellationToken);

				if (otpRequest is null)
				{
					throw new NotFoundException("Unable to resend existing request was not found");
				}

				using (LogContext.PushProperty("NumberOfResend", otpRequest.ResendCount))
				{
					otpRequest.Resend();

					await _dbContext.SaveChangesAsync(cancellationToken);

					return Unit.Value;
				}
			}
		}
	}
}