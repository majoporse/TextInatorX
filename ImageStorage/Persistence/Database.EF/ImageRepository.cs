using Application.Interfaces;
using Domain.Entities;
using EntityFramework.Exceptions.Common;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database.EF;

public class ImageRepository : IImageRepository
{
    private readonly Repository<Image> repository;

    public ImageRepository(ApplicationDbContext context)
    {
        repository = new Repository<Image>(context);
    }

    public async Task<ErrorOr<Image>> GetImageById(Guid id)
    {
        try
        {
            var image = (await repository.Get(image => image.Id == id).ToListAsync()).FirstOrDefault();
            if (image == null) return Error.NotFound($"Image with ID {id} not found.");
            return image;
        }
        catch (ReferenceConstraintException e)
        {
            return Error.NotFound(e.Message);
        }
    }

    public async Task<ErrorOr<Image>> UpdateImage(Image image)
    {
        try
        {
            repository.Update(image);
            await repository.SaveChangesAsync();
        }
        catch (ReferenceConstraintException e)
        {
            return Error.NotFound(e.Message);
        }

        return image;
    }

    public async Task<ErrorOr<Image>> DeleteImageById(Guid id)
    {
        try
        {
            var image = await repository.GetByIDAsync(id);
            if (image == null) return Error.NotFound($"Image with ID {id} not found.");

            repository.Delete(id);
            await repository.SaveChangesAsync();

            return image;
        }
        catch (ReferenceConstraintException e)
        {
            return Error.NotFound(e.Message);
        }
    }

    public async Task<ErrorOr<IEnumerable<Image>>> GetAllImagesAsync()
    {
        return await repository.Get().ToListAsync();
    }

    public async Task<ErrorOr<Image>> SaveImage(string name)
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
        catch (UniqueConstraintException e)
        {
            return Error.Conflict(e.Message);
        }
        catch (ReferenceConstraintException ex)
        {
            return Error.NotFound(ex.Message);
        }

        return image;
    }
}