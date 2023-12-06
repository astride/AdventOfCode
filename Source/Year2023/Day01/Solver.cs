using System.Text.RegularExpressions;
using Common.Helpers;
using Common.Interfaces;

namespace Year2023;

public class Day01Solver : IPuzzleSolver
{
	public string Title => "Trebuchet?!";
	public bool UsePartSpecificExampleInputFiles => true;
	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private static readonly Dictionary<string, int> IntByWord = new()
	{
		["1"] = 1,
		["2"] = 2,
		["3"] = 3,
		["4"] = 4,
		["5"] = 5,
		["6"] = 6,
		["7"] = 7,
		["8"] = 8,
		["9"] = 9,
		["one"] = 1,
		["two"] = 2,
		["three"] = 3,
		["four"] = 4,
		["five"] = 5,
		["six"] = 6,
		["seven"] = 7,
		["eight"] = 8,
		["nine"] = 9,
	};
	
	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var sum = input
			.Sum(line => 10 * GetFirstDigit(line, RegexHelper.DigitRegex) + GetLastDigit(line, RegexHelper.DigitRegex));

		return sum;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var firstDigitRegex = new Regex(GetFirstDigitRegexPattern());
		var lastDigitRegex = new Regex(GetLastDigitRegexPattern());

		var sum = input.Sum(line => 10 * GetFirstDigit(line, firstDigitRegex) + GetLastDigit(line, lastDigitRegex));

		return sum;
	}

	private static int GetFirstDigit(string line, Regex regex)
	{
		var match = regex.Match(line).Value;

		return IntByWord[match];
	}

	private static int GetLastDigit(string line, Regex regex)
	{
		var reversedLine = string.Concat(line.Reverse());

		var reversedMatch = regex.Match(reversedLine).Value;

		var match = string.Concat(reversedMatch.Reverse());

		return IntByWord[match];
	}

	private static string GetFirstDigitRegexPattern()
	{
		return GetDigitRegexPattern(IntByWord.Keys);
	}

	private static string GetLastDigitRegexPattern()
	{
		var reversedSearchTerms = IntByWord.Keys.Select(key => string.Concat(key.Reverse()));

		return GetDigitRegexPattern(reversedSearchTerms);
	}

	private static string GetDigitRegexPattern(IEnumerable<string> searchTerms)
	{
		return "(" + string.Join("|", searchTerms) + ")";
	}
}