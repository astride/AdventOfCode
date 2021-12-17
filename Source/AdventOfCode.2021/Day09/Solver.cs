using AdventOfCode.Common;
using AdventOfCode.Common.Classes;
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
				.Select(coor => new Coordinate(coor.X + 1, coor.Y + 1));

			var largestBasins = basinMap.GetLargestBasins(3, basinOrigoCoordinates);

			return largestBasins.Aggregate((x, y) => x * y);
		}
	}

	public static class Day09Helpers
	{
		private static readonly Coordinate Above = new Coordinate(0, 1);
		private static readonly Coordinate Right = new Coordinate(1, 0);
		private static readonly Coordinate Below = new Coordinate(0, -1);
		private static readonly Coordinate Left	= new Coordinate(-1, 0);

		public static IEnumerable<Coordinate> GetLowPointCoordinates(this string[] map)
		{
			var caveIndexMin = 0;
			var caveIndexMax = map.Length - 1;
			var locationIndexMin = 0;
			var locationIndexMax = map.First().Length - 1;

			var lowPointCoordinates = new List<Coordinate>();

			foreach (var caveIndex in Enumerable.Range(caveIndexMin, caveIndexMax + 1))
			{
				foreach (var locationIndex in Enumerable.Range(locationIndexMin, locationIndexMax + 1))
				{
					if ((caveIndex == caveIndexMin || (map[caveIndex])[locationIndex] < (map[caveIndex - 1])[locationIndex]) &&
						(caveIndex == caveIndexMax || (map[caveIndex][locationIndex]) < (map[caveIndex + 1])[locationIndex]) &&
						(locationIndex == locationIndexMin || (map[caveIndex][locationIndex]) < (map[caveIndex])[locationIndex - 1]) &&
						(locationIndex == locationIndexMax || (map[caveIndex][locationIndex]) < (map[caveIndex])[locationIndex + 1]))
					{
						lowPointCoordinates.Add(new Coordinate(locationIndex, caveIndex));
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

		public static IEnumerable<int> GetLargestBasins(this bool[,] map, int basinCount, IEnumerable<Coordinate> startingPoints)
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

		private static int GetBasinSize(this Coordinate origo, bool[,] map)
		{
			return new List<Coordinate> { origo }.GetBasinSize(map, origo);
		}

		public static int GetBasinSize(this IEnumerable<Coordinate> thisLevelCoors, bool[,] map, Coordinate refCoor)
		{
			if (!thisLevelCoors.Any())
			{
				return 0;
			}

			var coorsToSearchFromByCoorShift = new Dictionary<Coordinate, IEnumerable<Coordinate>>
			{
				[Above] = thisLevelCoors.Where(coor => coor.Y >= refCoor.Y),
				[Right] = thisLevelCoors.Where(coor => coor.X >= refCoor.X),
				[Below] = thisLevelCoors.Where(coor => coor.Y <= refCoor.Y),
				[Left] = thisLevelCoors.Where(coor => coor.X <= refCoor.X)
			};

			var nextLevelCoors = new List<Coordinate>();

			foreach (var searchScope in coorsToSearchFromByCoorShift)
            {
				nextLevelCoors.AddRange(searchScope.Value
					.Where(coor => map[coor.X + searchScope.Key.X, coor.Y + searchScope.Key.Y])
					.Select(coor => new Coordinate(coor.X + searchScope.Key.X, coor.Y + searchScope.Key.Y)));
            }

			return thisLevelCoors.Count()
				+ nextLevelCoors.Distinct().GetBasinSize(map, refCoor);
		}
	}
}