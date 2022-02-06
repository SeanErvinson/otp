using Otp.Core.Domains.Common.Exceptions;
using Otp.Core.Domains.Common.Models;

namespace Otp.Core.Domains.Entities;

public class Subscription : TimedEntity
{
	public Guid PrincipalId { get; private set; }
	public Plan Plan { get; private set; }

	public Principal Principal { get; private set; } = default!;

	private Subscription()
	{
	}

	private Subscription(Guid principalId, Plan plan)
	{
		PrincipalId = principalId;
		Plan = plan;
	}

	public void ChangePlan(Plan plan)
	{
		if (Plan == plan)
		{
			throw SubscriptionException.AlreadySubscribed(Plan);
		}
	
		Plan = plan;
	}
}

public enum Plan
{
	Usage,
	Free
}