﻿using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day11Solver : IPuzzleSolver
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

		private static int SolvePart1(string[] input)
		{
			//TODO
			return -1;
		}
	}

	public static class Day11Helpers
	{
		
	}
}
