using Contracts.Events;
using ImageProcessor.Application.Interfaces;
using Wolverine;
using Wolverine.Attributes;

namespace ImageProcessor.Application.Services.ImageProcessingService.Handlers;

[WolverineHandler]
public class GetImageTextRequestHandler(IMessageBus bus, IImageTextRepository repository)
{
   public async Task<GetImageTextRequestResult> HandleAsync(GetImageTextRequest request)
   {
      var imageText = await repository.GetByImageId(request.ImageId);
      
      return new GetImageTextRequestResult(imageText.Value.Text);
   } 
}