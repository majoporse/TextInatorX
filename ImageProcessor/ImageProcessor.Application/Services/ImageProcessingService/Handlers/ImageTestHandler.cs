using ErrorOr;
using ImageProcessor.Application.Interfaces;
using Tesseract;
using Wolverine;
using Wolverine.Attributes;

namespace ImageProcessor.Application.Services.ImageProcessingService.Handlers;

public record ImageTestRequest(string image)
{
    public record Result(ErrorOr<string> Text);
}

[WolverineHandler]
public class ImageTestHandler(IMessageBus bus, IImageTextRepository repository)
{
    public async Task<ImageTestRequest.Result> HandleAsync(ImageTestRequest request,
        CancellationToken cancellationToken = default)
    {
        var imageBytes = Convert.FromBase64String(request.image);
        var path = Path.Combine(Environment.CurrentDirectory, "..", "ImageProcessor.Application", "tessdata_fast");

        using var engine = new TesseractEngine(path, "eng");

        using var image = Pix.LoadFromMemory(imageBytes);
        using var page = engine.Process(image);

        var text = page.GetText();

        var res = new ImageTestRequest.Result(text);
        return res;
    }
}