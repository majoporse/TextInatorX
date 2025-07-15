using Domain.Entities;
using EntityFramework.Exceptions.Common;
using ErrorOr;
using ImageProcessor.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database.EF;

public class ImageProcessorRepository(ImageProcessorDbContext context) : IImageTextRepository
{
    private readonly Repository<ImageText> repository = new(context);

    public async Task<ErrorOr<ImageText>> Create(ImageText imageText, CancellationToken cancellationToken = default)
    {
        try
        {
            await repository.Insert(imageText);
            await repository.SaveChangesAsync(cancellationToken);
        }
        catch (UniqueConstraintException e)
        {
            return Error.Conflict(e.Message);
        }

        return imageText;
    }

    public async Task<ErrorOr<ImageText>> Delete(ImageText imageText, CancellationToken cancellationToken = default)
    {
        try
        {
            repository.Delete(imageText);
            await repository.SaveChangesAsync(cancellationToken);
        }
        catch (ReferenceConstraintException e)
        {
            return Error.Conflict(e.Message);
        }

        return imageText;
    }

    public async Task<ErrorOr<ImageText>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var imageText = await repository.GetByIDAsync(id, cancellationToken);
        if (imageText == null)
            return Error.NotFound("ImageText not found");
        return imageText;
    }

    public async Task<ErrorOr<ImageText>> GetByImageId(Guid imageId, CancellationToken cancellationToken = default)
    {
        var imageText = await repository.Get(e => e.ImageId == imageId)
            .FirstOrDefaultAsync(cancellationToken);
        if (imageText == null)
            return Error.NotFound($"ImageText not found for ImageId: {imageId}");
        return imageText;
    }

    public async Task<ErrorOr<IEnumerable<ImageText>>> GetAll(CancellationToken cancellationToken = default)
    {
        var texts = await context.ImagesWithText.ToListAsync(cancellationToken);
        return texts;
    }

    public async Task<ErrorOr<ImageText>> Update(ImageText imageText, CancellationToken cancellationToken = default)
    {
        try
        {
            repository.Update(imageText);
            await repository.SaveChangesAsync(cancellationToken);
        }
        catch (ReferenceConstraintException e)
        {
            return Error.Conflict(e.Message);
        }

        return imageText;
    }
}