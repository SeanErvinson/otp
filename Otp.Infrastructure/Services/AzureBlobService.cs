using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using Otp.Application.Common.Interfaces;
using Otp.Infrastructure.Options;

namespace Otp.Infrastructure.Services;

public class AzureBlobStorageService : IBlobStorageService
{
	private readonly StorageAccountOptions _options;

	public AzureBlobStorageService(IOptions<StorageAccountOptions> options)
	{
		_options = options.Value;
	}

	public async Task<Uri> UploadBlobAsync(string containerName,
		string blobPath,
		Stream stream,
		string contentType,
		bool publiclyAccessible = false,
		IDictionary<string, string>? metadata = null,
		CancellationToken cancellationToken = default)
	{
		var container = GetContainerClient(containerName);
		await container.CreateIfNotExistsAsync(publiclyAccessible ? PublicAccessType.Blob : PublicAccessType.None,
			cancellationToken: cancellationToken);
		var blobClient = container.GetBlobClient(blobPath);
		await blobClient.UploadAsync(stream,
			new BlobHttpHeaders { ContentType = contentType },
			metadata,
			cancellationToken: cancellationToken);
		return blobClient.Uri;
	}

	public Task<Stream> DownloadBlobAsync(string uri, CancellationToken cancellationToken = default) =>
		throw new NotImplementedException();

	private BlobContainerClient GetContainerClient(string containerName)
	{
		var serviceClient = new BlobServiceClient(_options.BlobStorage.ConnectionString);
		return serviceClient.GetBlobContainerClient(containerName);
	}
}