namespace Otp.Core.Domains.Common.Models;

public abstract class BaseEntity : IHasDomainEvent
{
	public Guid Id { get; }
	private readonly List<DomainEvent> _domainEvents = new();
	public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.Where(domainEvent => !domainEvent.IsPublished).ToList().AsReadOnly();

	protected BaseEntity()
	{
		Id = Guid.NewGuid();
	}

	protected void AddDomainEvent(DomainEvent domainEvent)
	{
		_domainEvents.Add(domainEvent);
	}
}