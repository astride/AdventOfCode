using Common.Interfaces;
using Common.Models;

namespace Year2023;

public class Day21Solver : IPuzzleSolver
{
	public string Title => "Step Counter";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private const char Rock = '#';
	private const char StartingPoint = 'S';

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var steps = isExampleInput ? 6 : 64;

		#region Map neighboring garden plots

		var gardenPlotNeighborsByGardenPlot = new Dictionary<Coordinate, List<Coordinate>>();

		var rows = input.Length;
		var cols = input[0].Length;

		var startingPoint = Coordinate.Origin; // Dummy value, will be updated

		for (var iRow = 0; iRow < rows; iRow++)
		{
			for (var iCol = 0; iCol < cols; iCol++)
			{
				var position = input[iRow][iCol];

				if (position is Rock)
				{
					continue;
				}

				var currentNeighbors = new List<Coordinate>();

				var current = new Coordinate(iRow, iCol);
				var above = new Coordinate(iRow - 1, iCol);
				var leftOf = new Coordinate(iRow, iCol - 1);

				if (gardenPlotNeighborsByGardenPlot.TryGetValue(above, out var aboveNeighbors))
				{
					currentNeighbors.Add(above);
					aboveNeighbors.Add(current);
				}

				if (gardenPlotNeighborsByGardenPlot.TryGetValue(leftOf, out var leftOfNeighbors))
				{
					currentNeighbors.Add(leftOf);
					leftOfNeighbors.Add(current);
				}

				gardenPlotNeighborsByGardenPlot[new Coordinate(iRow, iCol)] = currentNeighbors;

				if (position is StartingPoint)
				{
					startingPoint = new Coordinate(iRow, iCol);
				}
			}
		}

		#endregion

		#region Simulate steps through garden

		var possiblePositions = new HashSet<Coordinate> { startingPoint };

		for (var step = 0; step < steps; step++)
		{
			var previousPossiblePositions = possiblePositions.ToList();

			possiblePositions = new HashSet<Coordinate>();

			foreach (var previousPossiblePosition in previousPossiblePositions)
			{
				if (gardenPlotNeighborsByGardenPlot.TryGetValue(previousPossiblePosition, out var neighbors))
				{
					foreach (var neighbor in neighbors)
					{
						possiblePositions.Add(neighbor);
					}
				}
			}
		}
		
		#endregion

		return possiblePositions.Count;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}
}