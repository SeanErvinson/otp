namespace Otp.Core.Domains.Common.Models;

public abstract class BaseEntity<TKey> : IHasDomainEvent
{
	public TKey Id { get; } = default!;
	private readonly List<DomainEvent> _domainEvents = new();

	public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.ToList().AsReadOnly();
	
	protected BaseEntity() { }
	protected BaseEntity(TKey id)
	{
		Id = id;
	}

	protected void RemoveDomainEvent(DomainEvent domainEvent)
	{
		_domainEvents.Remove(domainEvent);
	}

	protected void AddDomainEvent(DomainEvent domainEvent)
	{
		_domainEvents.Add(domainEvent);
	}

	public void ClearDomainEvents()
	{
		_domainEvents.Clear();
	}
}