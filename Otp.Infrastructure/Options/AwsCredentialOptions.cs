namespace Otp.Infrastructure.Options;

public sealed record AwsCredentialOptions
{
	public const string Section = "Aws";

	public string AccessKeyId { get; init; } = string.Empty;
	public string SecretKey { get; init; } = string.Empty;
}