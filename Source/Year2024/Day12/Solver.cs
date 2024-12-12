using Common.Interfaces;
using Common.Models;

namespace Year2024;

public class Day12Solver : IPuzzleSolver
{
	public string Title => "Garden Groups";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var surveyedCoordinates = new HashSet<Coordinate>();

		var areaByRegionId = new Dictionary<int, int>();
		var perimeterByRegionId = new Dictionary<int, int>();

		var nextRegionId = 0;

		var rows = input.Length;
		var cols = input[0].Length;

		for (var iRow = 0; iRow < rows; iRow++)
		{
			for (var iCol = 0; iCol < cols; iCol++)
			{
				var coordinate = new Coordinate(iCol, iRow);

				if (!surveyedCoordinates.Add(coordinate))
				{
					// We've already surveyed this region
					continue;
				}

				var plantType = input[iRow][iCol];

				areaByRegionId[nextRegionId] = 1;
				perimeterByRegionId[nextRegionId] = 0;

				var surveyedCoordinatesForCurrentRegion = new HashSet<Coordinate> { coordinate };

				var neighborCoordinatesToCheck = coordinate.GetOrthogonalNeighbors();

				while (neighborCoordinatesToCheck.Length > 0)
				{
					var nextNeighborCoordinates = new List<Coordinate>();

					foreach (var neighbor in neighborCoordinatesToCheck)
					{
						if (surveyedCoordinatesForCurrentRegion.Contains(neighbor))
						{
							continue;
						}

						if (neighbor.X < 0 || neighbor.X == cols ||
						    neighbor.Y < 0 || neighbor.Y == rows ||
						    !input[neighbor.Y][neighbor.X].Equals(plantType))
						{
							perimeterByRegionId[nextRegionId]++;
							continue;
						}
						
						nextNeighborCoordinates.AddRange(neighbor.GetOrthogonalNeighbors());

						surveyedCoordinatesForCurrentRegion.Add(neighbor);
						surveyedCoordinates.Add(neighbor);

						areaByRegionId[nextRegionId]++;
					}

					neighborCoordinatesToCheck = nextNeighborCoordinates.ToArray();
				}

				nextRegionId++;
			}
		}

		var totalFencingPrice = 0;

		for (var regionId = 0; regionId < nextRegionId; regionId++)
		{
			var area = areaByRegionId[regionId];
			var perimeter = perimeterByRegionId[regionId];
			var fencingPrice = area * perimeter;

			totalFencingPrice += fencingPrice;
			
			// Console.WriteLine($"{area} * {perimeter} = {fencingPrice}");
		}
		
		return totalFencingPrice;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}
}