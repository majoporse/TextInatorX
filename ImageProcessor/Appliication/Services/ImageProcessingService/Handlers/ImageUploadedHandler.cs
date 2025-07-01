using Contracts.Events;
using Wolverine.Attributes;

namespace Appliication.Services.ImageProcessingService.Handlers;

[WolverineHandler]
public class ImageUploadedHandler
{
    public async Task<ImageUploadedEvent.Result> HandleAsync(ImageUploadedEvent request,
        CancellationToken cancellationToken = default)
    {
        Thread.Sleep(10000); // Simulate some processing delay

        return new ImageUploadedEvent.Result(request.ImageId, "kokotinka");
    }
}