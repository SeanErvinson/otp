using System.Runtime.Serialization;

namespace Otp.Application.Common.Exceptions;

[Serializable]
public sealed class InvalidRequestException : BusinessException
{
	public InvalidRequestException(string detail) : base(ExceptionConstants.InvalidInput, detail)
	{
	}

	public InvalidRequestException(string title, string detail, object? additionalProperties = null) : base(title,
		detail,
		additionalProperties)
	{
	}

	public InvalidRequestException(string title,
		string detail,
		Exception? innerException,
		object? additionalProperties = null) : base(title, detail, innerException, additionalProperties)
	{
	}

	protected InvalidRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}