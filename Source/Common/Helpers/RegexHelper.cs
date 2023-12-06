using System.Text.RegularExpressions;

namespace Common.Helpers;

public static class RegexHelper
{
    public static readonly Regex DigitRegex = new(@"\d");

    private static readonly Regex NumberRegex = new(@"\d+");
    private static readonly Regex WordRegex = new(@"[a-z]+");

    public static IEnumerable<int> GetAllNumbersAsInt(string source)
    {
        return NumberRegex.Matches(source)
            .Select(match => int.Parse(match.ToString()));
    }

    public static IEnumerable<string> GetAllNumbersAsString(string source)
    {
        return NumberRegex.Matches(source)
            .Select(match => match.ToString());
    }

    public static string GetFirstWord(string source)
    {
        return WordRegex.Match(source).ToString();
    }

    public static int GetFirstNumberAsInt(string source)
    {
        return int.Parse(NumberRegex.Match(source).ToString());
    }
}
