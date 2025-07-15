using SharedKernel.Types;

namespace Contracts.Events;

public record GetImageTextRequest(Guid ImageId)
{
    public record Response(string Text);
}

public record GetImageTextRequestResult : Res<GetImageTextRequest.Response>
{
    public static implicit operator GetImageTextRequestResult(GetImageTextRequest.Response response)
    {
        return new GetImageTextRequestResult
        {
            IsSuccess = true,
            Value = response
        };
    }

    public static implicit operator GetImageTextRequestResult(Err error)
    {
        return new GetImageTextRequestResult
        {
            IsSuccess = false,
            ErrorMessage = "An error occurred while retrieving image text."
        };
    }
}