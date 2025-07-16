using SharedKernel.Types;

namespace Contracts.Events;

public record AddImageRequest(byte[] file, string name)
{
    public record Response(ImageDto ImageDto, string ImageUrl);
}

public record AddImageRequestResult : Res<AddImageRequest.Response>
{
    public static implicit operator AddImageRequestResult(AddImageRequest.Response response)
    {
        return new AddImageRequestResult
        {
            IsSuccess = true,
            Value = response
        };
    }

    public static implicit operator AddImageRequestResult(Err error)
    {
        return new AddImageRequestResult
        {
            IsSuccess = false,
            ErrorMessage = error.ErrorMessage
        };
    }
}