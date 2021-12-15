using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day15Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			//Info: The shape of the cavern resembles a square
			var map = rawInput
				.Where(entry => !string.IsNullOrWhiteSpace(entry))
				.ToArray();

			Part1Solution = SolvePart1(map).ToString();
		}

		private static double SolvePart1(string[] map)
		{
			var riskLevelMap = map.AsRiskLevelMap();
			var lowestTotalRisk = riskLevelMap.GetLowestTotalRiskOfSouthEastFacingPath();

			return lowestTotalRisk;
		}
	}

	public static class Day15Helpers
	{
		private static double[,] LowestTotalRiskMap;

		public static double[,] AsRiskLevelMap(this string[] squaredMap)
		{
			var mapSize = squaredMap.Length;

			var map = new double[mapSize, mapSize];

			foreach (var y in Enumerable.Range(0, mapSize))
			{
				foreach (var x in Enumerable.Range(0, mapSize))
				{
					map[x, y] = int.Parse((squaredMap[y])[x].ToString());
				}
			}

			return map;
		}

		public static double GetLowestTotalRiskOfSouthEastFacingPath(this double[,] squaredMap)
		{
			var mapSize = squaredMap.GetLength(0);

			LowestTotalRiskMap = new double[mapSize, mapSize];

			foreach (var i in Enumerable.Range(1, mapSize - 1))
			{
				//calculate top--bottom along y axis
				foreach (var y in Enumerable.Range(0, i))
				{
					LowestTotalRiskMap[i, y] = y == 0
						? LowestTotalRiskMap[i - 1, y] + squaredMap[i, y]
						: Math.Min(LowestTotalRiskMap[i - 1, y], LowestTotalRiskMap[i, y - 1]) + squaredMap[i, y];
				}

				//calculate left--right along x axis
				foreach (var x in Enumerable.Range(0, i))
				{
					LowestTotalRiskMap[x, i] = x == 0
						? LowestTotalRiskMap[x, i - 1] + squaredMap[x, i]
						: Math.Min(LowestTotalRiskMap[x, i - 1], LowestTotalRiskMap[x - 1, i]) + squaredMap[x, i];
				}

				//calculate corner
				LowestTotalRiskMap[i, i] = Math.Min(LowestTotalRiskMap[i - 1, i], LowestTotalRiskMap[i, i - 1]) + squaredMap[i, i];
			}

			return LowestTotalRiskMap[mapSize - 1, mapSize - 1];
		}
	}
}
