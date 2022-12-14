namespace Otp.Infra.Aws;

public record DeploymentContext
{
	public string Application { get; init; } = string.Empty;
	public string Environment { get; init; } = string.Empty;

	public override string ToString() => $"{Application}-{Environment}";
}