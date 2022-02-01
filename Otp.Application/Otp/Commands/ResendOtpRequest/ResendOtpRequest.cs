﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;
using Serilog.Context;

namespace Otp.Application.Otp.Commands.ResendOtpRequest;

public record ResendOtpRequest(Guid Id, string Secret) : IRequest
{
	public class Handler : IRequestHandler<ResendOtpRequest>
	{
		private readonly IApplicationDbContext _dbContext;

		public Handler(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Unit> Handle(ResendOtpRequest request, CancellationToken cancellationToken)
		{
			using (LogContext.PushProperty("OtpRequestId", request.Id))
			{
				var otpRequest =
					await _dbContext.OtpRequests.FirstOrDefaultAsync(otpRequest =>
																		otpRequest.Id == request.Id
																		&& otpRequest.Secret == request.Secret
																		&& otpRequest.ExpiresOn > DateTime.UtcNow
																		&& otpRequest.Status == OtpRequestStatus.Success,
																	cancellationToken);

				if (otpRequest is null)
				{
					throw new NotFoundException("Unable to resend existing request was not found");
				}

				using (LogContext.PushProperty("NumberOfRetries", otpRequest.Retries))
				{
					otpRequest.Retry();

					await _dbContext.SaveChangesAsync(cancellationToken);

					return Unit.Value;
				}
			}
		}
	}
}