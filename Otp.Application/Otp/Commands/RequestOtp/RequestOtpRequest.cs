namespace Otp.Application.Otp.Commands.RequestOtp;

public abstract record RequestOtpRequest
{
	public string SuccessUrl { get; init; } = default!;
	public string CancelUrl { get; init; } = default!;
}

public record RequestOtpSmsRequest : RequestOtpRequest
{
	public string PhoneNumber { get; init; } = default!;
}

public record RequestOtpEmailRequest : RequestOtpRequest
{
	public string EmailAddress { get; init; } = default!;
}