namespace Numbridle.Models;

public class PlayerTraitDiscovery
{
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public string TraitKey { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Emoji { get; set; } = "🔢";

    public string Description { get; set; } = string.Empty;

    public string Rarity { get; set; } = "Common";

    public long PointsAwarded { get; set; }

    public DateTime DiscoveredAtUtc { get; set; }

    public Player Player { get; set; } = null!;
}