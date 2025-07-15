using SharedKernel.Types;

namespace Contracts.Events;

public record ImageUploadedEvent(Guid ImageId, string ImageUrl)
{
    public record Response(Guid ImageId, string Text);
}

public record ImageUploadedEventResult : Res<ImageUploadedEvent.Response>
{
    public static implicit operator ImageUploadedEventResult(ImageUploadedEvent.Response response)
    {
        return new ImageUploadedEventResult
        {
            IsSuccess = true,
            Value = response
        };
    }

    public static implicit operator ImageUploadedEventResult(Err error)
    {
        return new ImageUploadedEventResult
        {
            IsSuccess = false,
            ErrorMessage = "An error occurred while retrieving images."
        };
    }
}