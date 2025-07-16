using Application.Interfaces;
using Domain.Entities;
using SharedKernel.Types;
using Wolverine.Attributes;

namespace Application.Services.ImageService.Handlers;

public record DeleteImageHandlerRequest(Guid imageId)
{
    public record Response(Image Image);

    public record Result : Res<Response>
    {
        public static implicit operator Result(Response response)
        {
            return new Result
            {
                IsSuccess = true,
                Value = response
            };
        }

        public static implicit operator Result(Err error)
        {
            return new Result
            {
                IsSuccess = false,
                ErrorMessage = error.ErrorMessage
            };
        }
    }
}

[WolverineHandler]
public class DeleteImageHandler(IImageRepository imageRepository, IImageStorage imageStorage)
{
    public async Task<DeleteImageHandlerRequest.Result> HandleAsync(DeleteImageHandlerRequest request,
        CancellationToken cancellationToken)
    {
        var image = await imageRepository.DeleteImageById(request.imageId);
        if (image.IsError)
            return Err.NotFound($"Image with ID {request.imageId} not found.");

        var ok = await imageStorage.DeleteFileAsync(image.Value.FileName, cancellationToken);
        if (!ok)
            return Err.Failure("Image deletion failed.");

        return new DeleteImageHandlerRequest.Response(image.Value);
    }
}