using Application.Interfaces;
using Domain.Entities;
using ErrorOr;
using Wolverine.Attributes;

namespace Application.Services.ImageService.Handlers;

public record DeleteImageHandlerRequest(Guid ImageId)
{
    public record Result(Image Image);
}

[WolverineHandler]
public class DeleteImageHandler(IImageRepository imageRepository, IImageStorage imageStorage)
{
    public async Task<ErrorOr<DeleteImageHandlerRequest.Result>> HandleAsync(DeleteImageHandlerRequest request,
        CancellationToken cancellationToken)
    {
        var ok = await imageStorage.DeleteFileAsync(request.ImageId, cancellationToken);
        if (!ok)
            return Error.Failure("Image deletion failed.");

        var image = await imageRepository.DeleteImageById(request.ImageId);
        if (image is null)
            return Error.NotFound($"Image with ID {request.ImageId} not found.");

        return new DeleteImageHandlerRequest.Result(image);
    }
}