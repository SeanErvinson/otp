using System.Diagnostics;
using Otp.Core.Domains.Common.Models;
using Otp.Core.Domains.Events;
using Otp.Core.Domains.ValueObjects;
using Otp.Core.Utils;

namespace Otp.Core.Domains.Entities;

[DebuggerDisplay("{Name} - {Status}")]
public class App : AuditableEntity
{
	public Guid PrincipalId { get; }
	public string Name { get; }
	public Branding Branding { get; private set; }
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

	public string RegenerateApiKey()
	{
		var generatedKey = CryptoUtil.GenerateKey();
		HashedApiKey = CryptoUtil.HashKey(generatedKey);
		return generatedKey;
	}

	public void TriggerFailedCallback(OtpRequest request, string message)
	{
		TriggerCallback(request, CallbackEventType.Failed, message);
	}
	
	public void TriggerCanceledCallback(OtpRequest request)
	{
		TriggerCallback(request, CallbackEventType.Canceled, "Otp request was canceled");
	}
	
	public void TriggerSuccessCallback(OtpRequest request)
	{
		TriggerCallback(request, CallbackEventType.Success);
	}

	// TODO If switched to a microservice, replace this into a message handler call a mediator
	private void TriggerCallback(OtpRequest request, CallbackEventType type, string? message = null)
	{
		if (string.IsNullOrEmpty(CallbackUrl))
			return;

		AddDomainEvent(new CallbackTriggeredEvent(new CallbackEvent(Id,
																	request.Channel,
																	request.Id,
																	request.Recipient,
																	type, 
																	message), CallbackUrl, EndpointSecret));
	}

	public void MarkAsDeleted()
	{
		Status = AppStatus.Deleted;
	}

	public override string ToString()
	{
		return Name;
	}
}

public enum AppStatus
{
	Active = 1,
	Deleted = 2
}