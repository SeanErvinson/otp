﻿using MediatR;
using Otp.Application.Common.Interfaces;
using Otp.Application.Common.Models;
using Otp.Application.Services;
using Otp.Core.Domains.Events;
using Serilog;
using Serilog.Context;

namespace Otp.Application.Otp.EventHandlers;

public class OtpRequestedEventHandler : INotificationHandler<DomainEventNotification<OtpRequestedEvent>>
{
	private readonly IApplicationDbContext _applicationDbContext;
	private readonly ISenderService _senderService;

	public OtpRequestedEventHandler(ISenderService senderService, IApplicationDbContext applicationDbContext)
	{
		_senderService = senderService;
		_applicationDbContext = applicationDbContext;
	}

	public async Task Handle(DomainEventNotification<OtpRequestedEvent> notification, CancellationToken cancellationToken)
	{
		var otpRequest = notification.DomainEvent.OtpRequest;

		using (LogContext.PushProperty("OtpRequestId", otpRequest.Id))
		using (LogContext.PushProperty("OtpRequestMode", otpRequest.Mode))
		{
			var sender = _senderService.GetSenderFactory(otpRequest.Mode);

			using (LogContext.PushProperty("SenderType", sender.GetType().Name))
			{
				try
				{
					Log.Information("Sending otp request");
					await sender.Send(otpRequest, cancellationToken);
					otpRequest.SuccessfullySent();
				}
				catch (Exception e)
				{
					Log.Error(e, "Failed to send otp request");
					otpRequest.FailedSent(e.Message);
					throw new InvalidOperationException(e.Message);
				}
				finally
				{
					await _applicationDbContext.SaveChangesAsync(cancellationToken);
				}
			}
		}
	}
}