namespace Otp.Core.Domains.Common.Models;

public abstract class AuditableEntity : TimedEntity
{
	public string CreatedBy { get; set; } = string.Empty;
	public string? UpdatedBy { get; set; }
}