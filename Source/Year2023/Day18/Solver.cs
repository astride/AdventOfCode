using Common.Interfaces;

namespace Year2023;

public class Day18Solver : IPuzzleSolver
{
	public string Title => "Lavaduct Lagoon";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }
	
	private static readonly Dictionary<char, (int RowStep, int ColStep)> StepByDirection = new()
	{
		['U'] = (-1, 0),
		['D'] = (1, 0),
		['R'] = (0, 1),
		['L'] = (0, -1),
	};

	private static readonly Dictionary<char, char> TrenchDirectionByDiggingInstruction = new()
	{
		['U'] = '|',
		['D'] = '|',
		['R'] = '-',
		['L'] = '-',
	};
	
	private static readonly Dictionary<(char PreviousDirection, char CurrentDirection), char> CornerCharByDirectionChange = new()
	{
		[('U', 'R')] = '┌',
		[('U', 'L')] = '┐',
		[('D', 'R')] = '└',
		[('D', 'L')] = '┘',
		[('R', 'U')] = '┘',
		[('R', 'D')] = '┐',
		[('L', 'U')] = '└',
		[('L', 'D')] = '┌',
	};

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var currentTrenchRow = 0;
		var currentTrenchCol = 0;

		var previousDiggingDirection = input[^1][0];

		var trenchPositions = new Dictionary<(int Row, int Col), char>
		{
			[(currentTrenchRow, currentTrenchCol)] = previousDiggingDirection,
		};

		// Dig trench
		foreach (var diggingInstruction in input)
		{
			var diggingLength = int.Parse(diggingInstruction.Split(' ')[1]);

			var diggingDirection = diggingInstruction[0];

			var trenchDirection = TrenchDirectionByDiggingInstruction[diggingDirection];

			trenchPositions[(currentTrenchRow, currentTrenchCol)] =
				CornerCharByDirectionChange[(previousDiggingDirection, diggingDirection)];

			var step = StepByDirection[diggingDirection];
			
			(int Row, int Col) finalTrenchPosition =
				(currentTrenchRow + diggingLength * step.RowStep, currentTrenchCol + diggingLength * step.ColStep);

			while (currentTrenchRow != finalTrenchPosition.Row || currentTrenchCol != finalTrenchPosition.Col)
			{
				currentTrenchRow += step.RowStep;
				currentTrenchCol += step.ColStep;

				if (!trenchPositions.ContainsKey((currentTrenchRow, currentTrenchCol)))
				{
					trenchPositions[(currentTrenchRow, currentTrenchCol)] = trenchDirection;
				}
			}

			previousDiggingDirection = diggingDirection;
		}
		
		var iRowMin = trenchPositions.Keys.Min(position => position.Row);
		var iRowMax = trenchPositions.Keys.Max(position => position.Row);

		// PrintTrenchMap(trenchPositions, iRowMin, iRowMax);

		var interiorTrenchCubicMeterCount = 0;
		
		// Calculate
		for (var iRow = 1 + iRowMin; iRow < iRowMax; iRow++)
		{
			var dugTrenchColsInRow = trenchPositions.Keys
				.Where(position => position.Row == iRow)
				.Select(position => position.Col)
				.ToList();
			
			var iColMinInRow = dugTrenchColsInRow.Min();
			var iColMaxInRow = dugTrenchColsInRow.Max();

			var countableTrenchPositionsInRow = 0;
			char previousTrenchBendBeginningChar = default;

			for (var iCol = iColMinInRow; iCol < iColMaxInRow; iCol++)
			{
				if (!trenchPositions.TryGetValue((iRow, iCol), out var currentPositionChar))
				{
					currentPositionChar = '.';
				}
				
				if (currentPositionChar == '└' || currentPositionChar == '┌')
				{
					previousTrenchBendBeginningChar = currentPositionChar;
					continue;
				}

				if (currentPositionChar == '|')
				{
					countableTrenchPositionsInRow += 1;
					previousTrenchBendBeginningChar = default;
					continue;
				}

				if (currentPositionChar == '┘')
				{
					countableTrenchPositionsInRow += previousTrenchBendBeginningChar == '└' ? 2 : 1;
					previousTrenchBendBeginningChar = default;
				}

				if (currentPositionChar == '┐')
				{
					countableTrenchPositionsInRow += previousTrenchBendBeginningChar == '┌' ? 2 : 1;
					previousTrenchBendBeginningChar = default;
				}

				if (currentPositionChar == '.')
				{
					if (countableTrenchPositionsInRow % 2 == 1)
					{
						interiorTrenchCubicMeterCount++;
					}
					
					previousTrenchBendBeginningChar = default;
				}
			}
		}

		var cubicMetersOfLava = interiorTrenchCubicMeterCount + trenchPositions.Count;

		return cubicMetersOfLava;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}

	private static void PrintTrenchMap(Dictionary<(int Row, int Col), char> trenchPositions, int iRowMin, int iRowMax)
	{
		var iColMin = trenchPositions.Keys.Min(position => position.Col);
		var iColMax = trenchPositions.Keys.Max(position => position.Col);

		for (var iRow = iRowMin; iRow <= iRowMax; iRow++)
		{
			for (var iCol = iColMin; iCol <= iColMax; iCol++)
			{
				var trenchIsDugAtPosition = trenchPositions.TryGetValue((iRow, iCol), out var trenchPositionChar);
				
				Console.Write(trenchIsDugAtPosition ? trenchPositionChar : '.');
			}
			
			Console.WriteLine();
		}
	}
}