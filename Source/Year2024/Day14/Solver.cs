using Common.Interfaces;
using Common.Models;

namespace Year2024;

public class Day14Solver : IPuzzleSolver
{
	public string Title => "Restroom Redoubt";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private const int Seconds = 100;

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var rows = isExampleInput ? 7 : 103;
		var cols = isExampleInput ? 11 : 101;

		var resultingRobotPositions = input
			.Select(line => CalculateResultingRobotPosition(line, rows, cols))
			.ToList();

		var rowToIgnore = rows / 2;
		var colToIgnore = cols / 2;
		
		var robotsInQ1 = resultingRobotPositions.Count(pos => pos.X < colToIgnore && pos.Y < rowToIgnore);
		var robotsInQ2 = resultingRobotPositions.Count(pos => pos.X > colToIgnore && pos.Y < rowToIgnore);
		var robotsInQ3 = resultingRobotPositions.Count(pos => pos.X < colToIgnore && pos.Y > rowToIgnore);
		var robotsInQ4 = resultingRobotPositions.Count(pos => pos.X > colToIgnore && pos.Y > rowToIgnore);

		return robotsInQ1 * robotsInQ2 * robotsInQ3 * robotsInQ4;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}

	private static Coordinate CalculateResultingRobotPosition(string robotConfig, int rows, int cols)
	{
		var config = robotConfig.Split(' ');

		var p0 = config[0].Split(',');
		var v = config[1].Split(',');

		var x0 = p0[0].Split('=')[1];
		var y0 = p0[1];
		
		var vx = v[0].Split('=')[1];
		var vy = v[1];
		
		var xCalculated = int.Parse(x0) + Seconds * int.Parse(vx);
		var yCalculated = int.Parse(y0) + Seconds * int.Parse(vy);
		
		var x = xCalculated < 0
			? (cols + (xCalculated % cols)) % cols
			: xCalculated % cols;

		var y = yCalculated < 0
			? (rows + (yCalculated % rows)) % rows
			: yCalculated % rows;
		
		// Console.WriteLine($"({x},{y})");

		return new Coordinate(x, y);
	}
}