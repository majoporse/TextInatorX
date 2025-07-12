using Microsoft.AspNetCore.SignalR;

namespace Presentation.Mvc.Hubs;

public class ImageUploadHub : Hub
{
    public static readonly string RecieveImageDataEvent = "ReceiveImageTextData";

    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }
}