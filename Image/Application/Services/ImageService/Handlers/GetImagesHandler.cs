using Application.Interfaces;
using Domain.Entities;
using Wolverine.Attributes;

namespace Application.Services.ImageService.Handlers;

public record GetImageRequest(Guid ImageId)
{
    public record Result(ImageWithUrl Image);
}

[WolverineHandler]
public class GetImagesHandler(IImageRepository imageRepository, IImageStorage imageStorage)
{
    public async Task<GetImageRequest.Result> HandleAsync(GetImageRequest request,
        CancellationToken cancellationToken = default)
    {
        var image = await imageRepository.GetImageById(request.ImageId);
        // if (image is null) return Error.NotFound($"Image with ID {request.ImageId} not found.");
        var url = imageStorage.GetImageUrl(image.FileName);

        return new GetImageRequest.Result(new ImageWithUrl
        {
            Id = image.Id,
            Url = url,
            CreatedAt = image.CreatedAt,
            UpdatedAt = image.UpdatedAt,
            DeletedAt = image.DeletedAt
        });
    }
}