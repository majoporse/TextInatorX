using Domain.Entities;

namespace Application.Interfaces;

public interface IImageRepository
{
    public Task<Image?> GetImageById(Guid id);
    public Task<Image?> SaveImage(string name);
    public Task<Image?> UpdateImage(Image image);
    public Task<Image?> DeleteImageById(Guid id);
}