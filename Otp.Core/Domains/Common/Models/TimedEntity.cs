namespace Otp.Core.Domains.Common.Models;

public interface ITimedEntity
{
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }	
}

public abstract class TimedEntity<TKey> : BaseEntity<TKey>, ITimedEntity
{
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }	
	
	protected TimedEntity()
	{

	}
	
	protected TimedEntity(TKey id) : base(id)
	{

	}
}