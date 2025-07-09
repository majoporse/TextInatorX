using Microsoft.AspNetCore.SignalR;

namespace Presentation.Mvc.Hubs;

public class ImageUploadHub : Hub
{
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }
}