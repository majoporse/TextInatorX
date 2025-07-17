using Application.Interfaces;
using Contracts.Events;
using ImTools;
using SharedKernel.Types;
using Wolverine;
using Wolverine.Attributes;

namespace Application.Services.ImageService.Handlers;

[WolverineHandler]
public class GetAllImagesHandler(IMessageBus bus, IImageRepository imageRepository, IImageStorage imageStorage)
{
    public async Task<GetAllImagesRequestResult> HandleAsync(GetAllImagesRequest request,
        CancellationToken cancellationToken = default)
    {
        var image = await imageRepository.GetAllImagesAsync();

        var imageList = image.Value.Map(e => new ImageWithUrlDto
        {
            Id = e.Id,
            Name = e.Name,
            ImageUrl = imageStorage.GetImageUrl(e.FileName),
            CreatedAt = e.CreatedAt
        });

        GetAllImagesRequestResult res = new GetAllImagesRequest.Response(imageList);

        await bus.SendAsync(res);

        return res;
    }
}