using System.Runtime.Serialization;

namespace Otp.Application.Common.Exceptions;

[Serializable]
public abstract class BusinessException : Exception
{
	protected BusinessException(SerializationInfo info,
		StreamingContext context,
		string title,
		string detail,
		object? additionalProperties = null) : base(info, context)
	{
		Title = title;
		Detail = detail;
		AdditionalProperties = additionalProperties;
	}

	public BusinessException(string title, string detail, object? additionalProperties = null) :
		base(detail)
	{
		Title = title;
		Detail = detail;
		AdditionalProperties = additionalProperties;
	}

	public BusinessException(string title,
		string detail,
		Exception? innerException,
		object? additionalProperties = null) : base(detail, innerException)
	{
		Title = title;
		Detail = detail;
		AdditionalProperties = additionalProperties;
	}
	
	protected BusinessException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	public string Title { get; }
	public string Detail { get; }
	public object? AdditionalProperties { get; }
}