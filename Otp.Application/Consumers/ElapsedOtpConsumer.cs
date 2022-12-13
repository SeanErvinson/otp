using MassTransit;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Entities;
using Serilog;

namespace Otp.Application.Consumers;

public sealed record ElapsedOtp(Guid Id);

public class ElapsedOtpConsumer : IConsumer<ElapsedOtp>
{
	private readonly IApplicationDbContext _applicationDbContext;
	private readonly ILogger _logger;

	public ElapsedOtpConsumer(IApplicationDbContext applicationDbContext, ILogger logger)
	{
		_applicationDbContext = applicationDbContext;
		_logger = logger;
	}

	public async Task Consume(ConsumeContext<ElapsedOtp> context)
	{
		var message = context.Message;
		var otpRequest = _applicationDbContext.OtpRequests.Single(request => request.Id == message.Id);

		if (otpRequest.Availability is OtpRequestAvailability.Unavailable)
		{
			_logger.Information("OTP is already claimed");
			return;
		}

		otpRequest.ClaimRequest();
		_applicationDbContext.OtpRequests.Update(otpRequest);
		await _applicationDbContext.SaveChangesAsync(context.CancellationToken);
	}
}

public class ElapsedOtpConsumerDefinition : ConsumerDefinition<ElapsedOtpConsumer>
{
	protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ElapsedOtpConsumer> consumerConfigurator)
	{
		endpointConfigurator.ConfigureConsumeTopology = false;
	}
}