using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day04 : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var input = rawInput
				.Where(entry => !string.IsNullOrWhiteSpace(entry))
				.ToArray();

			Part1Solution = SolvePart1(input).ToString();
			Part2Solution = SolvePart2(input).ToString();
		}

		private static string SolvePart1(string[] diagnosticReport)
		{
			return null;
		}

		private static string SolvePart2(string[] diagnosticReport)
		{
			return null;
		}
	}

	static class Day04Helpers
	{
		
	}
}
