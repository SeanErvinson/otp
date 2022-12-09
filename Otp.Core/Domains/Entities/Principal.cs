using Otp.Core.Domains.Common.Models;

namespace Otp.Core.Domains.Entities;

public class Principal : AuditableEntity<Guid>
{
	private Principal()
	{
	}

	public Principal(string name, string userId, string customerId) : base(Guid.NewGuid())
	{
		Name = name;
		UserId = userId;
		CustomerId = customerId;
		Status = PrincipalStatus.Active;
	}

	public string Name { get; private set; }
	public string UserId { get; private set; }
	
	public string CustomerId { get; private set; }
	public PrincipalStatus Status { get; }
}

public enum PrincipalStatus
{
	Inactive = 0,
	Pending = 1,
	Active = 2,
	Deleted = 3
}