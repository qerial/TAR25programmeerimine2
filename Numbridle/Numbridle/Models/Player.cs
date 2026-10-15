using Numbridle.Models;

public class Player
{
    public int Id { get; set; }

    public string Username { get; set; } = "Player";

    public long TotalScore { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<NumberDiscovery> Discoveries { get; set; }
        = new List<NumberDiscovery>();
}