using AdventOfCode.Common;
using AdventOfCode.Common.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day18Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var input = rawInput
				.Where(entry => !string.IsNullOrWhiteSpace(entry));

			Part1Solution = SolvePart1(input.ToArray()).ToString();
		}

		private static int SolvePart1(string[] input)
		{
			return -1;
		}
	}

	public static class Day18Helpers
	{

	}
}
