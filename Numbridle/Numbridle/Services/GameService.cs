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

    public async Task<List<NumberDiscovery>> GetDiscoveriesAsync(
        int playerId)
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
        string combination =
            Random.Shared.Next(0, 1_000_000).ToString("D6");

        return await GuessAsync(playerId, combination);
    }

    public async Task<GuessResult> GuessAsync(
        int playerId,
        string combination)
    {
        if (string.IsNullOrEmpty(combination) ||
            combination.Length != 6 ||
            !combination.All(c => c >= '0' && c <= '9'))
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

        bool alreadyDiscovered =
            await _context.NumberDiscoveries.AnyAsync(d =>
                d.PlayerId == playerId &&
                d.NumberMeaningId == meaning.Id);

        long awardedPoints = alreadyDiscovered ? 0 : meaning.Points;

        if (!alreadyDiscovered)
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
            IsNewDiscovery = !alreadyDiscovered
        };
    }

    private static NumberMeaning CreateMeaning(string combination)
    {
        int zeroTier = combination
            .TakeWhile(c => c == '0')
            .Count();

        var (name, description, basePoints, rarity) =
            GetNumberMeaning(combination);

        long points = basePoints;

        if (zeroTier > 0)
        {
            points *= (long)Math.Pow(10, zeroTier);
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
        // Exact six-digit combinations.
        switch (combination)
        {
            case "000000":
                return (
                    "THE VOID",
                    "Six zeros. Absolutely nothing. Somehow, everything.",
                    1_000_000_000,
                    "Absolute");

            case "000001":
                return (
                    "THE ABSOLUTE ZERO",
                    "The rarest beginning. One appears after five zeros.",
                    1_000_000_000,
                    "Absolute");

            case "000067":
                return (
                    "SIX SEVEN 🗣️",
                    "SIX SEVEN! The legendary internet meme.",
                    20_000,
                    "Mythic");

            case "000069":
                return (
                    "NICE.",
                    "Six nine. You know exactly why.",
                    15_000,
                    "Mythic");

            case "000042":
                return (
                    "THE ANSWER",
                    "42: the answer to life, the universe, and everything.",
                    5_000,
                    "Legendary");

            case "000420":
                return (
                    "SMOKE WEED EVERY DAY",
                    "420. A legendary internet culture reference.",
                    25_000,
                    "Mythic");

            case "000666":
                return (
                    "THE DEVIL",
                    "666. The number of the beast.",
                    30_000,
                    "Mythic");

            case "000777":
                return (
                    "JACKPOT!",
                    "Triple sevens. Maximum luck.",
                    30_000,
                    "Mythic");

            case "000999":
                return (
                    "FINAL BOSS",
                    "Triple nine. Maximum final-boss energy.",
                    10_000,
                    "Legendary");

            case "001337":
                return (
                    "ELITE GAMER",
                    "1337. Leetspeak for elite.",
                    20_000,
                    "Mythic");

            case "000123":
                return (
                    "COUNTING IS HARD",
                    "One, two, three. The tutorial level.",
                    2_500,
                    "Epic");

            case "000321":
                return (
                    "COUNTDOWN!",
                    "Three, two, one. Prepare for launch.",
                    2_500,
                    "Epic");

            case "123456":
                return (
                    "THE CLASSIC",
                    "The famous sequence everyone knows.",
                    10_000,
                    "Legendary");

            case "654321":
                return (
                    "MISSION IMPOSSIBLE",
                    "Six, five, four, three, two, one.",
                    10_000,
                    "Legendary");

            case "123321":
                return (
                    "THE NUMERICAL MIRROR",
                    "Reads the same forwards and backwards.",
                    10_000,
                    "Legendary");

            case "112233":
                return (
                    "TRIPLE DOUBLE",
                    "11, 22, 33. Three satisfying pairs.",
                    7_500,
                    "Epic");

            case "420420":
                return (
                    "BLAZE TWICE",
                    "420 appears twice.",
                    120_000,
                    "Mythic");

            case "696969":
                return (
                    "NICE NICE NICE",
                    "Six nines. Internet culture perfected.",
                    150_000,
                    "Mythic");

            case "777777":
                return (
                    "SEVEN SEVEN SEVEN",
                    "Six sevens. The casino fears you.",
                    500_000,
                    "Mythic");

            case "111111":
                return (
                    "ONE BRAIN CELL",
                    "Six ones. One thought, six copies.",
                    50_000,
                    "Mythic");

            case "999999":
                return (
                    "THE FINAL NUMBER",
                    "Six nines. The counter reaches its final form.",
                    100_000,
                    "Mythic");
        }

        // Detect patterns in every other combination.
        var traits = new List<string>();
        long bonus = 0;

        if (combination.Contains("67"))
        {
            traits.Add("Six Seven");
            bonus += 2_000;
        }

        if (combination.Contains("69"))
        {
            traits.Add("Nice");
            bonus += 1_500;
        }

        if (combination.Contains("420"))
        {
            traits.Add("Blaze It");
            bonus += 2_500;
        }

        if (combination.Contains("666"))
        {
            traits.Add("The Devil");
            bonus += 3_000;
        }

        if (combination.Contains("777"))
        {
            traits.Add("Jackpot");
            bonus += 3_000;
        }

        if (combination.Contains("1337"))
        {
            traits.Add("Elite Gamer");
            bonus += 2_000;
        }

        if (combination.Contains("404"))
        {
            traits.Add("Brain Not Found");
            bonus += 500;
        }

        if (combination.Contains("80085"))
        {
            traits.Add("Calculator Comedy");
            bonus += 3_000;
        }

        int[] counts = combination
            .GroupBy(c => c)
            .Select(g => g.Count())
            .OrderByDescending(n => n)
            .ToArray();

        int uniqueDigits = combination.Distinct().Count();
        int maxRepeat = counts[0];

        if (maxRepeat == 6)
        {
            traits.Add("Six of a Kind");
            bonus += 50_000;
        }
        else if (maxRepeat == 5)
        {
            traits.Add("Five of a Kind");
            bonus += 10_000;
        }
        else if (maxRepeat == 4)
        {
            traits.Add("Four of a Kind");
            bonus += 3_000;
        }
        else if (maxRepeat == 3)
        {
            traits.Add("Three of a Kind");
            bonus += 724;
        }

        if (combination.SequenceEqual(combination.Reverse()))
        {
            traits.Add("Palindrome");
            bonus += 1_000;
        }

        if (uniqueDigits == 6)
        {
            traits.Add("No Repeats");
            bonus += 500;
        }

        if (uniqueDigits == 4)
        {
            traits.Add("Quartet");
            bonus += 290;
        }

        if (combination[0] == combination[5])
        {
            traits.Add("Equilibrium");
            bonus += 1_000;
        }

        if (!combination.Contains('0'))
        {
            traits.Add("Voidless");
            bonus += 167;
        }

        if ((combination[5] - '0') % 2 == 1)
        {
            traits.Add("Odd");
            bonus += 200;
        }

        if (combination.Contains('7'))
        {
            traits.Add("Lucky Seven");
            bonus += 213;
        }

        if (combination.Length == 6)
        {
            traits.Add("Six Digits");
            bonus += 111;
        }

        // Find any sequence of three consecutive digits.
        bool hasSequence = false;

        for (int i = 0; i <= combination.Length - 3; i++)
        {
            int a = combination[i] - '0';
            int b = combination[i + 1] - '0';
            int c = combination[i + 2] - '0';

            if ((b == a + 1 && c == b + 1) ||
                (b == a - 1 && c == b - 1))
            {
                hasSequence = true;
                break;
            }
        }

        if (hasSequence)
        {
            traits.Add("Sequence (3)");
            bonus += 1_716;
        }

        // Digits separated by one position.
        if (combination[0] == combination[2] ||
            combination[1] == combination[3] ||
            combination[2] == combination[4] ||
            combination[3] == combination[5])
        {
            traits.Add("Hopscotch");
            bonus += 312;
        }

        // Exactly one occurrence of a digit.
        foreach (char digit in combination.Distinct())
        {
            if (combination.Count(c => c == digit) == 1)
            {
                bonus += 50;
            }
        }

        int zeroTier = combination
            .TakeWhile(c => c == '0')
            .Count();

        long points = 10 + bonus;

        if (zeroTier > 0)
        {
            points *= (long)Math.Pow(10, zeroTier);
        }

        string name = traits.Count > 0
            ? traits.OrderByDescending(t => t.Length).First()
            : $"Number {combination}";

        string description = traits.Count > 0
            ? $"Discovered traits: {string.Join(", ", traits)}."
            : $"You discovered {combination}. Every number has a story.";

        string rarity = points switch
        {
            >= 100_000 => "Mythic",
            >= 15_000 => "Legendary",
            >= 5_000 => "Epic",
            >= 2_000 => "Rare",
            >= 200 => "Uncommon",
            _ => "Common"
        };

        return (name, description, points, rarity);
    }
}