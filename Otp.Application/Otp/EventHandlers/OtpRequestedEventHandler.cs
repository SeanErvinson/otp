using MediatR;
using Otp.Application.Common.Interfaces;
using Otp.Core.Domains.Events;
using Otp.Core.Domains.ValueObjects;
using Serilog;
using Serilog.Context;

namespace Otp.Application.Otp.EventHandlers;

public class OtpRequestedEventHandler : INotificationHandler<OtpRequestedEvent>
{
	private readonly ISenderService _senderService;

	public OtpRequestedEventHandler(ISenderService senderService)
	{
		_senderService = senderService;
	}

	public async Task Handle(OtpRequestedEvent notification, CancellationToken cancellationToken)
	{
		var otpRequest = notification.OtpRequest;

		using (LogContext.PushProperty("OtpRequestId", otpRequest.Id))
		using (LogContext.PushProperty("OtpRequestChannel", otpRequest.Channel))
		{
			var sender = _senderService.GetSenderFactory(otpRequest.Channel);

			using (LogContext.PushProperty("SenderType", sender.GetType().Name))
			{
				try
				{
					Log.Information("Sending otp request");
					await sender.Send(otpRequest, cancellationToken);
					otpRequest.AddEvent(OtpEvent.Success(EventState.Send, "Otp request sent"));
				}
				catch (Exception e)
				{
					Log.Error(e, "Failed to send otp request");
					otpRequest.AddEvent(OtpEvent.Fail(EventState.Send, e.Message));
					throw new InvalidOperationException(e.Message);
				}
			}
		}
	}
}