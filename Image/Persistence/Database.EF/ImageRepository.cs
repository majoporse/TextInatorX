using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database.EF;

public class ImageRepository : IImageRepository
{
    private readonly Repository<Image> repository;

    public ImageRepository(ApplicationDbContext context)
    {
        repository = new Repository<Image>(context);
    }

    public async Task<Image?> GetImageById(Guid id)
    {
        return (await repository.Get(image => image.Id == id).ToListAsync()).FirstOrDefault();
    }

    public async Task<Image?> UpdateImage(Image image)
    {
        repository.Update(image);
        await repository.SaveChangesAsync();
        return image;
    }

    public async Task<Image?> DeleteImageById(Guid id)
    {
        var image = await GetImageById(id);
        if (image == null)
            return null;
        repository.Delete(id);
        await repository.SaveChangesAsync();
        return image;
    }

    public async Task<IEnumerable<Image>> GetAllImagesAsync()
    {
        return await repository.Get().ToListAsync();
    }

    public async Task<Image?> SaveImage(string name)
    {
        var imageId = Guid.NewGuid();
        var image = new Image
        {
            Name = name,
            Id = imageId,
            FileName = imageId + Path.GetExtension(name)
        };
        try
        {
            await repository.Insert(image);
            await repository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return null;
        }

        return image;
    }
}