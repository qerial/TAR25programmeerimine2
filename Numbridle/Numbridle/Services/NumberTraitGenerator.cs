
using Numbridle.Models;

namespace Numbridle.Services;

public static class NumberTraitGenerator
{
    private static NumberTraitResult Trait(
        string key,
        string name,
        string emoji,
        string description,
        long points)
    {
        string rarity = points switch
        {
            >= 50000 => "Mythic",
            >= 10000 => "Legendary",
            >= 3000 => "Epic",
            >= 1000 => "Rare",
            >= 200 => "Uncommon",
            _ => "Common"
        };

        return new NumberTraitResult
        {
            Key = key,
            Name = name,
            Emoji = emoji,
            Description = description,
            Points = points,
            Rarity = rarity
        };
    }

    public static List<NumberTraitResult> Generate(string value)
    {
        if (value.Length != 6 || value.Any(c => c < '0' || c > '9'))
            throw new ArgumentException("Expected exactly six digits.", nameof(value));

        var result = new List<NumberTraitResult>();

        void AddIf(bool condition, NumberTraitResult trait)
        {
            if (condition)
                result.Add(trait);
        }

        var groups = value.GroupBy(c => c)
            .Select(g => new { Digit = g.Key, Count = g.Count() })
            .ToList();

        var counts = groups.Select(g => g.Count)
            .OrderByDescending(n => n).ToArray();

        int unique = groups.Count;
        int pairs = counts.Count(n => n == 2);
        int maxCount = counts.Max();
        int zeros = value.Count(c => c == '0');
        int sum = value.Sum(c => c - '0');

        AddIf(true, Trait(
            "six-digits", "Six Digits", "🐝",
            "A complete six-digit combination.", 111));

        AddIf(value.Any(c => c == '7'), Trait(
            "lucky-seven", "Lucky Seven", "7️⃣",
            "Contains the number 7.", 213));

        AddIf(value.Any(c => c == '9'), Trait(
            "fluorine", "Fluorine (9)", "🦷",
            "Contains the digit 9.", 100));

        AddIf(value.Any(c => c == '8'), Trait(
            "oxygen", "Oxygen (8)", "💨",
            "Contains the digit 8.", 100));

        AddIf(value.Any(c => c == '3'), Trait(
            "lithium", "Lithium (3)", "🔋",
            "Contains the digit 3.", 100));

        AddIf(value.All(c => (c - '0') % 2 == 1), Trait(
            "all-odd", "Odd Squad", "🦄",
            "Every digit is odd.", 200));

        AddIf(value.All(c => (c - '0') % 2 == 0), Trait(
            "all-even", "Even Steven", "🧊",
            "Every digit is even.", 200));

        AddIf(zeros == 0, Trait(
            "void", "Void", "🕳️",
            "Contains no zeros.", 167));

        AddIf(zeros == 1, Trait(
            "one-zero", "Lone Zero", "⭕",
            "Contains exactly one zero.", 150));

        AddIf(zeros >= 3, Trait(
            "zero-gravity", "Zero Gravity", "🌌",
            "At least half the digits are zero.", 500));

        AddIf(unique == 6, Trait(
            "no-repeats", "No Repeats", "🦋",
            "Every digit is different.", 250));

        AddIf(unique == 5, Trait(
            "five-unique", "Five Unique", "🖐️",
            "Contains exactly five different digits.", 180));

        AddIf(unique == 4, Trait(
            "quartet", "Quartet", "🎻",
            "Uses exactly four different digits.", 290));

        AddIf(unique == 3, Trait(
            "triple-colors", "Three's Company", "🎨",
            "Uses exactly three different digits.", 350));

        AddIf(unique == 2, Trait(
            "binary-soul", "Binary Soul", "🔀",
            "Uses exactly two different digits.", 750));

        AddIf(unique == 1, Trait(
            "monochrome", "Monochrome", "🧱",
            "All six digits are identical.", 50000));

        AddIf(pairs >= 1, Trait(
            "pair", "Pair", "👯",
            "Contains a pair of matching digits.", 120));

        AddIf(pairs >= 2, Trait(
            "double-pair", "Double Pair", "👯‍♂️",
            "Contains at least two pairs of matching digits.", 300));

        AddIf(pairs == 3, Trait(
            "triple-double", "Triple Double", "🥪",
            "Contains three pairs of matching digits.", 750));

        AddIf(maxCount >= 3, Trait(
            "three-kind", "Three of a Kind", "🎰",
            "Contains three or more identical digits.", 724));

        AddIf(maxCount >= 4, Trait(
            "four-kind", "Four of a Kind", "🃏",
            "At least four digits match.", 3000));

        AddIf(maxCount >= 5, Trait(
            "five-kind", "Five of a Kind", "🔥",
            "Five digits match.", 10000));

        AddIf(value[0] == value[5], Trait(
            "equilibrium", "Equilibrium", "🧘",
            "The first and last digits are identical.", 1000));

        AddIf(value.SequenceEqual(value.Reverse()), Trait(
            "palindrome", "Palindrome", "🪞",
            "Reads the same forwards and backwards.", 1000));

        AddIf(value[..3] == value[3..], Trait(
            "half-and-half", "Half and Half", "🍰",
            "The first three digits repeat in the last three.", 1500));

        AddIf(value[..2] == value.Substring(2, 2) &&
              value.Substring(2, 2) == value[4..], Trait(
            "triple-block", "Triple Block", "🧊",
            "The number consists of three identical two-digit blocks.", 3000));

        AddIf(value.Distinct().Count() == 1, Trait(
            "perfect-repeat", "Perfect Repeat", "♾️",
            "One digit fills every position.", 50000));

        AddIf(value.Contains("123") || value.Contains("234") ||
              value.Contains("345") || value.Contains("456") ||
              value.Contains("567") || value.Contains("678") ||
              value.Contains("789"), Trait(
            "sequence-three", "Sequence (3)", "🔢",
            "Contains three consecutive ascending digits.", 1716));

        AddIf(value.Contains("321") || value.Contains("432") ||
              value.Contains("543") || value.Contains("654") ||
              value.Contains("765") || value.Contains("876") ||
              value.Contains("987"), Trait(
            "countdown-three", "Countdown (3)", "📉",
            "Contains three consecutive descending digits.", 1000));

        AddIf(value.Any(c => value.Count(d => d == c) == 2), Trait(
            "matching-digits", "Twins", "👯",
            "At least one digit appears exactly twice.", 120));

        AddIf(Enumerable.Range(0, 4).Any(i =>
            value[i] == value[i + 2]), Trait(
            "hopscotch", "Hopscotch", "🦘",
            "A digit appears at positions two apart.", 312));

        AddIf(Enumerable.Range(0, 4).Any(i =>
            value[i] == value[i + 2] &&
            value[i + 1] != value[i]), Trait(
            "sandwich", "Sandwich", "🥪",
            "Matching digits surround a different digit.", 500));

        AddIf(sum == 21, Trait(
            "sum-21", "Blackjack", "🃏",
            "The digits add up to 21.", 721));

        AddIf(sum == 42, Trait(
            "sum-42", "The Answer", "🌌",
            "The digits add up to 42.", 420));

        AddIf(sum == 27, Trait(
            "sum-27", "Perfect Three", "✨",
            "The digits add up to 27.", 270));

        AddIf(value == new string(value[0], 6), Trait(
            "six-alike", "Six of a Kind", "👑",
            "All six digits are identical.", 50000));

        AddIf(IsAscending(value), Trait(
            "ascending", "Full Ascension", "📈",
            "Every digit increases by one.", 10000));

        AddIf(IsDescending(value), Trait(
            "descending", "Full Countdown", "📉",
            "Every digit decreases by one.", 10000));

        AddIf(value == "123456", Trait(
            "classic", "The Classic", "🔢",
            "The iconic sequence 123456.", 1000));

        AddIf(value == "654321", Trait(
            "mission-impossible", "Mission Impossible", "🕶️",
            "A perfect six-digit countdown.", 1000));

        AddIf(value.Contains("69"), Trait(
            "nice", "NICE.", "😏",
            "Contains the legendary 69 combination.", 1500));

        AddIf(value.Contains("420"), Trait(
            "four-twenty", "Four Twenty", "🌿",
            "Contains the internet-famous 420 combination.", 2500));

        AddIf(value.Contains("666"), Trait(
            "devil", "The Devil", "😈",
            "Contains three sixes in a row.", 3000));

        AddIf(value.Contains("777"), Trait(
            "jackpot", "Jackpot!", "🎰",
            "Three lucky sevens in a row.", 3000));

        AddIf(value.Contains("404"), Trait(
            "error-404", "Brain Not Found", "🧠",
            "Error 404: brain not found.", 500));

        AddIf(value.Contains("80085"), Trait(
            "calculator-classic", "Calculator Classic", "🙃",
            "A classic upside-down calculator joke.", 3000));

        return result
            .GroupBy(t => t.Key)
            .Select(g => g.First())
            .ToList();
    }

    private static bool IsAscending(string value)
    {
        for (int i = 1; i < value.Length; i++)
            if (value[i] - value[i - 1] != 1)
                return false;

        return true;
    }

    private static bool IsDescending(string value)
    {
        for (int i = 1; i < value.Length; i++)
            if (value[i - 1] - value[i] != 1)
                return false;

        return true;
    }
}
