namespace Numbridle.Models;

public class NumberMeaning
{
    public int Id { get; set; }

    public string Combination { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Rarity { get; set; } = "Common";

    public long Points { get; set; }

    public int ZeroTier { get; set; }

    public ICollection<NumberDiscovery> Discoveries { get; set; }
        = new List<NumberDiscovery>();
}