using MassTransit;
using MediatR;
using Otp.Application.Common.Exceptions;
using Otp.Application.Common.Interfaces;
using Otp.Application.Consumers;
using Otp.Core.Domains.Events;
using Otp.Core.Domains.ValueObjects;
using Serilog;
using LogContext = Serilog.Context.LogContext;

namespace Otp.Application.Otp.EventHandlers;

public class OtpRequestedEventHandler : INotificationHandler<OtpRequestedEvent>
{
	private readonly IChannelProviderService _channelProviderService;
	private readonly IMessageScheduler _messageScheduler;

	public OtpRequestedEventHandler(IChannelProviderService channelProviderService, IMessageScheduler messageScheduler)
	{
		_channelProviderService = channelProviderService;
		_messageScheduler = messageScheduler;
	}

	public async Task Handle(OtpRequestedEvent notification, CancellationToken cancellationToken)
	{
		var otpRequest = notification.OtpRequest;

		using (LogContext.PushProperty("OtpRequestId", otpRequest.Id))
		using (LogContext.PushProperty("OtpRequestChannel", otpRequest.Channel))
		{
			var sender = _channelProviderService.GetChannelFactory(otpRequest.Channel);

			using (LogContext.PushProperty("SenderType", sender.GetType().Name))
			{
				try
				{
					Log.Information("Sending otp request");
					var messageId = await sender.Send(otpRequest, cancellationToken);
					await _messageScheduler.ScheduleSend(new Uri("queue:elapsed-otp"), otpRequest.ExpiresOn,
						new ElapsedOtp(otpRequest.Id),
						cancellationToken);
					otpRequest.AssignCorrelationId(messageId);
				}
				catch (ChannelProviderException e)
				{
					Log.Error(e, "Failed to send otp request");
					otpRequest.AddEvent(OtpEvent.Fail(EventState.Send, response: e.Message));
					throw new InvalidOperationException(e.Message);
				}
			}
		}
	}
}