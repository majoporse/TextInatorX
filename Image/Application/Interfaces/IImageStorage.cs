namespace Application.Interfaces;

public interface IImageStorage
{
    public Task UploadFileAsync(string fileName, Stream fileStream, CancellationToken cancellationToken = default);
    public Task<Stream> DownloadFileAsync(string fileName, CancellationToken cancellationToken = default);
    public Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken = default);
    public string GetImageUrl(string fileName);
}