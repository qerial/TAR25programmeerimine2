
using Numbridle.Models;

namespace Numbridle.Services;

public static class NumberMeaningGenerator
{
    private sealed record Trait(
        string Name,
        string Description,
        int Points,
        int Priority = 0);

    private static readonly Dictionary<string, Trait> SpecialNumbers = new()
    {
        ["000000"] = new("THE VOID",
            "Six zeros. Absolutely nothing. Somehow, everything.",
            100_000, 100),

        ["000067"] = new("SIX SEVEN 🗣️",
            "SIX SEVEN! The legendary 6-7 internet meme. WHAT DOES IT MEAN?!",
            2_000, 80),

        ["000069"] = new("NICE.",
            "69. You know exactly why this number is funny. Nice.",
            1_500, 80),

        ["000042"] = new("THE ANSWER",
            "42. The answer to life, the universe, and everything.",
            500, 50),

        ["000420"] = new("SMOKE WEED EVERY DAY",
            "420 blaze it. A legendary internet and cannabis culture reference.",
            2_500, 80),

        ["000666"] = new("THE DEVIL",
            "666. The number of the beast. Absolutely diabolical.",
            3_000, 70),

        ["000777"] = new("JACKPOT!",
            "777. Three sevens. You just hit the imaginary casino jackpot.",
            3_000, 70),

        ["000999"] = new("FINAL BOSS",
            "Triple nine. Maximum digits. Final boss energy.",
            500, 50),

        ["000911"] = new("CALL 911",
            "911. This combination is an emergency.",
            300, 40),

        ["001337"] = new("ELITE GAMER",
            "1337. Leetspeak. You are officially elite.",
            2_000, 70),

        ["000800"] = new("EIGHT HUNDRED",
            "800. A round number with big-number energy.",
            100, 20),

        ["000101"] = new("THERE ARE ONLY TWO TYPES",
            "There are 10 types of people: those who understand binary and those who don't.",
            250, 30),

        ["000123"] = new("COUNTING IS HARD",
            "1, 2, 3. The tutorial level of numbers.",
            250, 30),

        ["000321"] = new("COUNTDOWN!",
            "Three, two, one. We are SO back.",
            250, 30),

        ["000100"] = new("ONE HUNDRED",
            "A perfect score. A century. A solid 100.",
            100, 20),

        ["000420"] = new("SMOKE WEED EVERY DAY",
            "420 blaze it. A legendary internet and cannabis culture reference.",
            2_500, 80),

        ["069420"] = new("NICE BLAZE",
            "69 AND 420 IN ONE NUMBER. NICE. BLAZE IT.",
            10_000, 100),

        ["042069"] = new("THE FUNNY COMBO",
            "420 and 69 together. Internet humor has peaked.",
            10_000, 100),

        ["696969"] = new("NICE NICE NICE",
            "Six nine digits. This is the internet in numerical form.",
            15_000, 100),

        ["420420"] = new("BLAZE TWICE",
            "420 twice. Smoke weed every day, apparently.",
            12_000, 100),

        ["777777"] = new("SEVEN SEVEN SEVEN",
            "SIX SEVENS. MAXIMUM LUCK. THE CASINO FEARS YOU.",
            50_000, 110),

        ["111111"] = new("ONE BRAIN CELL",
            "Six ones. There is exactly one thought bouncing around in here.",
            5_000, 80),

        ["999999"] = new("THE FINAL NUMBER",
            "Six nines. The counter has reached its final form.",
            10_000, 90),

        ["123456"] = new("THE CLASSIC",
            "123456. The password everyone tells you not to use.",
            1_000, 60),

        ["654321"] = new("MISSION IMPOSSIBLE",
            "Six, five, four, three, two, one. Prepare for launch.",
            1_000, 60),

        ["123321"] = new("THE NUMERICAL MIRROR",
            "123321. Reads the same in reverse. Symmetry goes hard.",
            1_000, 60),

        ["112233"] = new("TRIPLE DOUBLE",
            "11, 22, 33. Three pairs. Very satisfying.",
            750, 50),

        ["001337"] = new("ELITE GAMER",
            "1337. Leetspeak. You are officially elite.",
            2_000, 70)
    };

    public static NumberMeaning Generate(string combination)
    {
        if (combination is null ||
            combination.Length != 6 ||
            combination.Any(c => c < '0' || c > '9'))
        {
            throw new ArgumentException(
                "Combination must contain exactly six digits.",
                nameof(combination));
        }

        var traits = new List<Trait>
        {
            new(
                "JUST A NUMBER",
                "Every number has value. Yours is waiting to be understood.",
                10,
                0)
        };

        // Exact combinations get unique names and bonuses.
        if (SpecialNumbers.TryGetValue(combination, out var exact))
        {
            traits.Add(exact);
        }

        // Meme references. These can stack with other traits.
        AddIfContains(
            combination, "67",
            "SIX SEVEN 🗣️",
            "Six seven! The viral 6-7 meme. SIX SEVEN!",
            2_000, traits);

        AddIfContains(
            combination, "69",
            "NICE.",
            "69. The internet has decided this is funny. Nice.",
            1_500, traits);

        AddIfContains(
            combination, "420",
            "SMOKE WEED EVERY DAY",
            "420 blaze it. A legendary internet culture reference.",
            2_500, traits);

        AddIfContains(
            combination, "666",
            "THE DEVIL",
            "666. The number of the beast.",
            3_000, traits);

        AddIfContains(
            combination, "777",
            "JACKPOT!",
            "Triple sevens. The legendary lucky combination.",
            3_000, traits);

        AddIfContains(
            combination, "1337",
            "ELITE GAMER",
            "1337. Leetspeak for elite.",
            2_000, traits);

        AddIfContains(
            combination, "80085",
            "BOOBS",
            "Turn a calculator upside down. You know what it says.",
            3_000, traits);

        AddIfContains(
            combination, "404",
            "BRAIN NOT FOUND",
            "Error 404: brain not found.",
            500, traits);

        AddIfContains(
            combination, "911",
            "CALL 911",
            "This number is an emergency.",
            300, traits);

        // Count every digit, even when they aren't next to each other.
        var counts = combination
            .GroupBy(c => c)
            .Select(g => g.Count())
            .OrderByDescending(n => n)
            .ToArray();

        int mostRepeated = counts[0];

        switch (mostRepeated)
        {
            case 6:
                traits.Add(new Trait(
                    "SIX OF A KIND",
                    "ALL SIX DIGITS ARE IDENTICAL. ABSOLUTE UNIT.",
                    50_000, 110));
                break;

            case 5:
                traits.Add(new Trait(
                    "FIVE OF A KIND",
                    "Five identical digits in one combination. That's ridiculous.",
                    10_000, 90));
                break;

            case 4:
                traits.Add(new Trait(
                    "FOUR OF A KIND",
                    "Four matching digits. This combination means business.",
                    3_000, 70));
                break;

            case 3:
                traits.Add(new Trait(
                    "TRIPLE THREAT",
                    "Three matching digits! That's a seriously satisfying pattern.",
                    500, 40));
                break;
        }

        // Consecutive repetitions get an additional bonus.
        int longestRun = 1;
        int currentRun = 1;

        for (int i = 1; i < combination.Length; i++)
        {
            if (combination[i] == combination[i - 1])
            {
                currentRun++;
                longestRun = Math.Max(longestRun, currentRun);
            }
            else
            {
                currentRun = 1;
            }
        }

        if (longestRun >= 6)
        {
            traits.Add(new Trait(
                "UNBROKEN",
                "Six identical digits in a row. Perfection.",
                10_000, 90));
        }
        else if (longestRun == 5)
        {
            traits.Add(new Trait(
                "FIVE IN A ROW",
                "Five identical digits consecutively. Wild.",
                3_000, 70));
        }
        else if (longestRun == 4)
        {
            traits.Add(new Trait(
                "QUADRUPLE STREAK",
                "Four identical digits next to each other.",
                1_000, 50));
        }
        else if (longestRun == 3)
        {
            traits.Add(new Trait(
                "TRIPLE STREAK",
                "Three identical digits in a row.",
                250, 30));
        }

        // Other satisfying visual patterns.
        if (combination.SequenceEqual(combination.Reverse()))
        {
            traits.Add(new Trait(
                "PALINDROME",
                "Reads the same forwards and backwards.",
                500, 40));
        }

        if (combination.Distinct().Count() == 6)
        {
            traits.Add(new Trait(
                "NO REPEATS",
                "Every single digit is different.",
                100, 20));
        }

        if (IsAscending(combination))
        {
            traits.Add(new Trait(
                "ASCENDING",
                "123456 vibes. Every digit climbs.",
                1_000, 50));
        }
        else if (IsDescending(combination))
        {
            traits.Add(new Trait(
                "DESCENDING",
                "654321 vibes. The countdown is real.",
                1_000, 50));
        }

        // Base score plus every discovered trait.
        int points = traits.Sum(t => t.Points);

        // Highest-priority trait becomes the card's main identity.
        var primary = traits
            .OrderByDescending(t => t.Priority)
            .ThenByDescending(t => t.Points)
            .First();

        // Keep descriptions distinct so overlapping rules don't
        // repeat the same explanation.
        string description = string.Join(
            " ",
            traits
                .Select(t => t.Description)
                .Distinct());

        string rarity = points switch
        {
            >= 50_000 => "Transcendent",
            >= 15_000 => "Mythic",
            >= 5_000 => "Legendary",
            >= 2_000 => "Epic",
            >= 750 => "Rare",
            >= 200 => "Uncommon",
            _ => "Common"
        };

        int zeroTier = combination
            .TakeWhile(c => c == '0')
            .Count();

        return new NumberMeaning
        {
            Combination = combination,
            Name = primary.Name,
            Description = description,
            Rarity = rarity,
            Points = points,
            ZeroTier = zeroTier
        };
    }

    private static void AddIfContains(
        string combination,
        string pattern,
        string name,
        string description,
        int points,
        List<Trait> traits)
    {
        if (combination.Contains(pattern, StringComparison.Ordinal))
        {
            traits.Add(new Trait(
                name,
                description,
                points,
                60));
        }
    }

    private static bool IsAscending(string value)
    {
        for (int i = 1; i < value.Length; i++)
        {
            if (value[i] - value[i - 1] != 1)
                return false;
        }

        return true;
    }

    private static bool IsDescending(string value)
    {
        for (int i = 1; i < value.Length; i++)
        {
            if (value[i - 1] - value[i] != 1)
                return false;
        }

        return true;
    }
}