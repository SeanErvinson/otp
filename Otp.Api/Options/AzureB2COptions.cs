namespace Otp.Api.Options;

public class AzureB2COptions
{
	public const string Section = "AzureB2C";
	public string Instance { get; set; } = default!;
	public string ClientId { get; set; } = default!;
	public string Domain { get; set; } = default!;
	public string SignInPolicy { get; set; } = default!;
	public string Authority => $"{Instance}/{Domain}/{SignInPolicy}/v2.0".ToString();

	public ApiConnector ApiConnector { get; set; } = default!;
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