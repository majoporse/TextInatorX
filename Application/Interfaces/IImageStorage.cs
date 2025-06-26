namespace Application.Services.Interfaces;

public interface IImageStorage
{
    public Task UploadFileAsync(Guid id, Stream fileStream);
    public Task<Stream> DownloadFileAsync(Guid id);
    public Task<bool> DeleteFileAsync(Guid id);
}