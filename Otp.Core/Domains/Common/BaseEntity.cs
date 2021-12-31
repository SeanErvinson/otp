namespace Otp.Core.Domains.Common;

public abstract class BaseEntity
{
	protected BaseEntity()
	{
		Id = Guid.NewGuid();
	}

	public Guid Id { get; }
}