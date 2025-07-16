using SharedKernel.Types;

namespace Contracts.Events;

public record DeleteImageRequest(Guid imageId)
{
    public record Response(ImageDto ImageDto);
}

public record DeleteImageRequestResult : Res<DeleteImageRequest.Response>
{
    public static implicit operator DeleteImageRequestResult(DeleteImageRequest.Response response)
    {
        return new DeleteImageRequestResult
        {
            IsSuccess = true,
            Value = response
        };
    }

    public static implicit operator DeleteImageRequestResult(Err error)
    {
        return new DeleteImageRequestResult
        {
            IsSuccess = false,
            ErrorMessage = error.ErrorMessage
        };
    }
}