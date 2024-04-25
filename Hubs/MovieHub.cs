using Indecine.Data;
using Indecine.Models;
using Microsoft.AspNetCore.SignalR;

namespace Indecine.Hubs;

public class MovieHub(InMemoryDatabase database) : Hub
{
    private readonly InMemoryDatabase _database = database;

    public async Task JoinRoom(UserConnection connection)
    {
        Result result = _database.AddToRoom(connection);

        if (result.HasError)
        {
            await Clients
                .Group(connection.Room)
                .SendAsync("JoinRoomFailed", result.ErrorMessage);

            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, connection.Room);

        await Clients
            .Group(connection.Room)
            .SendAsync("JoinRoomSucceded", result.SuccessMessage);
    }

    public async Task AddToMovieIds(UserConnection connection, int movieId)
    {
        Result result = _database.AddToMovieIds(connection, movieId);

        if (result.HasError)
        {
            await Clients
                .Group(connection.Room)
                .SendAsync("AddToMovieIdsFailed", result.ErrorMessage);

            return;
        }

        Match match = _database.Matched(connection);

        if (match.Matched)
        {
            await Clients
                .Group(connection.Room)
                .SendAsync("Matched", match.MovieId);
        }

        await Clients
            .Group(connection.Room)
            .SendAsync("AddToMovieIdsSucceded", result.SuccessMessage);
    }
}
