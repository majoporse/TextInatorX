using Domain.Entities;
using ErrorOr;

namespace ImageProcessor.Application.Interfaces;

public interface IImageTextRepository
{
    public Task<ErrorOr<IEnumerable<ImageText>>> GetAll(CancellationToken cancellationToken = default);
    public Task<ErrorOr<ImageText>> Update(ImageText imageText, CancellationToken cancellationToken = default);
    public Task<ErrorOr<ImageText>> Create(ImageText imageText, CancellationToken cancellationToken = default);
    public Task<ErrorOr<ImageText>> Delete(ImageText imageText, CancellationToken cancellationToken = default);
    public Task<ErrorOr<ImageText>> GetById(Guid id, CancellationToken cancellationToken = default);
    public Task<ErrorOr<ImageText>> GetByImageId(Guid imageId, CancellationToken cancellationToken = default);
}