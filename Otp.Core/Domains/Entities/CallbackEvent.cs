﻿using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Common.Models;

namespace Otp.Core.Domains.Entities;

public class CallbackEvent : TimedEntity<Guid>
{
	public Guid AppId { get; private set; }
	public string? Message { get; set; }
	public Guid RequestId { get; private set; }
	public Channel Channel { get; private set; }
	public string Contact { get; private set; }
	public CallbackEventType Type { get; private set; }
	public int StatusCode { get; private set; }
	public string? ResponseMessage { get; private set; }

	public App App { get; private set; }

	private CallbackEvent()
	{
	}

	public CallbackEvent(Guid appId,
		Channel channel,
		Guid requestId,
		string contact,
		CallbackEventType type,
		string? message) : base(Guid.NewGuid())
	{
		AppId = appId;
		Channel = channel;
		RequestId = requestId;
		Contact = contact;
		Type = type;
		Message = message;
	}

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