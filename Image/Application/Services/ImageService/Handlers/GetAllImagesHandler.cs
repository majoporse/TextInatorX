using Application.Interfaces;
using Domain.Entities;
using ImTools;
using SharedKernel.Types;
using Wolverine.Attributes;

namespace Application.Services.ImageService.Handlers;

public record GetAllImagesHandlerRequest
{
    public record Response(IEnumerable<ImageWithUrl> Images);

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
                ErrorMessage = "An error occurred while retrieving images."
            };
        }
    }
}

[WolverineHandler]
public class GetAllImagesHandler(IImageRepository imageRepository, IImageStorage imageStorage)
{
    public async Task<GetAllImagesHandlerRequest.Result> HandleAsync(GetAllImagesHandlerRequest request,
        CancellationToken cancellationToken = default)
    {
        var image = await imageRepository.GetAllImagesAsync();

        var imageList = image.Value.Map(e => new ImageWithUrl
        {
            Id = e.Id,
            Name = e.Name,
            Url = imageStorage.GetImageUrl(e.FileName),
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt,
            DeletedAt = e.DeletedAt
        });

        return new GetAllImagesHandlerRequest.Response(imageList);
    }
}