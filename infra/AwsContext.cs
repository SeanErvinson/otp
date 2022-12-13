namespace Otp.Infra.Aws;

public record AwsContext(Input<string> AccountId, Input<string> Region);