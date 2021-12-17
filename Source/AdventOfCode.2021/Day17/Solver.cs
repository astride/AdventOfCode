using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day17Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var input = rawInput
				.Single(entry => !string.IsNullOrWhiteSpace(entry))
				.Substring("target area: ".Length)
				.Split(',')
				.Select(entry => entry.Trim());

			var xTargetRange = input
				.Single(entry => entry.Contains('x'));

			var yTargetRange = input
				.Single(entry => entry.Contains('y'));

			(int Min, int Max) xTarget = (xTargetRange.GetMinTargetValue(), xTargetRange.GetMaxTargetValue());
			(int Min, int Max) yTarget = (yTargetRange.GetMinTargetValue(), yTargetRange.GetMaxTargetValue());

			Part1Solution = SolvePart1(xTarget, yTarget).ToString();
		}

		private static int SolvePart1((int Min, int Max) xTarget, (int Min, int Max) yTarget)
		{
			return -1;
		}
	}

	public static class Day17Helpers
	{
		public static int GetMinTargetValue(this string targetRange)
		{
			return int.Parse(string.Concat(targetRange.Skip(2).TakeWhile(ch => ch != '.')));
		}

		public static int GetMaxTargetValue(this string targetRange)
		{
			return int.Parse(string.Concat(targetRange.SkipWhile(ch => ch != '.').SkipWhile(ch => ch == '.')));
		}
	}
}
