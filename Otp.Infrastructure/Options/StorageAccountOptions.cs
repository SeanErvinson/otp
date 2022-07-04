namespace Otp.Infrastructure.Options;

public sealed class StorageAccountOptions
{
	public static string Section = "Azure:StorageAccount";
	public Connection BlobStorage { get; set; }
}

public sealed class Connection
{
	public string ConnectionString { get; set; }
}