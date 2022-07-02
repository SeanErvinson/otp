using Otp.Core.Domains.Common.Models;

namespace Otp.Core.Domains.ValueObjects;

public class OtpAttempt : ValueObject
{
	public string? Code { get; private set; }
	public DateTime AttemptedOn { get; private set; }
	public OtpAttemptStatus AttemptStatus { get; private set; }

	public static OtpAttempt Fail(string? code)
	{
		return new OtpAttempt(OtpAttemptStatus.Fail, code);
	}

	public static OtpAttempt Success(string? code)
	{
		return new OtpAttempt(OtpAttemptStatus.Success, code);
	}

	public static OtpAttempt Cancel()
	{
		return new OtpAttempt(OtpAttemptStatus.Canceled);
	}

	private OtpAttempt()
	{
		
	}
	
	private OtpAttempt(OtpAttemptStatus attemptStatus, string? code = null)
	{
		AttemptedOn = DateTime.UtcNow;
		Code = code;
		AttemptStatus = attemptStatus;
	}
}

public enum OtpAttemptStatus
{
	Success,
	Fail,
	Canceled,
}