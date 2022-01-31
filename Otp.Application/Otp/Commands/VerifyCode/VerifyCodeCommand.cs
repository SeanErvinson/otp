using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;
using Serilog.Context;

namespace Otp.Application.Otp.Commands.VerifyCode;

public record VerifyCodeCommand(Guid Id, string Secret, string Code) : IRequest
{
	public class Handler : IRequestHandler<VerifyCodeCommand>
	{
		private readonly IApplicationDbContext _dbContext;

		public Handler(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Unit> Handle(VerifyCodeCommand request, CancellationToken cancellationToken)
		{
			using (LogContext.PushProperty("OtpRequestId", request.Id))
			{
				var otpRequest =
					await _dbContext.OtpRequests.FirstOrDefaultAsync(req =>
																		req.Id == request.Id
																		&& req.Secret == request.Secret
																		&& req.State == OtpRequestState.Available
																		&& req.Status == OtpRequestStatus.Success,
																	cancellationToken);

				if (otpRequest is null)
				{
					throw new NotFoundException("Otp request was not found");
				}

				if (otpRequest.ExpiresOn < DateTime.UtcNow)
				{
					throw new InvalidOperationException("Otp request has expired");
				}

				var app = await _dbContext.Apps.FirstOrDefaultAsync(app => app.Id == otpRequest.AppId && app.Status == AppStatus.Active, cancellationToken);

				if (app is null)
				{
					throw new NotFoundException($"App {otpRequest.AppId} does not exist or has already been deleted");
				}
				
				if (otpRequest.Code != request.Code)
				{
					app.TriggerFailedCallback(otpRequest);
					await _dbContext.SaveChangesAsync(cancellationToken);
					throw new UnauthorizedAccessException("Code provided was incorrect");
				}

				app.TriggerSuccessCallback(otpRequest);
				otpRequest.SuccessfullyClaimed();
				await _dbContext.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}