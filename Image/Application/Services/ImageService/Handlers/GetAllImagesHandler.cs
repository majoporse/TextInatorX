using Application.Interfaces;
using Domain.Entities;
using ErrorOr;
using ImTools;
using Wolverine.Attributes;

namespace Application.Services.ImageService.Handlers;

public record GetAllImagesHandlerRequest
{
    public record Result(IEnumerable<ImageWithUrl> Images);
}

[WolverineHandler]
public class GetAllImagesHandler(IImageRepository imageRepository, IImageStorage imageStorage)
{
    public async Task<ErrorOr<GetAllImagesHandlerRequest.Result>> HandleAsync(GetAllImagesHandlerRequest request,
        CancellationToken cancellationToken = default)
    {
        var image = await imageRepository.GetAllImagesAsync();
        var imageList = image.Map(e => new ImageWithUrl
        {
            Id = e.Id,
            Name = e.Name,
            Url = imageStorage.GetImageUrl(e.FileName),
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt,
            DeletedAt = e.DeletedAt
        });

        return new GetAllImagesHandlerRequest.Result(imageList);
    }
}