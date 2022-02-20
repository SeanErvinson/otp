using System.Runtime.Serialization;

namespace Otp.Core.Domains.Common.Exceptions;

[Serializable]
public class BrandingException : Exception
{
	public BrandingException(string message) : base(message)
	{
	}

	protected BrandingException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}