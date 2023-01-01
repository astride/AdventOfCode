using Common.Interfaces;

namespace Year2021;

public class Day12Solver : IPuzzleSolver
{
	public string Title => "Passage Pathing";
	public bool UsePartSpecificExampleInputFiles => true;
	
	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var connectedCavesMap = GetConnectedCavesMap(input);

		var pathCount = connectedCavesMap.GetPathCountVisitingSmallCavesOnceOrLess();

		return pathCount;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return "Not implemented";
	}

	private static IEnumerable<IEnumerable<string>> GetConnectedCavesMap(string[] input)
	{
		return input
			.Where(entry => !string.IsNullOrWhiteSpace(entry))
			.Select(entry => entry.Split('-'));
	}
}

public static class Day12Helpers
{
	private const string Start = "start";
	private const string End = "end";

	private static IEnumerable<string> StartingPoints;
	private static IEnumerable<string> EndingPoints;
	private static IEnumerable<string> SmallCaves;
	private static IDictionary<string, IEnumerable<string>> InitialPointToConnectedPoints;

	public static int GetPathCountVisitingSmallCavesOnceOrLess(this IEnumerable<IEnumerable<string>> connectionMap)
	{
		connectionMap.CreateOverview();

		var pathCount = 0;

		foreach (var startingPoint in StartingPoints)
		{
			var remainingPointsConnectedToPoint = new Dictionary<string, IEnumerable<string>>(InitialPointToConnectedPoints);

			pathCount += remainingPointsConnectedToPoint.GetPathCount(startingPoint);
		}

		return pathCount;
	}

	private static int GetPathCount(this Dictionary<string, IEnumerable<string>> pointsConnectedToPoint, string fromPoint)
	{
		var count = 0;

		// from point is an ending point
		if (EndingPoints.Contains(fromPoint)) count++;

		var availableSuccessors = pointsConnectedToPoint[fromPoint];

		// path cannot get any longer
		if (!availableSuccessors.Any()) return count;

		// small caves may be visited only once; when visited, they should not be available anymore
		if (SmallCaves.Contains(fromPoint))
		{
			pointsConnectedToPoint[fromPoint] = Array.Empty<string>();

			var pointsWithConnectionToJustVisitedSmallCave = pointsConnectedToPoint
				.Where(p => p.Value.Contains(fromPoint))
				.ToList();

			foreach (var pctp in pointsWithConnectionToJustVisitedSmallCave)
			{
				pointsConnectedToPoint[pctp.Key] = pctp.Value.Where(value => value != fromPoint);
			}
		}

		// continue path
		foreach (var successor in availableSuccessors)
		{
			var remainingPointsConnectedToPoint = new Dictionary<string, IEnumerable<string>>(pointsConnectedToPoint);

			count += remainingPointsConnectedToPoint.GetPathCount(successor);
		}

		return count;
	}

	private static void CreateOverview(this IEnumerable<IEnumerable<string>> connectionMap)
	{
		StartingPoints = connectionMap
			.Where(connection => connection.Contains(Start))
			.SelectMany(connection => connection)
			.Where(connectionPoint => connectionPoint != Start);

		EndingPoints = connectionMap
			.Where(connection => connection.Contains(End))
			.SelectMany(connection => connection)
			.Where(connectionPoint => connectionPoint != End);

		var points = connectionMap
			.SelectMany(connection => connection)
			.Distinct()
			.Where(connectionPoint =>
				connectionPoint != Start &&
				connectionPoint != End);

		SmallCaves = points
			.Where(point => point.ToLower() == point);

		InitialPointToConnectedPoints = new Dictionary<string, IEnumerable<string>>();

		foreach (var point in points)
		{
			InitialPointToConnectedPoints[point] = connectionMap
				.Where(connection => connection.Contains(point))
				.SelectMany(connection => connection)
				.Distinct()
				.Where(connectionPoint => 
					connectionPoint != point &&
					connectionPoint != Start &&
					connectionPoint != End);
		}

		var unvisitableCaves = InitialPointToConnectedPoints
			.Where(p =>
				SmallCaves.Contains(p.Key) &&
				p.Value.Count() == 1 &&
				SmallCaves.Contains(p.Value.Single()))
			.Select(p => p.Key)
			.ToList();

		if (unvisitableCaves.Any())
		{
			SmallCaves = SmallCaves.Except(unvisitableCaves);

			foreach (var unvisitableCave in unvisitableCaves)
			{
				InitialPointToConnectedPoints.Remove(unvisitableCave);

				var pointsWithConnectionToUnvisitableCave = InitialPointToConnectedPoints
					.Where(p => p.Value.Contains(unvisitableCave))
					.ToList();

				foreach (var pointsConnectedToPoint in pointsWithConnectionToUnvisitableCave)
				{
					InitialPointToConnectedPoints[pointsConnectedToPoint.Key] = pointsConnectedToPoint.Value.Where(p => p != unvisitableCave);
				}
			}
		}
	}
}
