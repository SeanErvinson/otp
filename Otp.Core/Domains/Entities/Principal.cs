using Otp.Core.Domains.Common.Models;

namespace Otp.Core.Domains.Entities;

public class Principal : AuditableEntity
{
	private Principal()
	{
	}

	public Principal(string name, string userId)
	{
		Name = name;
		UserId = userId;
		Status = PrincipalStatus.Active;
	}

	public string Name { get; private set; }
	public string UserId { get; private set; }
	public PrincipalStatus Status { get; }
}

public enum PrincipalStatus
{
	Inactive = 0,
	Pending = 1,
	Active = 2,
	Deleted = 3
}