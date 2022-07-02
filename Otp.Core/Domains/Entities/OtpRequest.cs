using System.Diagnostics;
using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Common.Exceptions;
using Otp.Core.Domains.Common.Models;
using Otp.Core.Domains.Events;
using Otp.Core.Domains.ValueObjects;
using Otp.Core.Utils;

namespace Otp.Core.Domains.Entities;

[DebuggerDisplay("{Id} - {Code} - {Availability}")]
public class OtpRequest : TimedEntity
{
	public Guid AppId { get; private set; }
	public string Code { get; private set; } = default!;
	public string SuccessUrl { get; } = default!;
	public string CancelUrl { get; } = default!;
	public string Recipient { get; } = default!;
	public Channel Channel { get; }
	public DateTime? VerifiedAt { get; private set; }
	public string AuthenticityKey { get; } = default!;
	public int ResendCount { get; private set; }
	public int MaxAttempts { get; private set; } = 3;
	public OtpRequestAvailability Availability { get; private set; }
	public ClientInfo? ClientInfo { get; private set; }
	
	private readonly List<OtpEvent> _timeline = new();
	public IReadOnlyCollection<OtpEvent> Timeline => _timeline.AsReadOnly();
	
	private readonly List<OtpAttempt> _otpAttempts = new();
	public IReadOnlyCollection<OtpAttempt> OtpAttempts => _otpAttempts.AsReadOnly();
	public DateTime ExpiresOn { get; } = DateTime.UtcNow.AddMinutes(5);
	public App App { get; private set; } = default!;

	public string RequestPath => new($"/{Enum.GetName(Channel)?.ToLower()}/{Id.ToString()}#{AuthenticityKey}/");

	private OtpRequest()
	{
	}

	public OtpRequest(Guid appId, string recipient, Channel channel, string successUrl, string cancelUrl)
	{
		AppId = appId;
		Recipient = recipient;
		Channel = channel;
		SuccessUrl = successUrl;
		CancelUrl = cancelUrl;
		Code = OtpUtil.GenerateCode();
		AuthenticityKey = CryptoUtil.HashKey(CryptoUtil.GenerateKey());
		Availability = OtpRequestAvailability.Available;
		AddEvent(OtpEvent.Success(EventState.Request));
		
		AddDomainEvent(new OtpRequestedEvent(this));
	}

	public void Resend()
	{
		ResendCount += 1;
		Code = OtpUtil.GenerateCode();

		AddDomainEvent(new OtpRequestedEvent(this));
	}

	public void AddEvent(OtpEvent otpEvent)
	{
		// TODO Consider adding a state machine to make sure that events are in proper order Request → Sent → Deliver
		_timeline.Add(otpEvent);
	}

	// TODO If switched to a microservice, all the trigger, should be replace with an event that sends a message
	public void AddAttempt(OtpAttempt attempt, ClientInfo clientInfo)
	{
		ClientInfo ??= clientInfo;

		if (ExpiresOn >= DateTime.UtcNow)
		{ 
			ClaimRequest();
			throw new OtpRequestException("Request has expired");
		}
		
		if (_otpAttempts.Count >= MaxAttempts)
		{
			const string message = "OTP has reached max attempt tries. Please request a new OTP";
			ClaimRequest();
			App.TriggerFailedCallback(this, message);
			throw new OtpRequestException(message);
		}

		_otpAttempts.Add(attempt);

		switch (attempt.AttemptStatus)
		{
			case OtpAttemptStatus.Success:
				ClaimRequest();
				App.TriggerSuccessCallback(this);
				break;
			case OtpAttemptStatus.Fail:
				App.TriggerFailedCallback(this, "Code provided by user is incorrect");
				break;
			case OtpAttemptStatus.Canceled:
				ClaimRequest();
				App.TriggerCanceledCallback(this);
				break;
		}
	}

	private void ClaimRequest()
	{
		Availability = OtpRequestAvailability.Unavailable;
		VerifiedAt = DateTime.UtcNow;
	}
}

public enum OtpRequestAvailability
{
	Available,
	Unavailable,
}
