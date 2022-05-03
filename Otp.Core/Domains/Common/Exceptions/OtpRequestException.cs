using System.Runtime.Serialization;

namespace Otp.Core.Domains.Common.Exceptions;

[Serializable]
public class OtpRequestException : Exception
{
	public OtpRequestException(string message) : base(message)
	{
	}

	protected OtpRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}