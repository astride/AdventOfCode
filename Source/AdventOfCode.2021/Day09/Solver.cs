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
		private static (int X, int Y) Above = (0, 1);
		private static (int X, int Y) Below = (0, -1);
		private static (int X, int Y) Left = (-1, 0);
		private static (int X, int Y) Right = (1, 0);

		private static IEnumerable<(int X, int Y)> NeighboringCoorsToCheckAtOrigo = new List<(int X, int Y)> { Above, Below, Left, Right };
		
		private static IEnumerable<(int X, int Y)> NeighboringCoorsToCheckAtTopRightEdge = new List<(int X, int Y)> { Above, Right };
		private static IEnumerable<(int X, int Y)> NeighboringCoorsToCheckAtBottomRightEdge = new List<(int X, int Y)> { Below, Right };
		private static IEnumerable<(int X, int Y)> NeighboringCoorsToCheckAtBottomLeftEdge = new List<(int X, int Y)> { Below, Left };
		private static IEnumerable<(int X, int Y)> NeighboringCoorsToCheckAtTopLeftEdge = new List<(int X, int Y)> { Above, Left };

		private static IEnumerable<(int X, int Y)> NeighboringCoorsToCheckAtCornerTop = NeighboringCoorsToCheckAtTopLeftEdge.Append(Right);
		private static IEnumerable<(int X, int Y)> NeighboringCoorsToCheckAtCornerRight = NeighboringCoorsToCheckAtTopRightEdge.Append(Below);
		private static IEnumerable<(int X, int Y)> NeighboringCoorsToCheckAtCornerBottom = NeighboringCoorsToCheckAtBottomRightEdge.Append(Left);
		private static IEnumerable<(int X, int Y)> NeighboringCoorsToCheckAtCornerLeft = NeighboringCoorsToCheckAtBottomLeftEdge.Append(Above);

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
			return 1
				+ NeighboringCoorsToCheckAtOrigo
					.Where(coors => map[origo.X + coors.X, origo.Y + coors.Y])
					.Select(coors => (origo.X + coors.X, origo.Y + coors.Y))
					.GetBasinSize(map, origo);
		}

		public static int GetBasinSize(this IEnumerable<(int X, int Y)> thisLevelCoors, bool[,] map, (int X, int Y) refCoor)
		{
			if (!thisLevelCoors.Any())
			{
				return 0;
			}

			var cornerTopCoor = thisLevelCoors.SingleOrDefault(coor => coor.X == refCoor.X && coor.Y > refCoor.Y);
			var cornerRightCoor = thisLevelCoors.SingleOrDefault(coor => coor.X > refCoor.X && coor.Y == refCoor.Y);
			var cornerBottomCoor = thisLevelCoors.SingleOrDefault(coor => coor.X == refCoor.X && coor.Y < refCoor.Y);
			var cornerLeftCoor = thisLevelCoors.SingleOrDefault(coor => coor.X < refCoor.X && coor.Y == refCoor.Y);

			var topRightCoors = thisLevelCoors.Where(coor => coor.X > refCoor.X && coor.Y > refCoor.Y);
			var bottomRightCoors = thisLevelCoors.Where(coor => coor.X > refCoor.X && coor.Y < refCoor.Y);
			var bottomLeftCoors = thisLevelCoors.Where(coor => coor.X < refCoor.X && coor.Y < refCoor.Y);
			var topLeftCoors = thisLevelCoors.Where(coor => coor.X < refCoor.X && coor.Y > refCoor.Y);

			List<(int X, int Y)> nextLevelCoors = new List<(int X, int Y)>();

			if (cornerTopCoor != default)
			{
				nextLevelCoors.AddRange(NeighboringCoorsToCheckAtCornerTop
					.Where(coorShift => map[
						cornerTopCoor.X + coorShift.X,
						cornerTopCoor.Y + coorShift.Y])
					.Select(coorShift => (
						cornerTopCoor.X + coorShift.X,
						cornerTopCoor.Y + coorShift.Y)));
			}
			if (cornerRightCoor != default)
			{
				nextLevelCoors.AddRange(NeighboringCoorsToCheckAtCornerRight
					.Where(coorShift => map[
						cornerRightCoor.X + coorShift.X,
						cornerRightCoor.Y + coorShift.Y])
					.Select(coorShift => (
						cornerRightCoor.X + coorShift.X,
						cornerRightCoor.Y + coorShift.Y)));
			}
			if (cornerBottomCoor != default)
			{
				nextLevelCoors.AddRange(NeighboringCoorsToCheckAtCornerBottom
					.Where(coorShift => map[
						cornerBottomCoor.X + coorShift.X,
						cornerBottomCoor.Y + coorShift.Y])
					.Select(coorShift => (
						cornerBottomCoor.X + coorShift.X,
						cornerBottomCoor.Y + coorShift.Y)));
			}
			if (cornerLeftCoor != default)
			{
				nextLevelCoors.AddRange(NeighboringCoorsToCheckAtCornerLeft
					.Where(coorShift => map[
						cornerLeftCoor.X + coorShift.X,
						cornerLeftCoor.Y + coorShift.Y])
					.Select(coorShift => (
						cornerLeftCoor.X + coorShift.X,
						cornerLeftCoor.Y + coorShift.Y)));
			}

			if (topRightCoors.Any())
			{
				nextLevelCoors.AddRange(topRightCoors
					.SelectMany(coor => NeighboringCoorsToCheckAtTopRightEdge
						.Select(coorShift => (
							coor.X + coorShift.X,
							coor.Y + coorShift.Y)))
					.Where(shiftedCoor => map[shiftedCoor.Item1, shiftedCoor.Item2]));
			}
			if (bottomRightCoors.Any())
			{
				nextLevelCoors.AddRange(bottomRightCoors
					.SelectMany(coor => NeighboringCoorsToCheckAtBottomRightEdge
						.Select(coorShift => (
							coor.X + coorShift.X,
							coor.Y + coorShift.Y)))
					.Where(shiftedCoor => map[shiftedCoor.Item1, shiftedCoor.Item2]));
			}
			if (bottomLeftCoors.Any())
			{
				nextLevelCoors.AddRange(bottomLeftCoors
					.SelectMany(coor => NeighboringCoorsToCheckAtBottomLeftEdge
						.Select(coorShift => (
							coor.X + coorShift.X,
							coor.Y + coorShift.Y)))
					.Where(shiftedCoor => map[shiftedCoor.Item1, shiftedCoor.Item2]));
			}
			if (topLeftCoors.Any())
			{
				nextLevelCoors.AddRange(topLeftCoors
					.SelectMany(coor => NeighboringCoorsToCheckAtTopLeftEdge
						.Select(coorShift => (
							coor.X + coorShift.X,
							coor.Y + coorShift.Y)))
					.Where(shiftedCoor => map[shiftedCoor.Item1, shiftedCoor.Item2]));
			}

			return thisLevelCoors.Count()
				+ nextLevelCoors.Distinct().GetBasinSize(map, refCoor);
		}
	}
}
