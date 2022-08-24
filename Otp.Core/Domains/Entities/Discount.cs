using Otp.Core.Domains.Common.Models;

namespace Otp.Core.Domains.Entities;

public class Discount : AuditableEntity
{
	public Guid ChannelPriceId { get; private set; }
	public Guid PrincipalId { get; private set; }
	public decimal Rate { get; private set; }

	public ChannelPrice ChannelPrice { get; private set; } = default!;
	public Principal Principal { get; private set; } = default!;

	private Discount()
	{
	}

	public Discount(Guid channelPriceId, Guid principalId, decimal rate)
	{
		ChannelPriceId = channelPriceId;
		PrincipalId = principalId;
		Rate = rate;
	}

	public void ChangeRate(decimal rate)
	{
		Rate = rate;
	}
}