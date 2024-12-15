using Common.Interfaces;
using Common.Models;

namespace Year2024;

public class Day15Solver : IPuzzleSolver
{
	public string Title => "Warehouse Woes";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private const char Wall = '#';
	private const char Box = 'O';
	private const char Robot = '@';
	private const char Empty = '.';

	private const char Up = '^';
	private const char Down = 'v';
	private const char Left = '<';
	private const char Right = '>';

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var map = input.TakeWhile(line => !string.IsNullOrEmpty(line)).ToList();
		var moves = string.Join(string.Empty, input.Skip(map.Count).SkipWhile(string.IsNullOrEmpty));

		var rows = map.Count;
		var cols = map[0].Length;

		var walls = new List<Coordinate>();
		var boxes = new List<Coordinate>();

		var robot = Coordinate.Origin;

		for (var iRow = 0; iRow < rows; iRow++)
		{
			for (var iCol = 0; iCol < cols; iCol++)
			{
				var coor = new Coordinate(iCol, iRow);

				switch (map[iRow][iCol])
				{
					case Wall:
						walls.Add(coor);
						break;
					case Box:
						boxes.Add(coor);
						break;
					case Robot:
						robot = coor;
						break;
				}
			}
		}

		foreach (var move in moves)
		{
			Func<Coordinate, Coordinate> movingPattern = move switch
			{
				Up => coor => coor.Subtract(0, 1),
				Down => coor => coor.Add(0, 1),
				Left => coor => coor.Subtract(1, 0),
				Right => coor => coor.Add(1, 0),
				_ => throw new ArgumentException("Invalid move"),
			};

			robot = TryMove(movingPattern);
		}
		
		LogMap();

		var gpsCoordinateSum = boxes
			.Select(box => 100 * box.Y + box.X)
			.Sum();
		
		return gpsCoordinateSum;
		
		Coordinate TryMove(Func<Coordinate, Coordinate> move)
		{
			var robotTarget = move(robot);

			if (walls.Contains(robotTarget)) return robot;
			if (!boxes.Contains(robotTarget)) return robotTarget;

			var boxTarget = robotTarget;

			while (boxes.Contains(boxTarget))
			{
				boxTarget = move(boxTarget);
			}

			if (walls.Contains(boxTarget)) return robot;
			
			boxes.Remove(robotTarget);
			boxes.Add(boxTarget);

			return robotTarget;
		}

		void LogMap()
		{
			for (var iRow = 0; iRow < rows; iRow++)
			{
				for (var iCol = 0; iCol < cols; iCol++)
				{
					var coor = new Coordinate(iCol, iRow);

					if (walls.Contains(coor)) Console.Write(Wall);
					else if (boxes.Contains(coor)) Console.Write(Box);
					else if (robot.Equals(coor)) Console.Write(Robot);
					else Console.Write(Empty);
				}

				Console.Write(Environment.NewLine);
			}
			
			Console.WriteLine();
		}
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}
}
