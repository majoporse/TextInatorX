using Contracts.Events;
using ImageProcessor.Application.Interfaces;
using SharedKernel.Types;
using Wolverine;
using Wolverine.Attributes;

namespace ImageProcessor.Application.Services.ImageProcessingService.Handlers;

[WolverineHandler]
public class GetImageTextRequestHandler(IMessageBus bus, IImageTextRepository repository)
{
    public async Task<GetImageTextRequestResult> HandleAsync(GetImageTextRequest request)
    {
        var imageText = await repository.GetByImageId(request.ImageId);

        var res = imageText.Match(
            val =>
                new GetImageTextRequestResult
                {
                    IsSuccess = true,
                    Value = new GetImageTextRequest.Response(val.Text)
                },
            err => Err.Failure(err.First()!.Description)
        );

        await bus.SendAsync(res);

        return res;
    }
}