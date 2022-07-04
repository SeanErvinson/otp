using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Common.Exceptions;
using Otp.Core.Domains.Common.Models;

namespace Otp.Core.Domains.Entities;

public class Subscription : TimedEntity
{
	public Guid PrincipalId { get; private set; }
	public TieredPlan TieredPlan { get; private set; }

	public Principal Principal { get; private set; } = default!;

	private Subscription()
	{
	}

	private Subscription(Guid principalId, TieredPlan tieredPlan)
	{
		PrincipalId = principalId;
		TieredPlan = tieredPlan;
	}

	public void ChangePlan(TieredPlan tieredPlan)
	{
		if (TieredPlan == tieredPlan)
		{
			throw SubscriptionException.AlreadySubscribed(TieredPlan);
		}
		TieredPlan = tieredPlan;
	}
}