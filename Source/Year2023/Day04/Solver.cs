using System.Text.RegularExpressions;
using Common.Interfaces;

namespace Year2023;

public class Day04Solver : IPuzzleSolver
{
	public string Title => "Scratchcards";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private const char NumberSeparator = '|';

	private static readonly Regex NumberRegex = new(@"\d*");
	private static readonly Regex CardIntroRegex = new(@"(Card )\d*: ");

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var points = 0;

		foreach (var cardInformation in input)
		{
			var numberInformation = CardIntroRegex.Replace(cardInformation, string.Empty);

			var splitNumbers = numberInformation.Split(NumberSeparator, StringSplitOptions.TrimEntries);

			var winningNumbers = NumberRegex.Matches(splitNumbers[0])
				.Select(match => match.ToString())
				.Where(number => !string.IsNullOrEmpty(number))
				.ToHashSet();

			var actualMatches = NumberRegex.Matches(splitNumbers[1])
				.Select(match => match.ToString())
				.Count(winningNumbers.Contains);

			if (actualMatches > 0)
			{
				points += (int)Math.Pow(2, actualMatches - 1);
			}
		}

		return points;
		// 26674 -- Wrong
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}
}