using Indecine.Models;
using Microsoft.AspNetCore.SignalR;

namespace Indecine.Hubs;

public class MovieHub : Hub
{
    public async Task JoinRoom(UserConnection connection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.Room);

        await Clients
            .Group(connection.Room)
            .SendAsync("JoinRoom", $"{connection.Username} has joined");
    }
}
