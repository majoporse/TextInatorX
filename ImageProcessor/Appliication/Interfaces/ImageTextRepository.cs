using Domain.Entities;
using ErrorOr;

namespace Appliication.Interfaces;

public interface ImageTextRepository
{

    public Task<ErrorOr<ImageText>> Update(ImageText imageText);
}