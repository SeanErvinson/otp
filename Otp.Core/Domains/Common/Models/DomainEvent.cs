namespace Otp.Core.Domains.Common.Models;

public interface IHasDomainEvent
{
	public IReadOnlyCollection<DomainEvent> DomainEvents { get; }
}

public abstract class DomainEvent
{
	public bool IsPublished { get; set; }
	public DateTimeOffset OccurredAt { get; }

	protected DomainEvent()
	{
		OccurredAt = DateTime.UtcNow;
	}
}