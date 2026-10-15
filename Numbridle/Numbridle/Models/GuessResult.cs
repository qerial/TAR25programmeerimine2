namespace Numbridle.Models;

public class GuessResult
{
    public string Combination { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Rarity { get; set; } = "Common";

    public long PointsAwarded { get; set; }

    public long TotalScore { get; set; }

    public int ZeroTier { get; set; }

    public bool IsNewDiscovery { get; set; }
}