﻿using Otp.Core.Domains.Common;
using Otp.Core.Domains.Events;
using Otp.Core.Utils;

namespace Otp.Core.Domains.Entities;

public class App : AuditableEntity
{
	public Guid PrincipalId { get; }
	public string Name { get; }
	public string? BackgroundUri { get; private set; }
	public string? LogoUri { get; private set; }
	public string? Description { get; private set; }
	public string? CallbackUrl { get; private set; }
	public string HashedApiKey { get; private set; }
	public string? EndpointSecret { get; private set; }
	public AppStatus Status { get; private set; }
	public IReadOnlyCollection<string>? Tags => _tags?.AsReadOnly();
	private readonly List<string>? _tags = new();

	public Principal Principal { get; set; }
	public bool IsDeleted => Status == AppStatus.Deleted;

	private App()
	{
	}

	public App(Guid principalId, string name, string apiKey, ICollection<string>? tags, string? description = null)
	{
		PrincipalId = principalId;
		Name = name;
		HashedApiKey = CryptoUtil.HashKey(apiKey);
		Status = AppStatus.Active;
		Description = description;
		if (tags?.Any() == true)
			_tags?.AddRange(tags);
	}

	public void UpdateDescription(string description)
	{
		Description = description;
	}

	public void UpdateCallbackUrl(string callbackUrl, string? endpointSecret)
	{
		CallbackUrl = callbackUrl;
		EndpointSecret = endpointSecret; //TODO: Probably needs to be salted since it is coming from the user
	}

	public void UpdateHashedApiKey(string value)
	{
		HashedApiKey = CryptoUtil.HashKey(value);
	}
	
	public void TriggerFailedCallback(OtpRequest request)
	{
		TriggerCallback(request, CallbackEventType.Failed);
	}
	
	public void TriggerCanceledCallback(OtpRequest request)
	{
		TriggerCallback(request, CallbackEventType.Canceled);
	}
	public void TriggerSuccessCallback(OtpRequest request)
	{
		TriggerCallback(request, CallbackEventType.Success);
	}

	private void TriggerCallback(OtpRequest request, CallbackEventType type)
	{
		if (string.IsNullOrEmpty(CallbackUrl))
			return;
		
		AddDomainEvent(new CallbackTriggeredEvent(new CallbackEvent
		{
			Mode = request.Mode,
			Contact = request.Contact,
			RequestId =request.Id,
			Type = type
		}, CallbackUrl, EndpointSecret));
	}

	public void MarkAsDeleted()
	{
		Status = AppStatus.Deleted;
	}
}

public enum AppStatus
{
	Active = 1,
	Deleted = 2
}