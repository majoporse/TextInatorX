using Application.Interfaces;
using Domain.Entities;
using ErrorOr;
using Wolverine.Attributes;

namespace Application.Services.ImageService.Handlers;

public record DeleteImageHandlerRequest(Guid imageId)
{
    public record Result(Image Image);
}

[WolverineHandler]
public class DeleteImageHandler(IImageRepository imageRepository, IImageStorage imageStorage)
{
    public async Task<ErrorOr<DeleteImageHandlerRequest.Result>> HandleAsync(DeleteImageHandlerRequest request,
        CancellationToken cancellationToken)
    {
        var image = await imageRepository.DeleteImageById(request.imageId);
        if (image is null)
            return Error.NotFound($"Image with ID {request.imageId} not found.");

        var ok = await imageStorage.DeleteFileAsync(image.FileName, cancellationToken);
        if (!ok)
            return Error.Failure("Image deletion failed.");

        return new DeleteImageHandlerRequest.Result(image);
    }
}