using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day16Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var input = rawInput.Single();

			Part1Solution = SolvePart1(input).ToString();
		}

		private static int SolvePart1(string bitsTransmission)
		{
			return -1;
		}
	}

	public static class Day16Helpers
	{
		
	}
}
