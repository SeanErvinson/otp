using Otp.Core.Domains.Common;
using Otp.Core.Domains.Events;
using Otp.Core.Utils;

namespace Otp.Core.Domains.Entities;

public class OtpRequest : TimedEntity
{
	public Guid AppId { get; private set; }
	public string Code { get; private set; }
	public string SuccessUrl { get; private set; }
	public string CancelUrl { get; private set; }
	public string Contact { get; private set; }
	public Mode Mode { get; }

	public DateTime? VerifiedAt { get; private set; }
	public DateTime ExpiresOn { get; } = DateTime.UtcNow.AddMinutes(5);
	public string Secret { get; private set; }
	public int Retries { get; private set; } = 0;
	public OtpRequestState State { get; private set; }
	public OtpRequestStatus Status { get; private set; }
	public string? ErrorMessage { get; private set; }
	public string RequestPath => new($"/{Enum.GetName(Mode)?.ToLower()}/{Id.ToString()}#{Secret}/");

	public App App { get; set; }

	private OtpRequest()
	{
	}

	public OtpRequest(Guid appId, string contact, Mode mode, string successUrl, string cancelUrl)
	{
		AppId = appId;
		Contact = contact;
		Mode = mode;
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