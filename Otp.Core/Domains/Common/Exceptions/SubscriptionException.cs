using System.Runtime.Serialization;
using Otp.Core.Domains.Common.Enums;

namespace Otp.Core.Domains.Common.Exceptions;

[Serializable]
public class SubscriptionException : Exception
{
	public SubscriptionException(string message) : base(message)
	{
	}

	protected SubscriptionException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	public static SubscriptionException AlreadySubscribed(TieredPlan tieredPlan) =>
		new($"Entity is already subscribed to {tieredPlan}");
}