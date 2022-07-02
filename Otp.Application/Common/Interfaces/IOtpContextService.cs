namespace Otp.Application.Common.Interfaces;

public interface IOtpContextService
{
	public string? AuthenticityKey { get; }
}