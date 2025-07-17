using Domain.Entities;
using SharedKernel.Types;

namespace Application.Interfaces;

public static class Mapper
{
    public static ImageDto MapToImageDto(this Image image)
    {
        return new ImageDto
        {
            Id = image.Id,
            Name = image.Name,
            CreatedAt = image.CreatedAt
        };
    }

    public static ImageWithUrlDto MapToImageDtoWithUrl(this Image image, string url)
    {
        return new ImageWithUrlDto
        {
            Id = image.Id,
            Name = image.Name,
            CreatedAt = image.CreatedAt,
            ImageUrl = url
        };
    }
}