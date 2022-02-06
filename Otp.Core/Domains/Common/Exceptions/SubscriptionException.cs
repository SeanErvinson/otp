using System.Runtime.Serialization;
using Otp.Core.Domains.Entities;

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

	public static SubscriptionException AlreadySubscribed(Plan plan)
	{
		return new SubscriptionException($"Entity is already subscribed to {plan}");
	}
}