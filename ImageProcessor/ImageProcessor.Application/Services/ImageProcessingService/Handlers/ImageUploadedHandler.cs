using Contracts.Events;
using Domain.Entities;
using ImageProcessor.Application.Interfaces;
using Microsoft.Extensions.Options;
using SharedKernel.Types;
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

        ImageUploadedEventResult res;
        try
        {
            using var loadedImage = Pix.LoadFromMemory(image);
            using var page = engine.Process(loadedImage);

            var text = page.GetText();

            res = new ImageUploadedEvent.Response(request.ImageId, text);
        }
        catch (Exception e)
        {
            res = Err.Failure(e.Message);
        }

        await bus.SendAsync(res);
        
        if (!res.IsSuccess)
        {
            return res;
        }
        
        await repository.Create(new ImageText
        {
            CreatedAt = DateTime.Now,
            Id = Guid.NewGuid(),
            ImageId = request.ImageId,
            Text = res.Value.Text 
        }, cancellationToken);

        return res;
    }
}