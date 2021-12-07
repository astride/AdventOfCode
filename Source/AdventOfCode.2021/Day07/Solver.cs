using AdventOfCode.Common;
using System;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day07Solver : IPuzzleSolver
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

		private static int SolvePart1(int[] crabPositions)
		{
			var minPos = crabPositions.Min();
			var maxPos = crabPositions.Max();

			var spentFuel = crabPositions.Length * (maxPos - minPos);

			foreach (var finalPos in Enumerable.Range(minPos, maxPos))
			{
				spentFuel = Math.Min(
					spentFuel,
					crabPositions.Select(pos => Math.Abs(pos - finalPos)).Sum());
			}

			return spentFuel;
		}
	}
}
