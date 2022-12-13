using MediatR;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Entities;
using Serilog;
using Serilog.Context;

namespace Otp.Application.Otp.Commands.RequestOtp;

public sealed record RequestOtp : IRequest<RequestOtpResponse>
{
	public Channel Channel { get; init; }
	public string Contact { get; init; } = default!;
	public string SuccessUrl { get; init; } = default!;
	public string CancelUrl { get; init; } = default!;
}

public sealed class RequestOtpHandler : IRequestHandler<RequestOtp, RequestOtpResponse>
{
	private readonly IAppContextService _appContextService;
	private readonly IApplicationDbContext _dbContext;

	public RequestOtpHandler(IAppContextService appContextService, IApplicationDbContext dbContext)
	{
		_appContextService = appContextService ?? throw new ArgumentNullException(nameof(appContextService));
		_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
	}

	public async Task<RequestOtpResponse> Handle(RequestOtp request, CancellationToken cancellationToken)
	{
		using (LogContext.PushProperty("Channel", request.Channel))
		{
			Log.Information("Requesting otp");
			var appId = await _dbContext.Apps
				.Where(app => app.HashedApiKey == _appContextService.HashApiKey && app.Status == AppStatus.Active)
				.Select(app => app.Id)
				.FirstOrDefaultAsync(cancellationToken);

			if (appId == Guid.Empty)
			{
				Log.Error("Api key is not associated with any active application");
				throw new NotFoundException("App does not exists");
			}
			var otpRequest = new OtpRequest(appId, request.Contact, request.Channel, request.SuccessUrl, request.CancelUrl);
			var result = await _dbContext.OtpRequests.AddAsync(otpRequest, cancellationToken);
			await _dbContext.SaveChangesAsync(cancellationToken);
			return new RequestOtpResponse(result.Entity.Id,
				new Uri(new Uri("http://localhost:3000/"), otpRequest.RequestPath));
		}
	}
}

public sealed record RequestOtpResponse(Guid Id, Uri RedirectUri);