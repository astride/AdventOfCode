﻿using Common.Interfaces;

namespace Year2021;

public class Day10Solver : IPuzzleSolver
{
	public string Title => "Syntax Scoring";
	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var navigationSystem = GetNavigationSystem(input);
		
		var totalSyntaxErrorScore = navigationSystem
			.Select(line => line.GetSyntaxErrorScore())
			.Sum();

		return totalSyntaxErrorScore;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var navigationSystem = GetNavigationSystem(input);
		
		var completionScores = navigationSystem
			.Where(line => !line.IsCorrupted())
			.Select(line => line.GetCompletionScore());

		return completionScores.ToList().GetMiddleScore();
	}

	private IEnumerable<string> GetNavigationSystem(string[] input)
	{
		return input
			.Where(entry => !string.IsNullOrWhiteSpace(entry))
			.ToArray();
	}
}

internal static class Day10Helpers
{
	private static readonly IEnumerable<char> OpeningChars = new List<char> { '(', '[', '{', '<' };

	private static readonly IDictionary<char, char> ClosingCharForOpeningChar = new Dictionary<char, char>
	{
		['('] = ')',
		['['] = ']',
		['{'] = '}',
		['<'] = '>',
	};
	
	private static readonly IDictionary<char, int> PointsForCorruptedEndingChar = new Dictionary<char, int>
	{
		[')'] = 3,
		[']'] = 57,
		['}'] = 1197,
		['>'] = 25137,
	};

	private static readonly IDictionary<char, int> PointsForClosingCharMatchingOpeningChar = new Dictionary<char, int>
	{
		['('] = 1,
		['['] = 2,
		['{'] = 3,
		['<'] = 4
	};

	public static bool IsCorrupted(this string input)
	{
		var corruptedChar = input.GetCorruptedEndingChar(out _);

		return corruptedChar.HasValue;
	}
	
	public static int GetSyntaxErrorScore(this string input)
	{
		var corruptedChar = input.GetCorruptedEndingChar(out _);

		if (corruptedChar == null)
		{
			return 0;
		}

		return PointsForCorruptedEndingChar[corruptedChar.Value];
	}

	public static double GetCompletionScore(this string input)
	{
		input.GetCorruptedEndingChar(out var openingCharsMissingClosingChar);
		openingCharsMissingClosingChar.Reverse();

		double completionScore = 0;

		foreach (var ch in openingCharsMissingClosingChar)
		{
			completionScore = 5 * completionScore + PointsForClosingCharMatchingOpeningChar[ch];
		}

		return completionScore;
	}

	private static char? GetCorruptedEndingChar(this string input, out List<char> openingCharsMissingClosingChar)
    {
		openingCharsMissingClosingChar = new List<char>();

		foreach (var ch in input)
		{
			if (OpeningChars.Contains(ch))
			{
				openingCharsMissingClosingChar.Add(ch);
			}
			else if (ch == ClosingCharForOpeningChar[openingCharsMissingClosingChar.Last()])
			{
				openingCharsMissingClosingChar.RemoveAt(openingCharsMissingClosingChar.Count - 1);
			}
			else
			{
				return ch;
			}
		}

		return null;
	}

	public static double GetMiddleScore(this IReadOnlyCollection<double> scores)
	{
		var middleIndex = (scores.Count - 1) / 2;

		return scores.OrderBy(_ => _).ToList()[middleIndex];
	}
}
