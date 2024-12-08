using Common.Interfaces;
using Common.Models;

namespace Year2024;

public class Day08Solver : IPuzzleSolver
{
	public string Title => "Resonant Collinearity";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private const char Empty = '.';

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var rows = input.Length;
		var cols = input[0].Length;

		var coordinatesByAntennaFrequency = GetCoordinatesByAntennaFrequency(input, rows, cols);

		var antinodeCoors = new HashSet<Coordinate>();

		var antennaFrequencyCoordinates = coordinatesByAntennaFrequency.Values
			.Where(coordinates => coordinates.Count > 1);

		foreach (var coordinates in antennaFrequencyCoordinates)
		{
			for (var iFirst = 0; iFirst < coordinates.Count - 1; iFirst++)
			{
				var first = coordinates[iFirst];

				for (var iSecond = iFirst + 1; iSecond < coordinates.Count; iSecond++)
				{
					var second = coordinates[iSecond];

					var deviation = second.Subtract(first);
					
					var upperAntinode = first.Subtract(deviation);
					var lowerAntinode = second.Add(deviation);

					if (upperAntinode.X >= 0 && upperAntinode.X < cols &&
					    upperAntinode.Y >= 0 && upperAntinode.Y < rows)
					{
						antinodeCoors.Add(upperAntinode);
					}

					if (lowerAntinode.X >= 0 && lowerAntinode.X < cols &&
					    lowerAntinode.Y >= 0 && lowerAntinode.Y < rows)
					{
						antinodeCoors.Add(lowerAntinode);
					}
				}
			}
		}

		return antinodeCoors.Count;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var rows = input.Length;
		var cols = input[0].Length;

		var coordinatesByAntennaFrequency = GetCoordinatesByAntennaFrequency(input, rows, cols);

		var antinodeCoors = new HashSet<Coordinate>();
		
		var antennaFrequencyCoordinates = coordinatesByAntennaFrequency.Values
			.Where(coordinates => coordinates.Count > 1);
		
		foreach (var coordinates in antennaFrequencyCoordinates)
		{
			for (var iFirst = 0; iFirst < coordinates.Count - 1; iFirst++)
			{
				var first = coordinates[iFirst];

				for (var iSecond = iFirst + 1; iSecond < coordinates.Count; iSecond++)
				{
					var second = coordinates[iSecond];

					var deviation = second.Subtract(first);

					var iRow = first.Y;
					var iCol = first.X;

					while (iRow >= 0 && iRow < rows && iCol >= 0 && iCol < cols)
					{
						antinodeCoors.Add(new Coordinate(iCol, iRow));

						iRow -= deviation.Y;
						iCol -= deviation.X;
					}

					iRow = second.Y;
					iCol = second.X;

					while (iRow >= 0 && iRow < rows && iCol >= 0 && iCol < cols)
					{
						antinodeCoors.Add(new Coordinate(iCol, iRow));

						iRow += deviation.Y;
						iCol += deviation.X;
					}
				}
			}
		}

		return antinodeCoors.Count;
	}

	private static Dictionary<char, List<Coordinate>> GetCoordinatesByAntennaFrequency(string[] map, int rows, int cols)
	{
		var coordinatesByAntennaFrequency = new Dictionary<char, List<Coordinate>>();

		for (var iRow = 0; iRow < rows; iRow++)
		{
			for (var iCol = 0; iCol < cols; iCol++)
			{
				var item = map[iRow][iCol];

				if (item is Empty)
				{
					continue;
				}

				if (!coordinatesByAntennaFrequency.TryAdd(item, new List<Coordinate> { new(iCol, iRow) }))
				{
					coordinatesByAntennaFrequency[item].Add(new Coordinate(iCol, iRow));
				}
			}
		}

		return coordinatesByAntennaFrequency;
	}
}