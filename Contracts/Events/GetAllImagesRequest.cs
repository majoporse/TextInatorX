using SharedKernel.Types;

namespace Contracts.Events;

public record GetAllImagesRequest
{
    public record Response(IEnumerable<ImageWithUrlDto> Images);
}

public record GetAllImagesRequestResult : Res<GetAllImagesRequest.Response>
{
    public static implicit operator GetAllImagesRequestResult(GetAllImagesRequest.Response response)
    {
        return new GetAllImagesRequestResult
        {
            IsSuccess = true,
            Value = response
        };
    }

    public static implicit operator GetAllImagesRequestResult(Err error)
    {
        return new GetAllImagesRequestResult
        {
            IsSuccess = false,
            ErrorMessage = error.ErrorMessage
        };
    }
}