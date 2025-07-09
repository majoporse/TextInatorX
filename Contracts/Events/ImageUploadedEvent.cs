namespace Contracts.Events;

public record ImageUploadedEvent(Guid ImageId, string ImageUrl)
{
}

public record ImageUploadedEventResult(Guid ImageId, string Text);