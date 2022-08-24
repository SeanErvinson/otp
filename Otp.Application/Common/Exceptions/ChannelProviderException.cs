using System.Runtime.Serialization;

namespace Otp.Application.Common.Exceptions;

public class ChannelProviderException: Exception
{
	public ChannelProviderException(string message, Exception? exception) : base(message, exception)
	{
	}

	protected ChannelProviderException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}