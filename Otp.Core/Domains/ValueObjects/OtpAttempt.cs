using System.Diagnostics;
using Otp.Core.Domains.Common.Models;

namespace Otp.Core.Domains.ValueObjects;

[DebuggerDisplay("{Code} - {IpAddress}")]
public class OtpAttempt : ValueObject
{
	public string? Code { get; private set; }
	public string IpAddress { get; private set; }
	public string UserAgent { get; private set; }
	public DateTime AttemptedOn { get; private set; }
	public OtpAttemptStatus AttemptStatus { get; private set; }

	public static OtpAttempt Fail(string ipAddress, string userAgent, string? code)
	{
		return new OtpAttempt(ipAddress, userAgent, OtpAttemptStatus.Fail, code);
	}

	public static OtpAttempt Success(string ipAddress, string userAgent, string? code)
	{
		return new OtpAttempt(ipAddress, userAgent, OtpAttemptStatus.Success, code);
	}

	public static OtpAttempt Cancel(string ipAddress, string userAgent)
	{
		return new OtpAttempt(ipAddress, userAgent, OtpAttemptStatus.Canceled);
	}

	private OtpAttempt()
	{
		
	}
	
	private OtpAttempt(string ipAddress, string userAgent, OtpAttemptStatus attemptStatus, string? code = null)
	{
		AttemptedOn = DateTime.UtcNow;
		Code = code;
		IpAddress = ipAddress;
		UserAgent = userAgent;
		AttemptStatus = attemptStatus;
	}
}

public enum OtpAttemptStatus
{
	Success,
	Fail,
	Canceled,
}