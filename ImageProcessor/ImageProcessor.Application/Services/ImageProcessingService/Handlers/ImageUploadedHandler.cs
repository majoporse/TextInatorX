using System.Text;
using Contracts.Events;
using ImageProcessor.Application.Interfaces;
using Tesseract;
using Wolverine;
using Wolverine.Attributes;

namespace ImageProcessor.Application.Services.ImageProcessingService.Handlers;

[WolverineHandler]
public class ImageUploadedHandler(IMessageBus bus, IImageTextRepository repository)
{
    public async Task<ImageUploadedEventResult> HandleAsync(ImageUploadedEvent request,
        CancellationToken cancellationToken = default)
    {
        var client = new HttpClient();
        var reply = await client.GetAsync(request.ImageUrl, cancellationToken);
        var image = await reply.Content.ReadAsStringAsync(cancellationToken);
        var imageBytes = Encoding.UTF8.GetBytes(image);
        
        var image64 = Convert.ToBase64String(imageBytes);

        var path = Path.Combine(Environment.CurrentDirectory, "..", "ImageProcessor.Application", "tessdata_fast");

        using var engine = new TesseractEngine(path, "eng");

        using var loadedImage = Pix.LoadFromMemory(imageBytes);
        using var page = engine.Process(loadedImage);

        var text = page.GetText();

        var res = new ImageUploadedEventResult(request.ImageId, text);
        await bus.SendAsync(res);
        return res;
    }
}