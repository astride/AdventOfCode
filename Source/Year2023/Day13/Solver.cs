using Common.Interfaces;

namespace Year2023;

public class Day13Solver : IPuzzleSolver
{
	public string Title => "Point of Incidence";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var patternNoteSum = 0;

		foreach (var pattern in GetPatterns(input))
		{
			var colCount = pattern[0].Length;
			var transposedPattern = new char[colCount][];

			var mirroredRowsWithPotential = new List<(int Upper, int Lower)>();

			for (var iRow = 0; iRow < pattern.Count; iRow++)
			{
				var mirroredRowsWithoutPotential = new List<(int Upper, int Lower)>();

				foreach (var mirroredRow in mirroredRowsWithPotential)
				{
					var mirroringRowIndex = mirroredRow.Upper - (iRow - mirroredRow.Lower);

					if (mirroringRowIndex >= 0 && pattern[iRow] != pattern[mirroringRowIndex])
					{
						mirroredRowsWithoutPotential.Add(mirroredRow);
					}
				}

				foreach (var mirroredRow in mirroredRowsWithoutPotential)
				{
					mirroredRowsWithPotential.Remove(mirroredRow);
				}
				
				if (iRow > 0 && pattern[iRow] == pattern[iRow - 1])
				{
					mirroredRowsWithPotential.Add((iRow - 1, iRow));
				}

				for (var iCol = 0; iCol < colCount; iCol++)
				{
					if (iRow == 0)
					{
						transposedPattern[iCol] = new char[pattern.Count];
					}

					transposedPattern[iCol][iRow] = pattern[iRow][iCol];
				}
			}

			if (mirroredRowsWithPotential.Count == 1)
			{
				patternNoteSum += 100 * mirroredRowsWithPotential.Single().Lower;
				continue;
			}

			var mirroredColsWithPotential = new List<(int Left, int Right)>();
			
			for (var iCol = 0; iCol < transposedPattern.Length; iCol++)
			{
				var mirroredColsWithoutPotential = new List<(int Upper, int Lower)>();

				foreach (var mirroredCol in mirroredColsWithPotential)
				{
					var mirroringColIndex = mirroredCol.Left - (iCol - mirroredCol.Right);

					if (mirroringColIndex >= 0 && !transposedPattern[iCol].SequenceEqual(transposedPattern[mirroringColIndex]))
					{
						mirroredColsWithoutPotential.Add(mirroredCol);
					}
				}

				foreach (var mirroredCol in mirroredColsWithoutPotential)
				{
					mirroredColsWithPotential.Remove(mirroredCol);
				}
				
				if (iCol > 0 && transposedPattern[iCol].SequenceEqual(transposedPattern[iCol - 1]))
				{
					mirroredColsWithPotential.Add((iCol - 1, iCol));
				}
			}

			if (mirroredColsWithPotential.Count == 1)
			{
				patternNoteSum += mirroredColsWithPotential.Single().Right;
			}
		}

		return patternNoteSum;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}

	private static IEnumerable<List<string>> GetPatterns(string[] input)
	{
		var pattern = new List<string>();

		foreach (var line in input)
		{
			if (string.IsNullOrEmpty(line))
			{
				yield return pattern;
				pattern.Clear();
			}
			else
			{
				pattern.Add(line);
			}
		}

		if (pattern.Any())
		{
			yield return pattern;
		}
	}
}