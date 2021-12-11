using AdventOfCode.Common;
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

		private static int SolvePart1(string[] octopusEnergyLevels)
		{
			var energyLevelsFlattened = octopusEnergyLevels.AsFlattened();

			//TODO
			return -1;
		}
	}

	public static class Day11Helpers
	{
		private readonly static int EnergyLevelMin = 0;
		private readonly static int EnergyLevelMax = 9;

		private static int EnergyLevelsRowCount;
		private static int EnergyLevelRowSize;

		public static List<int> AsFlattened(this string[] energyLevels)
		{
			EnergyLevelsRowCount = energyLevels.Length;
			EnergyLevelRowSize = energyLevels.First().Length;

			return energyLevels
				.SelectMany(energyLevelRow => energyLevelRow
					.ToCharArray()
					.Select(energyLevel => int.Parse(energyLevel.ToString())))
				.ToList();
		}

		public static void ExecuteStep(this List<int> energyLevels)
		{

		}
	}
}
