using ImageProcessor.Application.Services.ImageProcessingService.Handlers;
using Microsoft.AspNetCore.Http;
using Wolverine;
using Wolverine.Http;

namespace ImageProcessor.Application.Services.ImageProcessingService.Endpoints;

public class TestPayload
{
    public string Image { get; set; }
}

public static class TestDetectEndpoint
{
    [DevOnly]
    [WolverinePost("api/test")]
    public static async Task<IResult> GetAllTexts(IMessageBus bus, TestPayload imagePayload)
    {
        var images = await bus.InvokeAsync<ImageTestRequest.Result>(new ImageTestRequest(imagePayload.Image));
        return images.Text.Match(e => Results.Json(e), e => Results.InternalServerError());
    }
}