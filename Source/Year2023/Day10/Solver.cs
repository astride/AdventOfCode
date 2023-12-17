using Common.Interfaces;

namespace Year2023;

public class Day10Solver : IPuzzleSolver
{
	public string Title => "Pipe Maze";
	public bool UsePartSpecificExampleInputFiles => true;

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private const char StartingTile = 'S';

	private static readonly Dictionary<char, Direction[]> ConnectedDirectionsByPipeTile = new()
	{
		['|'] = new[] { Direction.North, Direction.South },
		['-'] = new[] { Direction.West, Direction.East },
		['L'] = new[] { Direction.North, Direction.East },
		['J'] = new[] { Direction.North, Direction.West },
		['7'] = new[] { Direction.South, Direction.West },
		['F'] = new[] { Direction.South, Direction.East },
		['.'] = Array.Empty<Direction>(),
	};

	private static readonly char[] PipeTilesConnectingSouthwards = { '|', '7', 'F' };
	private static readonly char[] PipeTilesConnectingNorthwards = { '|', 'L', 'J' };
	private static readonly char[] PipeTilesConnectingEastwards = { '-', 'L', 'F' };
	private static readonly char[] PipeTilesConnectingWestwards = { '-', 'J', '7' };

	private static readonly Dictionary<char, char> SymbolByPipeTile = new()
	{
		['|'] = '|',
		['-'] = '-',
		['L'] = '└',
		['J'] = '┘',
		['7'] = '┐',
		['F'] = '┌',
		['.'] = '.',
		['S'] = 'S',
	};

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var startingPosition = GetStartingPosition(input);

		var positionInPipe = GetPositionOneStepAwayFrom(startingPosition, input, out var directionToPreviousPipeTile);

		var loopLength = 1;

		while (positionInPipe != startingPosition)
		{
			var currentPipeTile = input[positionInPipe.Row][positionInPipe.Col];
			var directionToMoveInPipe = GetDirectionToMoveFromTile(currentPipeTile, directionToPreviousPipeTile);

			switch (directionToMoveInPipe)
			{
				case Direction.North:
					positionInPipe.Row--;
					break;
				case Direction.South:
					positionInPipe.Row++;
					break;
				case Direction.West:
					positionInPipe.Col--;
					break;
				case Direction.East:
					positionInPipe.Col++;
					break;
			}

			directionToPreviousPipeTile = directionToMoveInPipe switch
			{
				Direction.North => Direction.South,
				Direction.South => Direction.North,
				Direction.West => Direction.East,
				Direction.East => Direction.West,
				_ => throw new ArgumentException(),
			};

			loopLength++;
		}

		return loopLength / 2;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var startingPosition = GetStartingPosition(input);

		var currentPositionInPipe = GetPositionOneStepAwayFrom(startingPosition, input, out var directionToPreviousPipeTile);

		var mainLoopPipeTileByPosition = new Dictionary<(int Row, int Col), char>
		{
			[startingPosition] = StartingTile,
		};

		//  Map main loop pipe tiles
		while (currentPositionInPipe != startingPosition)
		{
			var currentPipeTile = input[currentPositionInPipe.Row][currentPositionInPipe.Col];
			
			if (currentPipeTile == 'S')
			{
				currentPipeTile = GetStartingPipeTile(input, currentPositionInPipe);
			}

			mainLoopPipeTileByPosition[currentPositionInPipe] = currentPipeTile;
			
			var directionToMoveInPipe = GetDirectionToMoveFromTile(currentPipeTile, directionToPreviousPipeTile);

			switch (directionToMoveInPipe)
			{
				case Direction.North:
					currentPositionInPipe.Row--;
					break;
				case Direction.South:
					currentPositionInPipe.Row++;
					break;
				case Direction.West:
					currentPositionInPipe.Col--;
					break;
				case Direction.East:
					currentPositionInPipe.Col++;
					break;
			}
			
			directionToPreviousPipeTile = directionToMoveInPipe switch
			{
				Direction.North => Direction.South,
				Direction.South => Direction.North,
				Direction.West => Direction.East,
				Direction.East => Direction.West,
				_ => throw new ArgumentException(),
			};
		}

		var enclosedTileCount = 0;
		
		// Calculate
		for (var iRow = 0; iRow < input.Length; iRow++)
		{
			var mainLoopPipeTilesInRow = mainLoopPipeTileByPosition.Keys.Where(key => key.Row == iRow).ToList();

			if (!mainLoopPipeTilesInRow.Any())
			{
				continue;
			}

			var iColMin = mainLoopPipeTilesInRow.Min(tile => tile.Col);
			var iColMax = mainLoopPipeTilesInRow.Max(tile => tile.Col);

			var countablePipeTilesInRow = 0;
			char previousBendBeginningtile = default;

			for (var iCol = iColMin; iCol <= iColMax; iCol++)
			{
				var currentTile = mainLoopPipeTileByPosition.TryGetValue((iRow, iCol), out var pipeTile)
					? pipeTile
					: '.';

				if (currentTile == 'L' || currentTile == 'F')
				{
					previousBendBeginningtile = currentTile;
					continue;
				}

				if (currentTile == '|')
				{
					countablePipeTilesInRow += 1;
					previousBendBeginningtile = default;
					continue;
				}

				if (currentTile == 'J')
				{
					countablePipeTilesInRow += previousBendBeginningtile == 'L' ? 2 : 1;
					previousBendBeginningtile = default;
				}

				if (currentTile == '7')
				{
					countablePipeTilesInRow += previousBendBeginningtile == 'F' ? 2 : 1;
					previousBendBeginningtile = default;
				}

				if (currentTile == '.')
				{
					if (countablePipeTilesInRow % 2 == 1)
					{
						enclosedTileCount++;
					}
					
					previousBendBeginningtile = default;
				}
			}
		}

		return enclosedTileCount;
	}

	private static (int Row, int Col) GetStartingPosition(IReadOnlyList<string> map)
	{
		for (var i = 0; i < map.Count; i++)
		{
			if (map[i].Contains(StartingTile))
			{
				return (i, map[i].IndexOf(StartingTile));
			}
		}

		return (-1, -1);
	}

	private static (int Row, int Col) GetPositionOneStepAwayFrom(
		(int Row, int Col) position,
		IReadOnlyList<string> map,
		out Direction directionToPreviousPipeTile)
	{
		// Try North
		if (position.Row > 0 &&
		    ConnectedDirectionsByPipeTile[map[position.Row - 1][position.Col]].Contains(Direction.South))
		{
			directionToPreviousPipeTile = Direction.South;
			return (position.Row - 1, position.Col);
		}
		
		// Try South
		if (position.Row < map.Count - 1 &&
		    ConnectedDirectionsByPipeTile[map[position.Row + 1][position.Col]].Contains(Direction.North))
		{
			directionToPreviousPipeTile = Direction.North;
			return (position.Row + 1, position.Col);
		}
		
		// Try West
		if (position.Col > 0 &&
		    ConnectedDirectionsByPipeTile[map[position.Row][position.Col - 1]].Contains(Direction.East))
		{
			directionToPreviousPipeTile = Direction.East;
			return (position.Row, position.Col - 1);
		}
		
		// Try East
		if (position.Col < map[0].Length - 1 &&
		    ConnectedDirectionsByPipeTile[map[position.Row][position.Col + 1]].Contains(Direction.West))
		{
			directionToPreviousPipeTile = Direction.West;
			return (position.Row, position.Col + 1);
		}

		directionToPreviousPipeTile = default;
		return (-1, -1);
	}

	private static char GetStartingPipeTile(string[] input, (int Row, int Col) position)
	{
		var isConnectedNorthwards = PipeTilesConnectingSouthwards.Contains(input[position.Row - 1][position.Col]);
		var isConnectedSouthwards = PipeTilesConnectingNorthwards.Contains(input[position.Row + 1][position.Col]);
		var isConnectedWestwards =  PipeTilesConnectingEastwards.Contains(input[position.Row][position.Col - 1]);
		var isConnectedEastwards =  PipeTilesConnectingWestwards.Contains(input[position.Row][position.Col + 1]);

		return isConnectedNorthwards && isConnectedSouthwards
			? '|'
			: isConnectedNorthwards && isConnectedWestwards
				? 'J'
				: isConnectedNorthwards && isConnectedEastwards
					? 'L'
					: isConnectedSouthwards && isConnectedWestwards
						? '7'
						: isConnectedSouthwards && isConnectedEastwards
							? 'F'
							: isConnectedWestwards && isConnectedEastwards
								? '-'
								: default;
	}

	private static Direction GetDirectionToMoveFromTile(char tile, Direction directionToPreviousPipeTile)
	{
		var possibleDirections = ConnectedDirectionsByPipeTile[tile];

		return possibleDirections.Single(direction => direction != directionToPreviousPipeTile);
	}

	private static void PrintNiceMap(
		string[] input,
		(int Row, int Col) startingPosition,
		Dictionary<(int Row, int Col), char> pipeTileByPosition)
	{
		for (var row = 0; row < input.Length; row++)
		{
			for (var col = 0; col < input[0].Length; col++)
			{
				if (pipeTileByPosition.TryGetValue((row, col), out var pipeTile))
				{
					Console.Write(SymbolByPipeTile[pipeTile]);
				}
				else if ((row, col) == startingPosition)
				{
					Console.Write(StartingTile);
				}
				else
				{
					Console.Write('.');
				}
			}
		
			Console.WriteLine();
		}
	}

	private enum Direction
	{
		North,
		South,
		East,
		West,
	};
}