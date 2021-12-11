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
			Part2Solution = SolvePart2(input).ToString();
		}

		private static int SolvePart1(string[] heightMap)
		{
			var lowPoints = heightMap.GetLowPointCoordinates()
				.Select(coor => int.Parse((heightMap[coor.Y])[coor.X].ToString()));

			var riskLevelSum = lowPoints.Count() + lowPoints.Sum();

			return riskLevelSum;
		}

		private static int SolvePart2(string[] heightMap)
		{
			var basinMap = heightMap.GenerateFramedBoolMap();

			var basinOrigoCoordinates = heightMap.GetLowPointCoordinates()
				.Select(coor => (coor.X + 1, coor.Y + 1));

			var largestBasins = basinMap.GetLargestBasins(3, basinOrigoCoordinates);

			return largestBasins.Aggregate((x, y) => x * y);
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
						lowPointCoordinates.Add((locationIndex, caveIndex));
					}
				}
			}

			return lowPointCoordinates;
		}

		public static bool[,] GenerateFramedBoolMap(this string[] map)
		{
			var sizeH = map.First().Length;
			var sizeV = map.Length;

			var boolMap = new bool[sizeH + 2, sizeV + 2];

			foreach (var indexVer in Enumerable.Range(0, sizeV))
			{
				foreach (var indexHor in Enumerable.Range(0, sizeH))
				{
					if ((map[indexVer])[indexHor] != '9')
					{
						boolMap[indexHor + 1, indexVer + 1] = true;
					}
				}
			}

			return boolMap;
		}

		public static IEnumerable<int> GetLargestBasins(this bool[,] map, int basinCount, IEnumerable<(int X, int Y)> startingPoints)
		{
			var largestBasins = Enumerable.Repeat(0, basinCount).ToList();
			int basinSize;

			foreach (var startingPoint in startingPoints)
			{
				basinSize = startingPoint.GetBasinSize(map);

				if (basinSize > largestBasins.Min())
				{
					largestBasins.Remove(largestBasins.Min());
					largestBasins.Add(basinSize);
				}
			}

			return largestBasins;
		}

		private static int GetBasinSize(this (int X, int Y) origo, bool[,] map)
		{
			return new List<(int X, int Y)> { origo }.GetBasinSize(map, origo);
		}

		public static int GetBasinSize(this IEnumerable<(int X, int Y)> thisLevelCoors, bool[,] map, (int X, int Y) refCoor)
		{
			if (!thisLevelCoors.Any())
			{
				return 0;
			}

			var coorsSearchingAbove = thisLevelCoors.Where(coor => coor.Y >= refCoor.Y);
			var coorsSearchingRight = thisLevelCoors.Where(coor => coor.X >= refCoor.X);
			var coorsSearchingBelow = thisLevelCoors.Where(coor => coor.Y <= refCoor.Y);
			var coorsSearchingLeft = thisLevelCoors.Where(coor => coor.X <= refCoor.X);

			List<(int X, int Y)> nextLevelCoors = new List<(int X, int Y)>();

			if (coorsSearchingAbove.Any())
			{
				nextLevelCoors.AddRange(coorsSearchingAbove
					.Where(coor => map[coor.X, coor.Y + 1])
					.Select(coor => (coor.X, coor.Y + 1)));
			}
			if (coorsSearchingRight.Any())
			{
				nextLevelCoors.AddRange(coorsSearchingRight
					.Where(coor => map[coor.X + 1, coor.Y])
					.Select(coor => (coor.X + 1, coor.Y)));
			}
			if (coorsSearchingBelow.Any())
			{
				nextLevelCoors.AddRange(coorsSearchingBelow
					.Where(coor => map[coor.X, coor.Y - 1])
					.Select(coor => (coor.X, coor.Y - 1)));
			}
			if (coorsSearchingLeft.Any())
			{
				nextLevelCoors.AddRange(coorsSearchingLeft
					.Where(coor => map[coor.X - 1, coor.Y])
					.Select(coor => (coor.X - 1, coor.Y)));
			}

			return thisLevelCoors.Count()
				+ nextLevelCoors.Distinct().GetBasinSize(map, refCoor);
		}
	}
}