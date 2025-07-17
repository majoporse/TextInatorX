using Application.Interfaces;
using Contracts.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Application.Services.ImageService.Handlers;

[WolverineHandler]
public class AddImageHandler(IMessageBus bus, IImageRepository imageRepository, IImageStorage imagesStorage)
{
    public async Task<AddImageRequestResult> HandleAsync(AddImageRequest request,
        CancellationToken cancellationToken = default)
    {
        var image = await imageRepository.SaveImage(request.name);
        using var memoryStream = new MemoryStream(request.file);

        await imagesStorage.UploadFileAsync(image.Value.FileName, memoryStream, cancellationToken);

        AddImageRequestResult res = new AddImageRequest.Response(image.Value.MapToImageDto(),
            imagesStorage.GetImageUrl(image.Value.FileName));

        await bus.SendAsync(res);

        return res;
    }
}