using Microsoft.EntityFrameworkCore;
using Numbridle.Data;
using Numbridle.Models;

namespace Numbridle.Services;

public class GameService : IGameService
{
    private readonly GameDbContext _context;

    public GameService(GameDbContext context)
    {
        _context = context;
    }

    public async Task<List<Player>> GetLeaderboardAsync()
    {
        return await _context.Players
            .AsNoTracking()
            .OrderByDescending(p => p.TotalScore)
            .Take(10)
            .ToListAsync();
    }

    public async Task<List<NumberDiscovery>> GetDiscoveriesAsync(int playerId)
    {
        return await _context.NumberDiscoveries
            .AsNoTracking()
            .Include(d => d.NumberMeaning)
            .Where(d => d.PlayerId == playerId)
            .OrderByDescending(d => d.DiscoveredAtUtc)
            .ToListAsync();
    }

    public async Task<GuessResult> RollAsync(int playerId)
    {
        var number = Random.Shared.Next(0, 1_000_000);
        var combination = number.ToString("D6");

        return await GuessAsync(playerId, combination);
    }

    public async Task<GuessResult> GuessAsync(
        int playerId,
        string combination)
    {
        if (combination.Length != 6 ||
            !combination.All(char.IsDigit))
        {
            throw new ArgumentException(
                "Combination must contain exactly six digits.",
                nameof(combination));
        }

        var player = await _context.Players
            .FirstOrDefaultAsync(p => p.Id == playerId);

        if (player == null)
        {
            throw new InvalidOperationException(
                $"Player with ID {playerId} was not found.");
        }

        var meaning = await _context.NumberMeanings
            .FirstOrDefaultAsync(m => m.Combination == combination);

        if (meaning == null)
        {
            meaning = CreateMeaning(combination);

            _context.NumberMeanings.Add(meaning);
            await _context.SaveChangesAsync();
        }

        var existingDiscovery = await _context.NumberDiscoveries
            .AnyAsync(d =>
                d.PlayerId == playerId &&
                d.NumberMeaningId == meaning.Id);

        long awardedPoints = existingDiscovery ? 0 : meaning.Points;

        if (!existingDiscovery)
        {
            _context.NumberDiscoveries.Add(new NumberDiscovery
            {
                PlayerId = playerId,
                NumberMeaningId = meaning.Id,
                DiscoveredAtUtc = DateTime.UtcNow,
                PointsAwarded = awardedPoints
            });

            player.TotalScore += awardedPoints;
            await _context.SaveChangesAsync();
        }

        return new GuessResult
        {
            Combination = combination,
            Name = meaning.Name,
            Description = meaning.Description,
            Rarity = meaning.Rarity,
            PointsAwarded = awardedPoints,
            TotalScore = player.TotalScore,
            ZeroTier = meaning.ZeroTier,
            IsNewDiscovery = !existingDiscovery
        };
    }

    private static NumberMeaning CreateMeaning(string combination)
    {
        var zeroTier = combination.TakeWhile(c => c == '0').Count();

        var (name, description, basePoints, rarity) =
            GetNumberMeaning(combination);

        long points = basePoints;

        if (zeroTier > 0)
        {
            points *= (long)Math.Pow(10, zeroTier);
        }

        if (combination == "000001")
        {
            name = "The Absolute Zero";
            description = "The rarest discovery. One is the final digit after five zeros.";
            points = 1_000_000_000;
            rarity = "Absolute";
        }

        return new NumberMeaning
        {
            Combination = combination,
            Name = name,
            Description = description,
            Rarity = rarity,
            Points = points,
            ZeroTier = zeroTier
        };
    }

    private static (
        string Name,
        string Description,
        long Points,
        string Rarity
    ) GetNumberMeaning(string combination)
    {
        return combination switch
        {
            "000666" => (
                "The Hidden Devil",
                "Six hundred sixty-six, concealed behind leading zeros.",
                666_666,
                "Mythic"),

            "000777" => (
                "Lucky Seven",
                "Three sevens bring extraordinary luck.",
                777_777,
                "Legendary"),

            "000999" => (
                "The Final Boss",
                "Three nines mark the ultimate challenge.",
                999_999,
                "Mythic"),

            "000001" => (
                "First Light",
                "The smallest positive six-digit combination.",
                100,
                "Rare"),

            "000000" => (
                "Absolute Nothing",
                "Six zeros. An extraordinary blank discovery.",
                1_000_000_000,
                "Absolute"),

            "000111" => (
                "The Beginning",
                "Three ones emerge from the darkness.",
                111_111,
                "Legendary"),

            _ => (
                $"Number {combination}",
                $"You discovered the combination {combination}. Every number has a place in Numbridle.",
                10,
                "Common")
        };
    }
}