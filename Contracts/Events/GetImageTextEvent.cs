namespace Contracts.Events;

public record GetImageTextRequest(Guid ImageId);
public record GetImageTextRequestResult(string Text);
