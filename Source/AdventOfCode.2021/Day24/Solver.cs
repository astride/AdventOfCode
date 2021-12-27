using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day24Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var input = rawInput;

			Part1Solution = SolvePart1(input).ToString();
		}

		private static int SolvePart1(IEnumerable<string> input)
		{
			return -1;
		}
	}

	public static class Day24Helpers
	{

	}
}
