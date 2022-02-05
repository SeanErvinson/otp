using MediatR;
using Otp.Core.Domains.Common.Models;

namespace Otp.Application.Common.Models;

public class DomainEventNotification<T> : INotification where T : DomainEvent
{
	
	public DomainEventNotification(T domainEvent)
	{
		DomainEvent = domainEvent;
	}

	public T DomainEvent { get; }
}