using Common.Interfaces;
using Common.Models;

namespace Year2024;

public class Day06Solver : IPuzzleSolver
{
	public string Title => "Guard Gallivant";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private const char Guard = '^';
	private const char Obstacle = '#';

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var rows = input.Length;
		var cols = input[0].Length;
		
		var obstacleCoordinates = new List<Coordinate>();

		var guardCol = -1;
		var guardRow = -1;

		for (var iRow = 0; iRow < rows; iRow++)
		{
			for (var iCol = 0; iCol < cols; iCol++)
			{
				switch (input[iRow][iCol])
				{
					case Obstacle:
						obstacleCoordinates.Add(new Coordinate(iCol, iRow));
						break;
					case Guard:
						guardCol = iCol;
						guardRow = iRow;
						break;
				}
			}
		}

		var obstacleRowsByCol = obstacleCoordinates
			.GroupBy(coor => coor.X)
			.ToDictionary(group => group.Key, group => group.Select(coor => coor.Y).ToList());

		var obstacleColsByRow = obstacleCoordinates
			.GroupBy(coor => coor.Y)
			.ToDictionary(group => group.Key, group => group.Select(coor => coor.X).ToList());
		
		var visitedCoordinates = new HashSet<Coordinate> { new(guardCol, guardRow) };

		var guardDirection = Direction.North;

		while (true)
		{
			if (guardDirection is Direction.North)
			{
				var obstacleFound = obstacleRowsByCol.TryGetValue(guardCol, out var obstacleRows) &&
				                    obstacleRows.Any(row => row < guardRow);
				
				var stoppingRow = obstacleFound
					? obstacleRows.Where(row => row < guardRow).Max() + 1
					: 0;
				
				for (var row = guardRow - 1; row >= stoppingRow; row--)
				{
					visitedCoordinates.Add(new Coordinate(guardCol, row));
				}

				guardRow = stoppingRow;

				if (obstacleFound)
				{
					guardDirection = Direction.East;
					continue;
				}

				break;
			}

			if (guardDirection is Direction.South)
			{
				var obstacleFound = obstacleRowsByCol.TryGetValue(guardCol, out var obstacleRows) &&
				                    obstacleRows.Any(row => row > guardRow);

				var stoppingRow = obstacleFound
					? obstacleRows.Where(row => row > guardRow).Min() - 1
					: rows - 1;
				
				for (var row = guardRow + 1; row <= stoppingRow; row++)
				{
					visitedCoordinates.Add(new Coordinate(guardCol, row));
				}

				guardRow = stoppingRow;

				if (obstacleFound)
				{
					guardDirection = Direction.West;
					continue;
				}

				break;
			}

			if (guardDirection is Direction.East)
			{
				var obstacleFound = obstacleColsByRow.TryGetValue(guardRow, out var obstacleCols) &&
				                    obstacleCols.Any(col => col > guardCol);

				var stoppingCol = obstacleFound
					? obstacleCols.Where(col => col > guardCol).Min() - 1
					: cols - 1;

				for (var col = guardCol + 1; col <= stoppingCol; col++)
				{
					visitedCoordinates.Add(new Coordinate(col, guardRow));
				}

				guardCol = stoppingCol;

				if (obstacleFound)
				{
					guardDirection = Direction.South;
					continue;
				}
				
				break;
			}

			if (guardDirection is Direction.West)
			{
				var obstacleFound = obstacleColsByRow.TryGetValue(guardRow, out var obstacleCols) &&
				                    obstacleCols.Any(col => col < guardCol);

				var stoppingCol = obstacleFound
					? obstacleCols.Where(col => col < guardCol).Max() + 1
					: 0;

				for (var col = guardCol - 1; col >= stoppingCol; col--)
				{
					visitedCoordinates.Add(new Coordinate(col, guardRow));
				}

				guardCol = stoppingCol;

				if (obstacleFound)
				{
					guardDirection = Direction.North;
					continue;
				}

				break;
			}
		}
		
		return visitedCoordinates.Count;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var rows = input.Length;
		var cols = input[0].Length;
		
		var obstacleCoordinates = new List<Coordinate>();

		var initialGuardCol = -1;
		var initialGuardRow = -1;

		for (var iRow = 0; iRow < rows; iRow++)
		{
			for (var iCol = 0; iCol < cols; iCol++)
			{
				switch (input[iRow][iCol])
				{
					case Obstacle:
						obstacleCoordinates.Add(new Coordinate(iCol, iRow));
						break;
					case Guard:
						initialGuardCol = iCol;
						initialGuardRow = iRow;
						break;
				}
			}
		}

		var obstacleRowsByCol = obstacleCoordinates
			.GroupBy(coor => coor.X)
			.ToDictionary(group => group.Key, group => group.Select(coor => coor.Y).ToList());

		var obstacleColsByRow = obstacleCoordinates
			.GroupBy(coor => coor.Y)
			.ToDictionary(group => group.Key, group => group.Select(coor => coor.X).ToList());

		var hitInDirectionsByObstacleCoor = obstacleCoordinates.ToDictionary(coor => coor, _ => default(Direction));
		
		var newObstructionCoordinates = new HashSet<Coordinate>();

		var guardDirection = Direction.North;

		var guardCol = initialGuardCol;
		var guardRow = initialGuardRow;

		while (true)
		{
			if (guardDirection is Direction.North)
			{
				var obstacleFound = obstacleRowsByCol.TryGetValue(guardCol, out var obstacleRows) &&
				                    obstacleRows.Any(row => row < guardRow);
				
				var stoppingRow = obstacleFound
					? obstacleRows.Where(row => row < guardRow).Max() + 1
					: 0;
				
				for (var row = guardRow - 1; row > stoppingRow; row--)
				{
					if (!obstacleColsByRow.TryGetValue(row, out var obstacleCols) ||
					    !obstacleCols.Any(col => col > guardCol))
					{
						continue;
					}
					
					var closestObstacleColEast = obstacleCols.Where(obstacleCol => obstacleCol > guardCol).Min();

					if (hitInDirectionsByObstacleCoor[new Coordinate(closestObstacleColEast, row)]
					    .HasFlag(Direction.East))
					{
						newObstructionCoordinates.Add(new Coordinate(guardCol, row - 1));
					}
				}

				guardRow = stoppingRow;

				if (obstacleFound)
				{
					hitInDirectionsByObstacleCoor[new Coordinate(guardCol, guardRow - 1)] |= Direction.North;
					guardDirection = Direction.East;
					continue;
				}

				break;
			}

			if (guardDirection is Direction.South)
			{
				var obstacleFound = obstacleRowsByCol.TryGetValue(guardCol, out var obstacleRows) &&
				                    obstacleRows.Any(row => row > guardRow);

				var stoppingRow = obstacleFound
					? obstacleRows.Where(row => row > guardRow).Min() - 1
					: rows - 1;
				
				for (var row = guardRow + 1; row < stoppingRow; row++)
				{
					if (!obstacleColsByRow.TryGetValue(row, out var obstacleCols) ||
					    !obstacleCols.Any(col => col < guardCol))
					{
						continue;
					}
					
					var closestObstacleColWest = obstacleCols.Where(obstacleCol => obstacleCol < guardCol).Max();

					if (hitInDirectionsByObstacleCoor[new Coordinate(closestObstacleColWest, row)]
					    .HasFlag(Direction.West))
					{
						newObstructionCoordinates.Add(new Coordinate(guardCol, row + 1));
					}
				}

				guardRow = stoppingRow;

				if (obstacleFound)
				{
					hitInDirectionsByObstacleCoor[new Coordinate(guardCol, guardRow + 1)] |= Direction.South;
					guardDirection = Direction.West;
					continue;
				}

				break;
			}

			if (guardDirection is Direction.East)
			{
				var obstacleFound = obstacleColsByRow.TryGetValue(guardRow, out var obstacleCols) &&
				                    obstacleCols.Any(col => col > guardCol);

				var stoppingCol = obstacleFound
					? obstacleCols.Where(col => col > guardCol).Min() - 1
					: cols - 1;

				for (var col = guardCol + 1; col < stoppingCol; col++)
				{
					if (!obstacleRowsByCol.TryGetValue(col, out var obstacleRows) ||
					    !obstacleRows.Any(row => row > guardRow))
					{
						continue;
					}
					
					var closestObstacleRowSouth = obstacleRows.Where(obstacleRow => obstacleRow > guardRow).Min();

					if (hitInDirectionsByObstacleCoor[new Coordinate(col, closestObstacleRowSouth)]
					    .HasFlag(Direction.South))
					{
						newObstructionCoordinates.Add(new Coordinate(col + 1, guardRow));
					}
				}

				guardCol = stoppingCol;

				if (obstacleFound)
				{
					hitInDirectionsByObstacleCoor[new Coordinate(guardCol + 1, guardRow)] |= Direction.East;
					guardDirection = Direction.South;
					continue;
				}
				
				break;
			}

			if (guardDirection is Direction.West)
			{
				var obstacleFound = obstacleColsByRow.TryGetValue(guardRow, out var obstacleCols) &&
				                    obstacleCols.Any(col => col < guardCol);

				var stoppingCol = obstacleFound
					? obstacleCols.Where(col => col < guardCol).Max() + 1
					: 0;

				for (var col = guardCol - 1; col > stoppingCol; col--)
				{
					if (!obstacleRowsByCol.TryGetValue(col, out var obstacleRows) ||
					    !obstacleRows.Any(row => row < guardRow))
					{
						continue;
					}
					
					var closestObstacleRowNorth = obstacleRows.Where(obstacleRow => obstacleRow < guardRow).Max();

					if (hitInDirectionsByObstacleCoor[new Coordinate(col, closestObstacleRowNorth)]
					    .HasFlag(Direction.North))
					{
						newObstructionCoordinates.Add(new Coordinate(col - 1, guardRow));
					}

				}

				guardCol = stoppingCol;

				if (obstacleFound)
				{
					hitInDirectionsByObstacleCoor[new Coordinate(guardCol - 1, guardRow)] |= Direction.West;
					guardDirection = Direction.North;
					continue;
				}

				break;
			}
		}
		
		// TODO The loop could be created with obstacles that haven't already been visited

		foreach (var coor in newObstructionCoordinates)
		{
			Console.WriteLine($"({coor.X},{coor.Y})");
		}
		
		return newObstructionCoordinates.Count;
	}
}

[Flags]
internal enum Direction
{
	None = 0,
	North = 1,
	South = 2,
	East = 4,
	West = 8,
}
