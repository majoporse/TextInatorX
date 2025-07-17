using Application.Interfaces;
using Contracts.Events;
using SharedKernel.Types;
using Wolverine;
using Wolverine.Attributes;

namespace Application.Services.ImageService.Handlers;

[WolverineHandler]
public class GetImagesHandler(IMessageBus bus, IImageRepository imageRepository, IImageStorage imageStorage)
{
    public async Task<GetImageRequestResult> HandleAsync(GetImageRequest request,
        CancellationToken cancellationToken = default)
    {
        var imageRes = await imageRepository.GetImageById(request.ImageId);

        if (imageRes.IsError) return Err.NotFound($"Image with ID {request.ImageId} not found.");

        var image = imageRes.Value;
        var url = imageStorage.GetImageUrl(image.FileName);

        GetImageRequestResult res = new GetImageRequest.Response(image.MapToImageDtoWithUrl(url));

        await bus.SendAsync(res);

        return res;
    }
}