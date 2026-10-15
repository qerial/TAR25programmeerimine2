namespace Numbridle.Models;

public class NumberDiscovery
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public int NumberMeaningId { get; set; }
    public DateTime DiscoveredAtUtc { get; set; }
    public long PointsAwarded { get; set; }

    public Player Player { get; set; } = null!;
    public NumberMeaning NumberMeaning { get; set; } = null!;
}