using Indecine.Models;
using System.Collections.Concurrent;

namespace Indecine.Data;

public class InMemoryDatabase
{
    public ConcurrentDictionary<string, List<Participant>> Rooms { get; private set; } = new();

    public Result AddToRoom(UserConnection connection)
    {
        if (!Rooms.TryGetValue(connection.Room, out List<Participant>? participants))
        {
            participants = [];
            Rooms[connection.Room] = participants;
        }

        int participantsLimit = 2;

        if (participants.Count >= participantsLimit)
        {
            return new Result().Fail("Participant limit exceeded");
        }

        if (!participants.Any(p => p.Username == connection.Username))
        {
            var participant = new Participant
            {
                Username = connection.Username,
                MovieIds = []
            };
            participants.Add(participant);
        }

        return new Result().Succeded($"{connection.Username} has joined the room {connection.Room}");
    }

    public Result AddToMovieIds(UserConnection connection, int movieId)
    {
        if (Rooms.TryGetValue(connection.Room, out List<Participant>? participants))
        {
            Participant? participant = participants.Find(p => p.Username == connection.Username);

            if (participant == null)
            {
                return new Result().Fail($"Participant {connection.Username} not found");
            }

            if (participant.MovieIds.Any(mId => mId == movieId))
            {
                return new Result().Fail($"Movie {movieId} already exists");
            }

            participant.MovieIds.Add(movieId);

            return new Result().Succeded($"Movie {movieId} added successfully");
        }

        return new Result().Fail($"Room {connection.Room} not found");
    }

    public Match Matched(UserConnection connection)
    {
        Match match = new();

        if (Rooms.TryGetValue(connection.Room, out List<Participant>? participants))
        {
            Participant firstParticipant = participants.First();
            Participant lastParticipant = participants.Last();

            foreach (int id in firstParticipant.MovieIds)
            {
                if (lastParticipant.MovieIds.Any(lId => lId == id))
                {
                    match.Matched = true;
                    match.MovieId = id;
                    break;
                }
            }
        }

        return match;
    }
}
