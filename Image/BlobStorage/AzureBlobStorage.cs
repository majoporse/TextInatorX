using Application.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using MimeDetective;
using MimeDetective.Definitions;

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
        blobContainerClient = blobServiceClient.GetBlobContainerClient(storageOptions.Value.ContainerName);
        blobContainerClient.CreateIfNotExists();
        blobContainerClient.SetAccessPolicy(PublicAccessType.Blob);
    }

    public async Task UploadFileAsync(Guid id, Stream fileStream, CancellationToken cancellationToken = default)
    {
        if (fileStream.CanSeek) fileStream.Position = 0;
        var inspector = new ContentInspectorBuilder
        {
            Definitions = DefaultDefinitions.All()
        }.Build();

        var contentType = inspector.Inspect(fileStream).ByMimeType().FirstOrDefault()?.MimeType ?? "application/octet-stream";

        var options = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType
            }
        };
        
        var blobClient = blobContainerClient.GetBlobClient(GetBlobName(id));
        
        fileStream.Position = 0; // Ensure the stream is at the beginning before upload
        var a = await blobClient.UploadAsync(fileStream, options, cancellationToken);
        
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
        return blobContainerClient.Uri + "/" + GetBlobName(id);
    }

    private static string GetBlobName(Guid id)
    {
        return $"images/{id}";
    }
}