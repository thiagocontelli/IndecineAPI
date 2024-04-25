namespace Indecine.Models;

public class Participant
{
    public string Username { get; set; } = string.Empty;
    public List<int> MovieIds { get; set; } = [];
}
