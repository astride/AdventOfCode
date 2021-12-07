using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day08Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var input = rawInput
				.Where(entry => !string.IsNullOrWhiteSpace(entry))
				.Single()
				.Split(',')
				.Select(entry => int.Parse(entry))
				.ToArray();

			Part1Solution = SolvePart1(input).ToString();
		}

		private static int SolvePart1(int[] input)
		{
			//TODO
			return -1;
		}
	}

	public static class Day08Helpers
	{

	}
}
