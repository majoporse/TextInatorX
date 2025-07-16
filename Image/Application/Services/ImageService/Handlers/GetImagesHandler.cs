using Application.Interfaces;
using Domain.Entities;
using SharedKernel.Types;
using Wolverine.Attributes;

namespace Application.Services.ImageService.Handlers;

public record GetImageRequest(Guid ImageId)
{
    public record Response(ImageWithUrl Image);

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
public class GetImagesHandler(IImageRepository imageRepository, IImageStorage imageStorage)
{
    public async Task<GetImageRequest.Result> HandleAsync(GetImageRequest request,
        CancellationToken cancellationToken = default)
    {
        var res = await imageRepository.GetImageById(request.ImageId);

        if (res.IsError) return Err.NotFound($"Image with ID {request.ImageId} not found.");

        var image = res.Value;
        var url = imageStorage.GetImageUrl(image.FileName);

        return new GetImageRequest.Response(new ImageWithUrl
        {
            Id = image.Id,
            Url = url,
            CreatedAt = image.CreatedAt,
            UpdatedAt = image.UpdatedAt,
            DeletedAt = image.DeletedAt
        });
    }
}