using Common.Interfaces;

namespace Year2021;

public class Day15Solver : IPuzzleSolver
{
	public string Title => "Chiton";
	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var map = GetMap(input);
		
		var riskLevelMap = map.AsRiskLevelMap();
		var lowestTotalRisk = riskLevelMap.GetLowestTotalRiskOfSouthEastFacingPath();

		return lowestTotalRisk;
	}

	// TODO Implementation is not finished at all
	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var map = GetMap(input);
		
		var riskLevelMap = map.AsRiskLevelMap();
		var lowestTotalRisk = riskLevelMap.GetLowestTotalRiskOfSouthEastFacingPath(5);

		return lowestTotalRisk;
	}

	private static string[] GetMap(string[] input)
	{
		//Info: The shape of the cavern resembles a square
		return input
			.Where(entry => !string.IsNullOrWhiteSpace(entry))
			.ToArray();
	}
}

public static class Day15Helpers
{
	private static double[,] LowestTotalRiskMap;
	private static double[][,] BottomRowLowestTotalRiskOfTileAtCoor;
	private static double[][,] RightColLowestTotalRiskOfTileAtCoor;

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

	private static void PopulateLowestTotalRiskMapOfSouthEastFacingPath(this int[,] squaredMap,
		int[] bottomRowOfAboveTile = null,
		int[] rightColOfLeftTile = null)
	{
		var mapSize = squaredMap.GetLength(0);

		LowestTotalRiskMap = new double[mapSize, mapSize];

		var isUppermostTile = bottomRowOfAboveTile == null;
		var isLeftmostTile = rightColOfLeftTile == null;

		// populate uppermost, leftmost cell if isn't base map
		if (!isUppermostTile && !isLeftmostTile)
		{
			LowestTotalRiskMap[0, 0] = Math.Min(bottomRowOfAboveTile[0], rightColOfLeftTile[0]) + squaredMap[0, 0];
		}
		else if (!isUppermostTile)
		{
			LowestTotalRiskMap[0, 0] = bottomRowOfAboveTile[0] + squaredMap[0, 0];
		}
		else if (!isLeftmostTile)
		{
			LowestTotalRiskMap[0, 0] = rightColOfLeftTile[0] + squaredMap[0, 0];
		}

		// populate rest of map
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

	public static double GetLowestTotalRiskOfSouthEastFacingPath(this int[,] squaredMap)
	{
		var mapSize = squaredMap.GetLength(0);

		squaredMap.PopulateLowestTotalRiskMapOfSouthEastFacingPath();

		return LowestTotalRiskMap[mapSize - 1, mapSize - 1];
	}

	public static double GetLowestTotalRiskOfSouthEastFacingPath(this int[,] baseMap, int mapRepetitions)
	{
		var mapSize = baseMap.GetLength(0);
		var map = new int[mapSize, mapSize]; //TODO unnecessary?

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
					baseMap.PopulateLowestTotalRiskMapOfSouthEastFacingPath();
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

					// generate Lowest...Map
					if (isUppermostTile)
					{
						// call generate method w/previous rightcol array
					}
					else if (isLeftmostTile)
					{
						// call generate method w/previous bottomrow array
					}
					else
					{
						// call generate method w/previous rightcol and bottomrow array
					}
				}

				// update BottomRow...AtCoor
				// update RightCol...AtCoor
			}
		}

		return LowestTotalRiskMap[mapSize - 1, mapSize - 1];
	}
}
