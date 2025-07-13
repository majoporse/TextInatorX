using Contracts.Events;
using Domain.Entities;
using ImageProcessor.Application.Interfaces;
using Microsoft.Extensions.Options;
using Tesseract;
using Wolverine;
using Wolverine.Attributes;

namespace ImageProcessor.Application.Services.ImageProcessingService.Handlers;

[WolverineHandler]
public class ImageUploadedHandler(IMessageBus bus, IImageTextRepository repository, IOptions<TesseractOptions> options)
{
    public async Task<ImageUploadedEventResult> HandleAsync(ImageUploadedEvent request,
        CancellationToken cancellationToken = default)
    {
        var client = new HttpClient();
        var reply = await client.GetAsync(request.ImageUrl, cancellationToken);
        var image = await reply.Content.ReadAsByteArrayAsync(cancellationToken);

        using var engine = new TesseractEngine(options.Value.DataPath, options.Value.Language);

        using var loadedImage = Pix.LoadFromMemory(image);
        using var page = engine.Process(loadedImage);

        var text = page.GetText();

        var res = new ImageUploadedEventResult(request.ImageId, text);
        await bus.SendAsync(res);

        await repository.Create(new ImageText
        {
            CreatedAt = DateTime.Now,
            Id = Guid.NewGuid(),
            ImageId = request.ImageId,
            Text = text
        }, cancellationToken);

        return res;
    }
}