namespace Otp.Infra.Aws;

public record DeploymentContext
{
	public string Application { get; init; }
	public string Environment { get; init; }

	public override string ToString() => $"{Application}-{Environment}";
}