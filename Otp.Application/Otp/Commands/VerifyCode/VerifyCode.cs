using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;
using Otp.Core.Domains.ValueObjects;
using Serilog;
using Serilog.Context;

namespace Otp.Application.Otp.Commands.VerifyCode;

public record VerifyCode(Guid Id, string Code) : IRequest<VerifyCodeResponse>
{
	public class Handler : IRequestHandler<VerifyCode, VerifyCodeResponse>
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

		public async Task<VerifyCodeResponse> Handle(VerifyCode request,
			CancellationToken cancellationToken)
		{
			using (LogContext.PushProperty("OtpRequestId", request.Id))
			{
				var otpRequest =
					await _dbContext.OtpRequests
						.Include(otpRequest => otpRequest.App)
						.FirstOrDefaultAsync(req =>
								req.Id == request.Id &&
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
				var requestInfo = new ClientInfo(_currentUserService.IpAddress,
					_currentUserService.UserAgent,
					_currentUserService.Referrer);

				if (otpRequest.Code != request.Code)
				{
					otpRequest.AddAttempt(OtpAttempt.Fail(request.Code), requestInfo);
					await _dbContext.SaveChangesAsync(cancellationToken);
					throw new InvalidRequestException("Code provided was incorrect");
				}
				Log.Information("Setting request to success");
				otpRequest.AddAttempt(OtpAttempt.Success(request.Code), requestInfo);
				await _dbContext.SaveChangesAsync(cancellationToken);
				return new VerifyCodeResponse(otpRequest.SuccessUrl);
			}
		}
	}
}

public record VerifyCodeResponse(string SuccessUrl);