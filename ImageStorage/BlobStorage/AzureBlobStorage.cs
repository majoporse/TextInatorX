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

    public async Task UploadFileAsync(string fileName, Stream fileStream, CancellationToken cancellationToken = default)
    {
        if (fileStream.CanSeek) fileStream.Position = 0;
        var inspector = new ContentInspectorBuilder
        {
            Definitions = DefaultDefinitions.All()
        }.Build();

        var contentType = inspector.Inspect(fileStream).ByMimeType().FirstOrDefault()?.MimeType ??
                          "application/octet-stream";

        var options = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType
            }
        };

        var blobClient = blobContainerClient.GetBlobClient(fileName);

        fileStream.Position = 0; // Ensure the stream is at the beginning before upload
        var a = await blobClient.UploadAsync(fileStream, options, cancellationToken);

        if (!a.GetRawResponse().IsError)
            Console.WriteLine($"File uploaded successfully with ID: {fileName}");
        else
            Console.WriteLine($"Failed to upload file with ID: {fileName}");
    }

    public async Task<Stream> DownloadFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var stream = new MemoryStream();

        await blobContainerClient.GetBlobClient(fileName).DownloadToAsync(stream, cancellationToken);
        return stream;
    }

    public async Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var blobClient = blobContainerClient.GetBlobClient(fileName);
        return (await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken)).GetRawResponse().IsError;
    }

    public string GetImageUrl(string fileName)
    {
        return blobContainerClient.Uri + "/" + fileName;
    }
}