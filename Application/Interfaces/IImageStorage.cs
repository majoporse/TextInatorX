namespace Application.Interfaces;

public interface IImageStorage
{
    public Task UploadFileAsync(Guid id, Stream fileStream, CancellationToken cancellationToken = default);
    public Task<Stream> DownloadFileAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<bool> DeleteFileAsync(Guid id, CancellationToken cancellationToken = default);
    public string GetImageUrl(Guid id);
}