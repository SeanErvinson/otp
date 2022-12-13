using System.Runtime.Serialization;

namespace Otp.Application.Common.Exceptions;

[Serializable]
public sealed class ExpiredResourceException : Exception
{
	public ExpiredResourceException()
	{
	}
	
	public ExpiredResourceException(string message) : base(message)
	{
	}

	public ExpiredResourceException(object entity) : base("Resource has expired or is no longer available.")
	{
	}

	protected ExpiredResourceException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}