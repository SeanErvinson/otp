using System.Runtime.Serialization;

namespace Otp.Application.Common.Exceptions;

public class InvalidRequestException : Exception
{
	public InvalidRequestException(string message) : base(message)
	{
	}

	public InvalidRequestException(object entity) : base($"Entity {entity} has bad or invalid data")
	{
	}

	protected InvalidRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}