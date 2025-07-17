using Domain.Entities;

namespace Application.Interfaces;

public interface IImageService
{
    public Task<Image> UploadFileAsync(string fileName, Stream fileStream);
    public Task<Stream> DownloadFileAsync(string fileName);
    public Task<Image> DeleteFileAsync(string fileName);
}