using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day10Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var input = rawInput
				.Where(entry => !string.IsNullOrWhiteSpace(entry))
				.ToArray();

			Part1Solution = SolvePart1(input).ToString();
		}

		private static int SolvePart1(string[] navigationSystem)
		{
			var totalSyntaxErrorScore = navigationSystem
				.Select(line => line.GetSyntaxErrorScore())
				.Sum();

			return totalSyntaxErrorScore;
		}
	}

	public static class Day10Helpers
	{
		private static IEnumerable<char> OpeningChars = new List<char> { '(', '[', '{', '<' };

		private static IDictionary<char, char> ClosingCharForOpeningChar = new Dictionary<char, char>
		{
			['('] = ')',
			['['] = ']',
			['{'] = '}',
			['<'] = '>',
		};
		
		private static IDictionary<char, int> ValueOfCorruptedEndingChar = new Dictionary<char, int>
		{
			[')'] = 3,
			[']'] = 57,
			['}'] = 1197,
			['>'] = 25137,
		};

		public static int GetSyntaxErrorScore(this string input)
		{
			var openingChars = new List<char>();

			foreach (var ch in input)
			{
				if (OpeningChars.Contains(ch))
				{
					openingChars.Add(ch);
				}
				else if (ch == ClosingCharForOpeningChar[openingChars.Last()])
				{
					openingChars.RemoveAt(openingChars.Count() - 1);
				}
				else
				{
					return ValueOfCorruptedEndingChar[ch];
				}
			}

			return 0;
		}
	}
}
