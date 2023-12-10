using Common.Interfaces;

namespace Year2023;

public class Day10Solver : IPuzzleSolver
{
	public string Title => "Pipe Maze";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private const char StartingChar = 'S';

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
		return 0;
	}

	private static (int Row, int Col) GetStartingPosition(IReadOnlyList<string> map)
	{
		for (var i = 0; i < map.Count; i++)
		{
			if (map[i].Contains(StartingChar))
			{
				return (i, map[i].IndexOf(StartingChar));
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

	private static Direction GetDirectionToMoveFromTile(char tile, Direction directionToPreviousPipeTile)
	{
		var possibleDirections = ConnectedDirectionsByPipeTile[tile];

		return possibleDirections.Single(direction => direction != directionToPreviousPipeTile);
	}

	private enum Direction
	{
		North,
		South,
		East,
		West,
	};
}