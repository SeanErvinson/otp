using System.Diagnostics;
using Otp.Core.Domains.Common.Enums;
using Otp.Core.Domains.Common.Models;
using Otp.Core.Domains.Events;
using Otp.Core.Utils;

namespace Otp.Core.Domains.Entities;

[DebuggerDisplay("{Code} - {State} - {Status}")]
public class OtpRequest : TimedEntity
{
	public Guid AppId { get; private set; }
	public string Code { get; private set; } = default!;
	public string SuccessUrl { get; } = default!;
	public string CancelUrl { get; } = default!;
	public string Contact { get; } = default!;
	public Channel Channel { get; }
	public DateTime? VerifiedAt { get; private set; }
	public string Secret { get; } = default!;
	public int Retries { get; private set; }
	public OtpRequestState State { get; private set; }
	public OtpRequestStatus Status { get; private set; }
	public string? ErrorMessage { get; private set; }
	public DateTime ExpiresOn { get; } = DateTime.UtcNow.AddMinutes(5);
	public App App { get; private set; } = default!;
	
	public string RequestPath => new($"/{Enum.GetName(Channel)?.ToLower()}/{Id.ToString()}#{Secret}/");
	
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
		Secret = CryptoUtil.HashKey(CryptoUtil.GenerateKey());
		State = OtpRequestState.Available;
		
		AddDomainEvent(new OtpRequestedEvent(this));
	}

	public void Retry()
	{
		Retries += 1;
		Code = OtpUtil.GenerateCode();
		
		AddDomainEvent(new OtpRequestedEvent(this));
	}

	public void SuccessfullySent()
	{
		Status = OtpRequestStatus.Success;
	}
	
	public void SuccessfullyClaimed()
	{
		State = OtpRequestState.Claimed;
		Status = OtpRequestStatus.Success;
		VerifiedAt = DateTime.UtcNow;
	}
	
	public void FailedClaimed(string message)
	{
		State = OtpRequestState.Claimed;
		Status = OtpRequestStatus.Success;
		VerifiedAt = DateTime.UtcNow;
		ErrorMessage = message;
	}

	public void FailedSent(string message)
	{
		Status = OtpRequestStatus.Failed;
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