using Otp.Core.Domains.Common;
using Otp.Core.Domains.Entities;

namespace Otp.Core.Domains.Events;

public class CallbackTriggeredEvent : DomainEvent
{
	public CallbackTriggeredEvent(CallbackEvent callbackEvent, string callbackUrl, string? endpointSecret)
	{
		CallbackEvent = callbackEvent;
		CallbackUrl = callbackUrl;
		EndpointSecret = endpointSecret;
	}

	public CallbackEvent CallbackEvent { get; }
	public string CallbackUrl { get; }
	public string? EndpointSecret { get; }
}