using MediatR;
using Otp.Application.Common.Interfaces;
using Otp.Application.Common.Models;
using Otp.Core.Domains.Common;
using Serilog;
using Serilog.Context;

namespace Otp.Infrastructure.Services;

public class DomainEventService : IDomainEventService
{
	private readonly IPublisher _mediator;

	public DomainEventService(IPublisher mediator)
	{
		_mediator = mediator;
	}

	public async Task Publish(DomainEvent domainEvent, CancellationToken cancellationToken = default)
	{
		using (LogContext.PushProperty("DomainEvent", domainEvent.GetType().Name))
		{
			Log.Information("Publishing domain event");
			domainEvent.IsPublished = true;
			await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent), cancellationToken);
			Log.Information("Published domain event");
		}
	}
	
	private static INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
	{
		return (INotification)Activator.CreateInstance(
			typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent)!;
	}
}