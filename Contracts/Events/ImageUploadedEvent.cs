namespace Contracts.Events;

public record ImageUploadedEvent(Guid ImageId, string ImageUrl)
{
    public record Result(Guid ImageId, string Text);
}