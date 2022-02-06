using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Common.Models;

namespace Otp.Core.Domains.Entities;

public class Discount : AuditableEntity
{
	public Guid SubscriptionId { get; private set; }
	public int Threshold { get; private set; }
	public Channel Channel { get; }
	public decimal Rate { get; private set; }

	public Subscription Subscription { get; private set; } = default!;

	private Discount()
	{
	}

	public Discount(Guid subscriptionId, int threshold, Channel channel, decimal rate)
	{
		SubscriptionId = subscriptionId;
		Threshold = threshold;
		Channel = channel;
		Rate = rate;
	}

	public void ChangeRate(decimal rate)
	{
		Rate = rate;
	}
	
	public void ChangeThreshold(int threshold)
	{
		Threshold = threshold;
	}
}