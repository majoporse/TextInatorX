using Domain.Entities;
using ErrorOr;

namespace Application.Interfaces;

public interface IImageRepository
{
    public Task<ErrorOr<IEnumerable<Image>>> GetAllImagesAsync();
    public Task<ErrorOr<Image>> GetImageById(Guid id);
    public Task<ErrorOr<Image>> SaveImage(string name);
    public Task<ErrorOr<Image>> UpdateImage(Image image);
    public Task<ErrorOr<Image>> DeleteImageById(Guid id);
}