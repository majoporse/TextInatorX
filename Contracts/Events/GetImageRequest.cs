using SharedKernel.Types;

namespace Contracts.Events;

public record GetImageRequest(Guid ImageId)
{
    public record Response(ImageWithUrlDto ImageWithUrlDto);
}

public record GetImageRequestResult : Res<GetImageRequest.Response>
{
    public static implicit operator GetImageRequestResult(GetImageRequest.Response response)
    {
        return new GetImageRequestResult
        {
            IsSuccess = true,
            Value = response
        };
    }

    public static implicit operator GetImageRequestResult(Err error)
    {
        return new GetImageRequestResult
        {
            IsSuccess = false,
            ErrorMessage = error.ErrorMessage
        };
    }
}