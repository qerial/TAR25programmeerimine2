namespace Numbridle.Models;

public class NumberTraitResult
{
    public string Key { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Emoji { get; set; } = "🔢";

    public string Description { get; set; } = string.Empty;

    public long Points { get; set; }

    public string Rarity { get; set; } = "Common";

    public bool Earned { get; set; }

    public bool IsNewDiscovery { get; set; }
}