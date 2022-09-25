namespace Otp.Core.Domains.Common.Models;

public interface IAuditableEntity
{
	public string CreatedBy { get; set; }
	public string? UpdatedBy { get; set; }
}

public abstract class AuditableEntity<TKey> : TimedEntity<TKey>, IAuditableEntity
{
	public string CreatedBy { get; set; } = string.Empty;
	public string? UpdatedBy { get; set; }

	protected AuditableEntity()
	{
	}

	protected AuditableEntity(TKey id) : base(id)
	{
	}
}