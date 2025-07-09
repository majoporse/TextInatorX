using Contracts.Events;
using Domain.Entities;
using ImageProcessor.Application.Interfaces;
using Wolverine;
using Wolverine.Attributes;

namespace ImageProcessor.Application.Services.ImageProcessingService.Handlers;

[WolverineHandler]
public class ImageUploadedHandler(IMessageBus bus, IImageTextRepository repository)
{
    public async Task<ImageUploadedEventResult> HandleAsync(ImageUploadedEvent request,
        CancellationToken cancellationToken = default)
    {
        var imageText = await repository.Create(new ImageText
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            ImageId = request.ImageId,
            Text = "kokotinka"
        }, cancellationToken);

        var res = new ImageUploadedEventResult(request.ImageId, "kokotinka");
        await bus.SendAsync(res);
        return res;
    }
}