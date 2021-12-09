using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day09Solver : IPuzzleSolver
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

		private static int SolvePart1(string[] heightMap)
		{
			var lowPoints = heightMap.GetLowPointCoordinates()
				.Select(coor => int.Parse((heightMap[coor.X])[coor.Y].ToString()));

			var riskLevelSum = lowPoints.Count() + lowPoints.Sum();

			return riskLevelSum;
		}
	}

	public static class Day09Helpers
	{
		public static IEnumerable<(int X, int Y)> GetLowPointCoordinates(this string[] map)
		{
			var caveIndexMin = 0;
			var caveIndexMax = map.Length - 1;
			var locationIndexMin = 0;
			var locationIndexMax = map.First().Length - 1;

			var lowPointCoordinates = new List<(int X, int Y)>();

			foreach (var caveIndex in Enumerable.Range(caveIndexMin, caveIndexMax + 1))
			{
				foreach (var locationIndex in Enumerable.Range(locationIndexMin, locationIndexMax + 1))
				{
					if ((caveIndex == caveIndexMin || (map[caveIndex])[locationIndex] < (map[caveIndex - 1])[locationIndex]) &&
						(caveIndex == caveIndexMax || (map[caveIndex][locationIndex]) < (map[caveIndex + 1])[locationIndex]) &&
						(locationIndex == locationIndexMin || (map[caveIndex][locationIndex]) < (map[caveIndex])[locationIndex - 1]) &&
						(locationIndex == locationIndexMax || (map[caveIndex][locationIndex]) < (map[caveIndex])[locationIndex + 1]))
					{
						lowPointCoordinates.Add((caveIndex, locationIndex));
					}
				}
			}

			return lowPointCoordinates;
		}
	}
}
