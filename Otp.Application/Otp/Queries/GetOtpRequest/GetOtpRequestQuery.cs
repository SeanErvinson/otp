using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;

namespace Otp.Application.Otp.Queries.GetOtpRequest;

public record GetOtpRequestQueryRequest(string Key);

public record GetOtpRequestQuery(Guid Id, string Key) : IRequest<GetOtpRequestQueryResponse>
{
	public class Handler : IRequestHandler<GetOtpRequestQuery, GetOtpRequestQueryResponse>
	{
		private readonly IApplicationDbContext _dbContext;

		public Handler(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<GetOtpRequestQueryResponse> Handle(GetOtpRequestQuery request, CancellationToken cancellationToken)
		{
			var otpRequest =
				await _dbContext.OtpRequests.AsNoTracking()
								.Include(otpRequest => otpRequest.App)
								.FirstOrDefaultAsync(otpRequest =>
														otpRequest.Id == request.Id
														&& otpRequest.Key == request.Key
														&& otpRequest.State == OtpRequestState.Available
														&& otpRequest.Status == OtpRequestStatus.Success,
													cancellationToken);

			if (otpRequest is null)
			{
				throw new NotFoundException($"Otp request {request.Id} does not exist");
			}
			
			if (otpRequest.ExpiresOn < DateTime.UtcNow)
			{
				throw new ExpiredResourceException("Otp request has expired");
			}

			if (otpRequest.App.IsDeleted)
			{
				throw new NotFoundException($"App {otpRequest.AppId} does not exist or has already been deleted");
			}

			return new GetOtpRequestQueryResponse
			{
				BackgroundUri = otpRequest.App.BackgroundUri,
				LogoUri = otpRequest.App.LogoUri,
				Contact = otpRequest.Contact
			};
		}
	}
}

public record GetOtpRequestQueryResponse
{
	public string? BackgroundUri { get; init; }
	public string? LogoUri { get; init; }
	public string Contact { get; init; } = default!;
}