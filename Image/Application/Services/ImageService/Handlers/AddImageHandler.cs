using Application.Interfaces;
using Domain.Entities;
using Wolverine.Attributes;

namespace Application.Services.ImageService.Handlers;

public record AddImageHandlerRequest(Stream FileStream, string name)
{
    public record Result(Image Image, string ImageUrl);
}

[WolverineHandler]
public class AddImageHandler(IImageRepository imageRepository, IImageStorage imagesStorage)
{
    public async Task<AddImageHandlerRequest.Result> HandleAsync(AddImageHandlerRequest request,
        CancellationToken cancellationToken = default)
    {
        var image = await imageRepository.SaveImage(request.name);
        // if (image is null)
        //     return Error.NotFound("Could not save image");

        await imagesStorage.UploadFileAsync(image.Id, request.FileStream, cancellationToken);
        var imageUrl = imagesStorage.GetImageUrl(image.Id);

        return new AddImageHandlerRequest.Result(image, imageUrl);
    }
}