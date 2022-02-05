using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Common.Models;

namespace Otp.Core.Domains.Entities;

public class CallbackEvent : TimedEntity
{
	public Guid RequestId { get; set; }
	public Channel Channel { get; set; }
	public string Contact { get; set; }
	public CallbackEventType Type { get; set; }
	public int StatusCode { get; private set; }
	public string? ResponseMessage { get; private set; }

	public void SetResponse(int statusCode, string? responseMessage)
	{
		StatusCode = statusCode;
		ResponseMessage = responseMessage;
	}
}

public enum CallbackEventType
{
	Success,
	Failed,
	Canceled
}