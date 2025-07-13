using Application.Interfaces;
using Domain.Entities;
using ErrorOr;
using Wolverine.Attributes;

namespace Application.Services.ImageService.Handlers;

public record GetImagesHandlerRequest(Guid ImageId)
{
    public record Result(ImageWithUrl Image);
}

[WolverineHandler]
public class GetImagesHandler(IImageRepository imageRepository, IImageStorage imageStorage)
{
    public async Task<ErrorOr<GetImagesHandlerRequest.Result>> HandleAsync(GetImagesHandlerRequest request,
        CancellationToken cancellationToken = default)
    {
        var image = await imageRepository.GetImageById(request.ImageId);
        if (image is null) return Error.NotFound($"Image with ID {request.ImageId} not found.");
        var url = imageStorage.GetImageUrl(image.FileName);

        return new GetImagesHandlerRequest.Result(new ImageWithUrl
        {
            Id = image.Id,
            Url = url,
            CreatedAt = image.CreatedAt,
            UpdatedAt = image.UpdatedAt,
            DeletedAt = image.DeletedAt
        });
    }
}