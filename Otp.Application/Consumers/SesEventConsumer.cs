using System.ComponentModel.DataAnnotations;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.ValueObjects;
using Serilog;
using LogContext = Serilog.Context.LogContext;

namespace Otp.Application.Consumers;

public class SesEventConsumer : IConsumer<SesEvent>
{
	private readonly ILogger _logger;
	private readonly IApplicationDbContext _applicationDbContext;

	public SesEventConsumer(ILogger logger, IApplicationDbContext applicationDbContext)
	{
		_logger = logger;
		_applicationDbContext = applicationDbContext;
	}

	public async Task Consume(ConsumeContext<SesEvent> context)
	{
		var message = context.Message;
		var otpRequest =
			await _applicationDbContext.OtpRequests.SingleAsync(request =>
				request.CorrelationId == message.Mail.MessageId);

		if (!Enum.TryParse<EventType>(message.EventType, out var eventType))
		{
			_logger.Error("An unexpected event type {EventType} occured", message.EventType);
			return;
		}

		OtpEvent? otpEvent;

		switch (eventType)
		{
			case EventType.Send or EventType.RenderingFailure:
				if (eventType is EventType.Send)
				{
					otpEvent = OtpEvent.Success(EventState.Send, message.Mail.Timestamp);
					break;
				}
				const string response = "Message could not be sent";
				LogContext.PushProperty("MessageTemplate", message.Failure.TemplateName);
				_logger.Warning("{Message} {ProviderMessage}", response, message.Failure.ErrorMessage);
				otpEvent = OtpEvent.Fail(EventState.Send, message.Mail.Timestamp, response);
				break;
			case EventType.Delivery or EventType.Bounce or EventType.DeliveryDelay:

				otpEvent = eventType switch
				{
					EventType.Delivery => OtpEvent.Success(EventState.Deliver, message.Delivery.Timestamp),
					EventType.Bounce => OtpEvent.Fail(EventState.Deliver,
						message.Bounce.Timestamp,
						$"Message couldn't be delivered. Provider responded with {message.Bounce.BounceType}"),
					EventType.DeliveryDelay => OtpEvent.Fail(EventState.Deliver,
						message.DeliveryDelay.Timestamp,
						$"Message couldn't be delivered. Provider responded with {message.DeliveryDelay.DelayType}"),
				};

				break;
			default:
				throw new NotSupportedException($"Event type {eventType} is not supported");
		}

		otpRequest.AddEvent(otpEvent);

		_applicationDbContext.OtpRequests.Update(otpRequest);
		await _applicationDbContext.SaveChangesAsync(context.CancellationToken);
	}
}

public sealed record SesEvent
{
	public string EventType { get; init; } = default!;
	public Mail Mail { get; init; } = default!;
	public Delivery Delivery { get; init; } = default!;
	public Failure Failure { get; init; } = default!;
	public DeliveryDelay DeliveryDelay { get; init; } = default!;
	public Bounce Bounce { get; init; } = default!;
}

public sealed record Delivery(DateTime Timestamp);
public sealed record Failure(string ErrorMessage, string TemplateName);
public sealed record DeliveryDelay(string DelayType, DateTime Timestamp);
public sealed record Bounce(string BounceType, DateTime Timestamp);

public sealed record Mail
{
	public DateTime Timestamp { get; init; }
	public string MessageId { get; init; } = default!;
}

public enum EventType
{
	Send,

	[Display(Name = "Rendering Failure")]
	RenderingFailure,
	Bounce,
	Delivery,
	DeliveryDelay
}

public class SesEventConsumerDefinition : ConsumerDefinition<SesEventConsumer>
{
	protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
		IConsumerConfigurator<SesEventConsumer> consumerConfigurator)
	{
		endpointConfigurator.ClearSerialization();
		endpointConfigurator.UseRawJsonSerializer();
		endpointConfigurator.ConfigureConsumeTopology = false;
	}
}