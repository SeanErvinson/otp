namespace Otp.Api.Options;

public class AzureB2COptions
{
	public const string Section = "AzureB2C";
	public string Instance { get; set; } = string.Empty;
	public string ClientId { get; set; } = string.Empty;
	public string Domain { get; set; } = string.Empty;
	public string SignInPolicy { get; set; } = string.Empty;
	public string Authority => $"{Instance}/{Domain}/{SignInPolicy}/v2.0".ToString();
}