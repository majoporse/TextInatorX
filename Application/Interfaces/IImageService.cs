using Domain.Entities;

namespace Application.Services.Interfaces;

public interface IImageService
{
    public Task<Image> UploadFileAsync(string fileName, Stream fileStream);
    public Task<Stream> DownloadFileAsync(string fileName);
    public Task<Image> DeleteFileAsync(string fileName);
}