using Application.Services.Interfaces;
using Azure.Storage.Blobs;

namespace BlobStorage;

public class AzureBlobStorage : IImageStorage
{
    private readonly BlobContainerClient blobContainerClient;

    public AzureBlobStorage(string connectionString, string containerName)
    {
        var blobServiceClient = new BlobServiceClient(connectionString);
        blobContainerClient = blobServiceClient.CreateBlobContainer(containerName);
    }
    
    public async Task UploadFileAsync(Guid id, Stream fileStream)
    {
        var a = await blobContainerClient.UploadBlobAsync($"images/{id}", fileStream);
        if (!a.GetRawResponse().IsError)
        {
            Console.WriteLine($"File uploaded successfully with ID: {id}");
        }
        else
        {
            Console.WriteLine($"Failed to upload file with ID: {id}");
        }
    }

    public Task<Stream> DownloadFileAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteFileAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}