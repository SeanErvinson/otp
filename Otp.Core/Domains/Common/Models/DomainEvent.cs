using MediatR;

namespace Otp.Core.Domains.Common.Models;

public interface IHasDomainEvent
{
	public IReadOnlyCollection<DomainEvent> DomainEvents { get; }
}

public abstract class DomainEvent : INotification
{
}