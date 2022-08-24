namespace Otp.Api.Options;

public record AzureB2COptions
{
	public const string Section = "AzureB2C";
	public string Instance { get; init; } = default!;
	public string ClientId { get; init; } = default!;
	public string Domain { get; init; } = default!;
	public string SignInPolicy { get; init; } = default!;

	public string Authority => $"{Instance}/{Domain}/{SignInPolicy}/v2.0".ToString();

	public ApiConnector ApiConnector { get; init; } = default!;
}

public class ApiConnector
{
	public ApiConnectorAuth OnCreate { get; set; } = default!;
}

public class ApiConnectorAuth
{
	public string Username { get; set; } = default!;
	public string Password { get; set; } = default!;
}