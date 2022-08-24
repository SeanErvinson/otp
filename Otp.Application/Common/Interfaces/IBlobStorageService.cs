namespace Otp.Application.Common.Interfaces;

public interface IBlobStorageService
{
	Task<Uri> UploadBlobAsync(string containerName,
		string blobPath,
		Stream stream,
		string contentType,
		bool publiclyAccessible = false,
		IDictionary<string, string>? metadata = null,
		CancellationToken cancellationToken = default);

	Task<Stream> DownloadBlobAsync(string uri, CancellationToken cancellationToken = default);
}