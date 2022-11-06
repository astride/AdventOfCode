using Common.Interfaces;
using Common.Models;

namespace Year2021;

public class Day09Solver : IPuzzleSolver
{
	public string Title => "Smoke Basin";
	public string Part1Solution { get; set; } = string.Empty;
	public string Part2Solution { get; set; } = string.Empty;

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
			.Select(coordinate => int.Parse((heightMap[coordinate.Y])[coordinate.X].ToString()))
			.ToList();

		var riskLevelSum = lowPoints.Count + lowPoints.Sum();

		return riskLevelSum;
	}

	private static int SolvePart2(string[] heightMap)
	{
		var basinMap = heightMap.GenerateFramedBoolMap();

		var basinOriginCoordinates = heightMap.GetLowPointCoordinates()
			.Select(coordinate => new Coordinate(coordinate.X + 1, coordinate.Y + 1));

		var largestBasins = basinMap.GetLargestBasins(3, basinOriginCoordinates);

		return largestBasins.Aggregate((x, y) => x * y);
	}
}

public static class Day09Helpers
{
	private static readonly Coordinate Above = new(0, 1);
	private static readonly Coordinate Right = new(1, 0);
	private static readonly Coordinate Below = new(0, -1);
	private static readonly Coordinate Left	= new(-1, 0);

	public static IEnumerable<Coordinate> GetLowPointCoordinates(this string[] map)
	{
		const int caveIndexMin = 0;
		var caveIndexMax = map.Length - 1;
		const int locationIndexMin = 0;
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

		foreach (var startingPoint in startingPoints)
		{
			var basinSize = startingPoint.GetBasinSize(map);

			if (basinSize > largestBasins.Min())
			{
				largestBasins.Remove(largestBasins.Min());
				largestBasins.Add(basinSize);
			}
		}

		return largestBasins;
	}

	private static int GetBasinSize(this Coordinate origin, bool[,] map)
	{
		return new List<Coordinate> { origin }.GetBasinSize(map, origin);
	}

	private static int GetBasinSize(this IReadOnlyCollection<Coordinate> thisLevelCoordinates, bool[,] map, Coordinate reference)
	{
		if (!thisLevelCoordinates.Any())
		{
			return 0;
		}

		var coordinatesToSearchFromByCoordinateShift = new Dictionary<Coordinate, IEnumerable<Coordinate>>
		{
			[Above] = thisLevelCoordinates.Where(coordinate => coordinate.Y >= reference.Y),
			[Right] = thisLevelCoordinates.Where(coordinate => coordinate.X >= reference.X),
			[Below] = thisLevelCoordinates.Where(coordinate => coordinate.Y <= reference.Y),
			[Left] = thisLevelCoordinates.Where(coordinate => coordinate.X <= reference.X)
		};

		var nextLevelCoordinates = new List<Coordinate>();

		foreach (var searchScope in coordinatesToSearchFromByCoordinateShift)
        {
			nextLevelCoordinates.AddRange(searchScope.Value
				.Where(coordinate => map[coordinate.X + searchScope.Key.X, coordinate.Y + searchScope.Key.Y])
				.Select(coordinate => new Coordinate(coordinate.X + searchScope.Key.X, coordinate.Y + searchScope.Key.Y)));
        }

		return
			thisLevelCoordinates.Count +
			nextLevelCoordinates.Distinct().ToList().GetBasinSize(map, reference);
	}
}
