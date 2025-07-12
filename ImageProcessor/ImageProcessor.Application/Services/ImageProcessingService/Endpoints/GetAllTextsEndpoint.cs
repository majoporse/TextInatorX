using ImageProcessor.Application.Services.ImageProcessingService.Handlers;
using Microsoft.AspNetCore.Http;
using Wolverine;
using Wolverine.Http;

namespace ImageProcessor.Application.Services.ImageProcessingService.Endpoints;

public static class GetAllTextsEndpoint
{
    [DevOnly]
    [WolverineGet("api/texts")]
    public static async Task<IResult> GetAllTexts(IMessageBus bus)
    {
        var images = await bus.InvokeAsync<GetAllImageTextRequest.Response>(new GetAllImageTextRequest());
        return images.Texts.Match(e => Results.Json(e), e => Results.InternalServerError());
    }
}