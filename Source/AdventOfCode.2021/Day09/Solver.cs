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
			var caveIndexMin = 0;
			var caveIndexMax = heightMap.Length - 1;
			var locationIndexMin = 0;
			var locationIndexMax = heightMap.First().Length - 1;

			var lowPoints = new List<int>();

			foreach (var caveIndex in Enumerable.Range(caveIndexMin, caveIndexMax + 1))
			{
				foreach (var locationIndex in Enumerable.Range(locationIndexMin, locationIndexMax + 1))
				{
					if ((caveIndex == caveIndexMin || (heightMap[caveIndex])[locationIndex] < (heightMap[caveIndex - 1])[locationIndex]) &&
						(caveIndex == caveIndexMax || (heightMap[caveIndex][locationIndex]) < (heightMap[caveIndex + 1])[locationIndex]) &&
						(locationIndex == locationIndexMin || (heightMap[caveIndex][locationIndex]) < (heightMap[caveIndex])[locationIndex - 1]) &&
						(locationIndex == locationIndexMax || (heightMap[caveIndex][locationIndex]) < (heightMap[caveIndex])[locationIndex + 1]))
					{
						lowPoints.Add(int.Parse((heightMap[caveIndex])[locationIndex].ToString()));
					}
				}
			}

			var riskLevelSum = lowPoints.Count() + lowPoints.Sum();

			return riskLevelSum;
		}
	}
}
