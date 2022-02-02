using System.Runtime.Serialization;

namespace Otp.Application.Common.Exceptions;

public class ExpiredResourceException : Exception
{
	public ExpiredResourceException(string message) : base(message)
	{
	}

	public ExpiredResourceException(object entity) : base($"Entity {entity} has bad or invalid data")
	{
	}

	protected ExpiredResourceException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}