using Application.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using ErrorOr;

namespace Application.Services.ImageService.Handlers;

public record AddImageHandlerRequest(string Name, Stream FileStream)
{
    public record Result(Image Image);
}

public class AddImageHandler(IImageRepository imageRepository, IImageStorage imagesStorage)
{
    public async Task<ErrorOr<AddImageHandlerRequest.Result>> HandleAsync(AddImageHandlerRequest request,
        CancellationToken cancellationToken = default)
    {
        var image = await imageRepository.SaveImage(request.Name);
        if (image is null)
            return Error.NotFound($"Image with ID {request.Name} not found.");
        
        await imagesStorage.UploadFileAsync(image.Id, request.FileStream);

        return new AddImageHandlerRequest.Result(image);
    }
}