using Application.Interfaces;
using Domain.Entities;
using SharedKernel.Types;
using Wolverine.Attributes;

namespace Application.Services.ImageService.Handlers;

public record AddImageHandlerRequest(Stream FileStream, string name)
{
    public record Response(Image Image, string ImageUrl);

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
                ErrorMessage = "An error occurred while adding the image."
            };
        }
    }
}

[WolverineHandler]
public class AddImageHandler(IImageRepository imageRepository, IImageStorage imagesStorage)
{
    public async Task<AddImageHandlerRequest.Result> HandleAsync(AddImageHandlerRequest request,
        CancellationToken cancellationToken = default)
    {
        var image = await imageRepository.SaveImage(request.name);

        await imagesStorage.UploadFileAsync(image.Value.FileName, request.FileStream, cancellationToken);

        return new AddImageHandlerRequest.Response(image.Value, imagesStorage.GetImageUrl(image.Value.FileName));
    }
}