namespace Otp.Core.Domains.Common;

public abstract class AuditableEntity : TimedEntity
{
	public string CreatedBy { get; set; } = string.Empty;
	public string? UpdatedBy { get; set; }
}