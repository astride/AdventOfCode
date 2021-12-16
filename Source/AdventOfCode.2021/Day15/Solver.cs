using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day15Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; } //2860: Too high

		public void SolvePuzzle(string[] rawInput)
		{
			//Info: The shape of the cavern resembles a square
			var map = rawInput
				.Where(entry => !string.IsNullOrWhiteSpace(entry))
				.ToArray();

			Part1Solution = SolvePart1(map).ToString();
			Part2Solution = SolvePart2(map).ToString();
		}

		private static double SolvePart1(string[] map)
		{
			var riskLevelMap = map.AsRiskLevelMap();
			var lowestTotalRisk = riskLevelMap.GetLowestTotalRiskOfSouthEastFacingPath();

			return lowestTotalRisk;
		}

		private static double SolvePart2(string[] map)
		{
			var riskLevelMap = map.AsRiskLevelMap();
			var lowestTotalRisk = riskLevelMap.GetLowestTotalRiskOfSouthEastFacingPath(5);

			return lowestTotalRisk;
		}
	}

	public static class Day15Helpers
	{
		private static double[,] LowestTotalRiskMap;
		private static double[,,] BottomRowLowestTotalRiskOfTileAtCoor;
		private static double[,,] RightColLowestTotalRiskOfTileAtCoor;

		public static int[,] AsRiskLevelMap(this string[] squaredMap)
		{
			var mapSize = squaredMap.Length;

			var map = new int[mapSize, mapSize];

			foreach (var y in Enumerable.Range(0, mapSize))
			{
				foreach (var x in Enumerable.Range(0, mapSize))
				{
					map[x, y] = int.Parse((squaredMap[y])[x].ToString());
				}
			}

			return map;
		}

		#region Part 1
		public static double GetLowestTotalRiskOfSouthEastFacingPath(this int[,] squaredMap)
		{
			var mapSize = squaredMap.GetLength(0);

			squaredMap.PopulateLowestTotalRiskMapOfSouthEastFacingPath();

			return LowestTotalRiskMap[mapSize - 1, mapSize - 1];
		}

		public static void PopulateLowestTotalRiskMapOfSouthEastFacingPath(this int[,] squaredMap)
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
		}
		#endregion

		#region Part 2
		public static double GetLowestTotalRiskOfSouthEastFacingPath(this int[,] baseMap, int mapRepetitions)
		{
			var mapSize = baseMap.GetLength(0);
			var map = new int[mapSize, mapSize]; //TODO unneccesary?

			BottomRowLowestTotalRiskOfTileAtCoor = new double[mapRepetitions, mapRepetitions, mapSize];
			RightColLowestTotalRiskOfTileAtCoor = new double[mapRepetitions, mapRepetitions, mapSize];

			int incrementor;

			bool isUppermostTile;
			bool isLeftmostTile;

			foreach (var y in Enumerable.Range(0, mapRepetitions))
			{
				isUppermostTile = y == 0;

				foreach (var x in Enumerable.Range(0, mapRepetitions))
				{
					isLeftmostTile = x == 0;

					if (isUppermostTile && isLeftmostTile)
					{
						// populate Lowest...Map
						baseMap.PopulateLowestTotalRiskMapOfSouthEastFacingPath(0, 0);
					}
					else
					{
						// prepare repeated map
						incrementor = x + y;

						foreach (var xMap in Enumerable.Range(0, mapSize))
						{
							foreach (var yMap in Enumerable.Range(0, mapSize))
							{
								map[xMap, yMap] = 1 + ((baseMap[xMap, yMap] + incrementor - 1) % 9);
							}
						}

						// populate Lowest...Map
						map.PopulateLowestTotalRiskMapOfSouthEastFacingPath(x, y);
					}
				}
			}

			return LowestTotalRiskMap[mapSize - 1, mapSize - 1];
		}

		private static void PopulateLowestTotalRiskMapOfSouthEastFacingPath(this int[,] map, int xTile, int yTile)
		{
			var isUppermostTile = yTile == 0;
			var isLeftmostTile = xTile == 0;

			var mapSize = map.GetLength(0);

			LowestTotalRiskMap = new double[mapSize, mapSize];

			var riskLevel = map[0, 0];
			double riskLevelLeftOf;
			double riskLevelAbove;

			// populate uppermost, leftmost cell if isn't base map
			if (!isUppermostTile && !isLeftmostTile)
			{
				LowestTotalRiskMap[0, 0] = riskLevel
					+ Math.Min(
						BottomRowLowestTotalRiskOfTileAtCoor[xTile, yTile - 1, 0],
						RightColLowestTotalRiskOfTileAtCoor[xTile - 1, yTile, 0]);
			}
			else if (!isUppermostTile)
			{
				LowestTotalRiskMap[0, 0] = riskLevel
					+ BottomRowLowestTotalRiskOfTileAtCoor[xTile, yTile - 1, 0];
			}
			else if (!isLeftmostTile)
			{
				LowestTotalRiskMap[0, 0] = riskLevel
					+ RightColLowestTotalRiskOfTileAtCoor[xTile - 1, yTile, 0];
			}

			bool isLastLevel;

			// populate rest of map
			foreach (var i in Enumerable.Range(1, mapSize - 1))
			{
				isLastLevel = i == mapSize - 1;

				// populate top--bottom
				foreach (var y in Enumerable.Range(0, i))
				{
					riskLevel = map[i, y];
					riskLevelLeftOf = LowestTotalRiskMap[i - 1, y];

					LowestTotalRiskMap[i, y] = y == 0
						? isUppermostTile
							? riskLevel + riskLevelLeftOf
							: riskLevel + Math.Min(riskLevelLeftOf, BottomRowLowestTotalRiskOfTileAtCoor[xTile, yTile - 1, i])
						: riskLevel + Math.Min(riskLevelLeftOf, LowestTotalRiskMap[i, y - 1]);

					if (isLastLevel)
					{
						RightColLowestTotalRiskOfTileAtCoor[xTile, yTile, y] = LowestTotalRiskMap[i, y];
					}
				}

				// populate left--right
				foreach (var x in Enumerable.Range(0, i))
				{
					riskLevel = map[x, i];
					riskLevelAbove = LowestTotalRiskMap[x, i - 1];

					LowestTotalRiskMap[x, i] = x == 0
						? isLeftmostTile
							? riskLevel + riskLevelAbove
							: riskLevel + Math.Min(riskLevelAbove, RightColLowestTotalRiskOfTileAtCoor[xTile - 1, yTile, i])
						: riskLevel + Math.Min(riskLevelAbove, LowestTotalRiskMap[x - 1, i]);

					if (isLastLevel)
					{
						BottomRowLowestTotalRiskOfTileAtCoor[xTile, yTile, x] = LowestTotalRiskMap[x, i];
					}
				}

				// populate corner
				riskLevel = map[i, i];
				riskLevelLeftOf = LowestTotalRiskMap[i - 1, i];
				riskLevelAbove = LowestTotalRiskMap[i, i - 1];

				LowestTotalRiskMap[i, i] = riskLevel + Math.Min(riskLevelLeftOf, riskLevelAbove);

				if (isLastLevel)
				{
					RightColLowestTotalRiskOfTileAtCoor[xTile, yTile, i] = LowestTotalRiskMap[i, i];
					BottomRowLowestTotalRiskOfTileAtCoor[xTile, yTile, i] = LowestTotalRiskMap[i, i];
				}
			}
		}
		#endregion

		//TODO move to common? nice to have
		private static void Print<T>(this T[,] map)
		{
			foreach (var y in Enumerable.Range(0, map.GetLength(1)))
			{
				foreach (var x in Enumerable.Range(0, map.GetLength(0)))
				{
					Console.Write(map[x, y] + " ");
				}

				Console.WriteLine();
			}

			Console.WriteLine();
		}
	}
}
