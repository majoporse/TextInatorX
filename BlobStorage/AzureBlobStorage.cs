using Application.Interfaces;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace BlobStorage;

public class BlobStorageException : Exception
{
    public BlobStorageException(string message) : base(message)
    {
    }
}

public class AzureBlobStorage : IImageStorage
{
    private readonly BlobContainerClient blobContainerClient;

    public AzureBlobStorage(BlobServiceClient blobServiceClient, IOptions<StorageOptions> storageOptions)
    {
        var client = blobServiceClient.CreateBlobContainer(storageOptions.Value.ContainerName);
        if (client.GetRawResponse().IsError) throw new BlobStorageException(client.GetRawResponse().ToString());

        if (!client.Value.Exists()) throw new BlobStorageException(client.GetRawResponse().ToString());

        blobContainerClient = client.Value;
    }

    public async Task UploadFileAsync(Guid id, Stream fileStream, CancellationToken cancellationToken = default)
    {
        var a = await blobContainerClient.UploadBlobAsync(GetBlobName(id), fileStream, cancellationToken);
        if (!a.GetRawResponse().IsError)
            Console.WriteLine($"File uploaded successfully with ID: {id}");
        else
            Console.WriteLine($"Failed to upload file with ID: {id}");
    }

    public async Task<Stream> DownloadFileAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var stream = new MemoryStream();

        await blobContainerClient.GetBlobClient(GetBlobName(id)).DownloadToAsync(stream, cancellationToken);
        return stream;
    }

    public async Task<bool> DeleteFileAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var blobClient = blobContainerClient.GetBlobClient(GetBlobName(id));
        return (await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken)).GetRawResponse().IsError;
    }

    public string GetImageUrl(Guid id)
    {
        return blobContainerClient.Uri + GetBlobName(id);
    }

    private static string GetBlobName(Guid id)
    {
        return $"images/{id}";
    }
}