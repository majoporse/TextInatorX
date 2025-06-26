using Application.Interfaces;
using Domain.Entities;
using ErrorOr;

namespace Application.Services.ImageService.Handlers;

public record GetImagesHandlerRequest(Guid ImageId)
{
    public record Result(Image Image);
}

public class GetImagesHandler(IImageRepository imageRepository)
{
    public async Task<ErrorOr<GetImagesHandlerRequest.Result>> HandleAsync(GetImagesHandlerRequest request,
        CancellationToken cancellationToken = default)
    {
        var image = await imageRepository.GetImageById(request.ImageId);
        if (image is null) return Error.NotFound($"Image with ID {request.ImageId} not found.");

        return new GetImagesHandlerRequest.Result(image);
    }
}