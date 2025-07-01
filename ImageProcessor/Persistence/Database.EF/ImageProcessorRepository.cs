using Domain.Entities;
using ErrorOr;

namespace Persistence.Database.EF;

public class ImageProcessorRepository(Repository<ImageText> repository)
{
    public async Task<ErrorOr<ImageText>> Save(ImageText imageText)
    {
        await repository.Insert(imageText);
        await repository.SaveChangesAsync();
        return imageText;
    }

    public async Task<ErrorOr<ImageText>> Delete(ImageText imageText)
    {
        var text = await repository.GetByIDAsync(imageText.Id);
        if (text == null)
            return Error.NotFound("ImageText not found");

        repository.Delete(imageText);
        await repository.SaveChangesAsync();
        return imageText;
    }

    public async Task<ErrorOr<ImageText>> Update(ImageText imageText)
    {
        repository.Update(imageText);
        await repository.SaveChangesAsync();
        return imageText;
    }
}