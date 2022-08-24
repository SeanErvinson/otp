namespace Otp.Application.Otp.Commands.RequestOtp;

public abstract record RequestOtpRequest
{
	/// <summary>
	/// Where to redirect a user after OTP success
	/// </summary>
	public string SuccessUrl { get; init; } = default!;
	/// <summary>
	/// Where to redirect a user after OTP failed
	/// </summary>
	public string CancelUrl { get; init; } = default!;
}

/// <summary>
/// SMS Otp request
/// </summary>
public record RequestOtpSmsRequest : RequestOtpRequest
{
	/// <summary>The user's phone number. Phone number must include country code.</summary>
	public string PhoneNumber { get; init; } = default!;
}

/// <summary>
/// Email Otp request
/// </summary>
public record RequestOtpEmailRequest : RequestOtpRequest
{
	/// <summary>The user's email address</summary>
	public string EmailAddress { get; init; } = default!;
}