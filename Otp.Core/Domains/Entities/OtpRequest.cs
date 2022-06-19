using System.Diagnostics;
using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Common.Exceptions;
using Otp.Core.Domains.Common.Models;
using Otp.Core.Domains.Events;
using Otp.Core.Domains.ValueObjects;
using Otp.Core.Utils;

namespace Otp.Core.Domains.Entities;

[DebuggerDisplay("{Id} - {Code} - {State} - {Status}")]
public class OtpRequest : TimedEntity
{
	public Guid AppId { get; private set; }
	public string Code { get; private set; } = default!;
	public string SuccessUrl { get; } = default!;
	public string CancelUrl { get; } = default!;
	public string Contact { get; } = default!;
	public Channel Channel { get; }
	public DateTime? VerifiedAt { get; private set; }
	public string Key { get; } = default!;
	public int ResendCount { get; private set; }
	public int MaxAttempts { get; private set; } = 3;
	public OtpRequestState State { get; private set; }
	public OtpRequestStatus Status { get; private set; }
	public string? ErrorMessage { get; private set; }
	public RequestInfo? RequestInfo { get; private set; }
	private readonly List<OtpAttempt> _otpAttempts = new();
	public IReadOnlyCollection<OtpAttempt> OtpAttempts => _otpAttempts.AsReadOnly();
	public DateTime ExpiresOn { get; } = DateTime.UtcNow.AddMinutes(5);
	public App App { get; private set; } = default!;

	public string RequestPath => new($"/{Enum.GetName(Channel)?.ToLower()}/{Id.ToString()}#{Key}/");

	private OtpRequest()
	{
	}

	public OtpRequest(Guid appId, string contact, Channel channel, string successUrl, string cancelUrl)
	{
		AppId = appId;
		Contact = contact;
		Channel = channel;
		SuccessUrl = successUrl;
		CancelUrl = cancelUrl;
		Code = OtpUtil.GenerateCode();
		Key = CryptoUtil.HashKey(CryptoUtil.GenerateKey());
		State = OtpRequestState.Available;

		AddDomainEvent(new OtpRequestedEvent(this));
	}

	public void Resend()
	{
		ResendCount += 1;
		Code = OtpUtil.GenerateCode();

		AddDomainEvent(new OtpRequestedEvent(this));
	}

	public void SentSuccessfully()
	{
		Status = OtpRequestStatus.Success;
	}

	public void SentFailed(string message)
	{
		Status = OtpRequestStatus.Failed;
		ErrorMessage = message;
	}

	// TODO If switched to a microservice, all the trigger, should be replace with an event that sends a message
	public void AddAttempt(OtpAttempt attempt, RequestInfo requestInfo)
	{
		RequestInfo ??= requestInfo;
		
		if (_otpAttempts.Count >= MaxAttempts)
		{
			const string message = "OTP has reached max attempt tries. Please request a new OTP";
			ClaimFailed(message);
			App.TriggerFailedCallback(this, message);
			throw new OtpRequestException(message);
		}

		_otpAttempts.Add(attempt);

		switch (attempt.AttemptStatus)
		{
			case OtpAttemptStatus.Success:
				ClaimSuccessfully();
				App.TriggerSuccessCallback(this);
				break;
			case OtpAttemptStatus.Canceled:
				ClaimFailed("Request was canceled");
				App.TriggerCanceledCallback(this);
				break;
		}
	}

	private void ClaimSuccessfully()
	{
		State = OtpRequestState.Claimed;
		Status = OtpRequestStatus.Success;
		VerifiedAt = DateTime.UtcNow;
	}

	private void ClaimFailed(string message)
	{
		State = OtpRequestState.Claimed;
		Status = OtpRequestStatus.Success;
		VerifiedAt = DateTime.UtcNow;
		ErrorMessage = message;
	}
}

public enum OtpRequestState
{
	Available,
	Claimed,
}

public enum OtpRequestStatus
{
	Success,
	Failed
}