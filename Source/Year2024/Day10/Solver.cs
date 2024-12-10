using Common.Interfaces;
using Common.Models;

namespace Year2024;

public class Day10Solver : IPuzzleSolver
{
	public string Title => "Hoof It";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private static readonly Coordinate[] NeighborDeviations =
	{
		new(-1, 0),
		new(+1, 0),
		new(0, -1),
		new(0, +1),
	};
	
	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var rows = input.Length;
		var cols = input[0].Length;

		var trailheads = new List<Coordinate>();
		var heightByPosition = new Dictionary<Coordinate, int>();
		
		for (var iRow = 0; iRow < rows; iRow++)
		{
			for (var iCol = 0; iCol < cols; iCol++)
			{
				var coordinate = new Coordinate(iCol, iRow);
				var height = int.Parse(input[iRow][iCol].ToString());

				heightByPosition[coordinate] = height;

				if (height == 0)
				{
					trailheads.Add(coordinate);
				}
			}
		}

		var validCoordinatesByHeight = new Dictionary<int, List<Coordinate>>();
		var trailheadScore = 0;
		
		foreach (var trailhead in trailheads)
		{
			validCoordinatesByHeight[0] = new List<Coordinate> { trailhead };

			for (var i = 0; i < 9; i++)
			{
				var nextHeight = i + 1;
				
				validCoordinatesByHeight[nextHeight] = new List<Coordinate>();
			
				foreach (var coordinate in validCoordinatesByHeight[i])
				{
					foreach (var deviation in NeighborDeviations)
					{
						var neighborCoordinate = coordinate.Add(deviation);

						if (heightByPosition.TryGetValue(neighborCoordinate, out var neighborHeight) &&
						    neighborHeight == nextHeight)
						{
							validCoordinatesByHeight[nextHeight].Add(neighborCoordinate);
						}
					}
				}
			}

			var reachablePeaks = validCoordinatesByHeight[9].GroupBy(coor => coor).Count();
			
			trailheadScore += reachablePeaks;
		}

		return trailheadScore;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var rows = input.Length;
		var cols = input[0].Length;

		var trailheads = new List<Coordinate>();
		var heightByPosition = new Dictionary<Coordinate, int>();
		
		for (var iRow = 0; iRow < rows; iRow++)
		{
			for (var iCol = 0; iCol < cols; iCol++)
			{
				var coordinate = new Coordinate(iCol, iRow);
				var height = int.Parse(input[iRow][iCol].ToString());

				heightByPosition[coordinate] = height;

				if (height == 0)
				{
					trailheads.Add(coordinate);
				}
			}
		}

		var validCoordinatesByHeight = new Dictionary<int, List<Coordinate>>();
		var trailheadScore = 0;
		
		foreach (var trailhead in trailheads)
		{
			validCoordinatesByHeight[0] = new List<Coordinate> { trailhead };

			for (var i = 0; i < 9; i++)
			{
				var nextHeight = i + 1;
				
				validCoordinatesByHeight[nextHeight] = new List<Coordinate>();
			
				foreach (var coordinate in validCoordinatesByHeight[i])
				{
					foreach (var deviation in NeighborDeviations)
					{
						var neighborCoordinate = coordinate.Add(deviation);

						if (heightByPosition.TryGetValue(neighborCoordinate, out var neighborHeight) &&
						    neighborHeight == nextHeight)
						{
							validCoordinatesByHeight[nextHeight].Add(neighborCoordinate);
						}
					}
				}
			}

			trailheadScore += validCoordinatesByHeight[9].Count;
		}

		return trailheadScore;
	}
}