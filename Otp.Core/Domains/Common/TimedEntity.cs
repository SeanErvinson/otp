namespace Otp.Core.Domains.Common;

public abstract class TimedEntity : BaseEntity
{
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
}