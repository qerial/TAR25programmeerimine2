using Microsoft.EntityFrameworkCore;
using Numbridle.Models;

namespace Numbridle.Data;

public static class SeedData
{
    public static async Task InitializeAsync(GameDbContext db)
    {
        if (!await db.Players.AnyAsync())
        {
            db.Players.Add(new Player
            {
                Id = 1,
                Username = "DemoPlayer",
                TotalScore = 0
            });
        }

        var meanings = new[]
        {
            new NumberMeaning
            {
                Combination = "000000",
                Name = "THE VOID",
                Description = "Absolute zero. No digits remain. The rarest discovery.",
                Rarity = "Transcendent",
                Points = 100_000_000,
                ZeroTier = 6
            },
            new NumberMeaning
            {
                Combination = "000001",
                Name = "First Light",
                Description = "A single spark emerging from nothing.",
                Rarity = "Mythic",
                Points = 5_000_000,
                ZeroTier = 5
            },
            new NumberMeaning
            {
                Combination = "000010",
                Name = "The Awakening",
                Description = "A hidden signal in the void.",
                Rarity = "Legendary",
                Points = 1_000_000,
                ZeroTier = 4
            },
            new NumberMeaning
            {
                Combination = "000100",
                Name = "Origin",
                Description = "The beginning of a new sequence.",
                Rarity = "Legendary",
                Points = 500_000,
                ZeroTier = 3
            },
            new NumberMeaning
            {
                Combination = "001000",
                Name = "Echo",
                Description = "A faint signal repeating through the void.",
                Rarity = "Epic",
                Points = 100_000,
                ZeroTier = 2
            },
            new NumberMeaning
            {
                Combination = "010000",
                Name = "Hidden Signal",
                Description = "A signal obscured by leading zeros.",
                Rarity = "Rare",
                Points = 25_000,
                ZeroTier = 1
            },
            new NumberMeaning
            {
                Combination = "000666",
                Name = "The Devil",
                Description = "A dark symbol with a legendary reputation.",
                Rarity = "Legendary",
                Points = 666_666
            },
            new NumberMeaning
            {
                Combination = "000777",
                Name = "Jackpot",
                Description = "Three sevens: a legendary lucky combination.",
                Rarity = "Legendary",
                Points = 777_777
            },
            new NumberMeaning
            {
                Combination = "000420",
                Name = "Four-Twenty",
                Description = "A familiar cultural reference.",
                Rarity = "Rare",
                Points = 42_000
            },
            new NumberMeaning
            {
                Combination = "000123",
                Name = "Ascending Sequence",
                Description = "Three consecutive ascending digits.",
                Rarity = "Uncommon",
                Points = 1_230
            },
            new NumberMeaning
            {
                Combination = "000111",
                Name = "Triple One",
                Description = "Three identical ones.",
                Rarity = "Uncommon",
                Points = 1_110
            },
            new NumberMeaning
            {
                Combination = "000999",
                Name = "The End",
                Description = "Three nines, marking a symbolic ending.",
                Rarity = "Epic",
                Points = 99_999
            }
        };

        foreach (var meaning in meanings)
        {
            var exists = await db.NumberMeanings.AnyAsync(
                x => x.Combination == meaning.Combination);

            if (!exists)
                db.NumberMeanings.Add(meaning);
        }

        await db.SaveChangesAsync();
    }
}