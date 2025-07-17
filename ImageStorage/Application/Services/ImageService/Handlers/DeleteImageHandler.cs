using Application.Interfaces;
using Contracts.Events;
using SharedKernel.Types;
using Wolverine;
using Wolverine.Attributes;

namespace Application.Services.ImageService.Handlers;

[WolverineHandler]
public class DeleteImageHandler(IMessageBus bus, IImageRepository imageRepository, IImageStorage imageStorage)
{
    public async Task<DeleteImageRequestResult> HandleAsync(DeleteImageRequest request,
        CancellationToken cancellationToken)
    {
        var image = await imageRepository.DeleteImageById(request.imageId);
        if (image.IsError)
            return Err.NotFound($"Image with ID {request.imageId} not found.");

        var ok = await imageStorage.DeleteFileAsync(image.Value.FileName, cancellationToken);
        if (!ok)
            return Err.Failure("Image deletion failed.");

        DeleteImageRequestResult res = new DeleteImageRequest.Response(image.Value.MapToImageDto());

        await bus.SendAsync(res);

        return res;
    }
}