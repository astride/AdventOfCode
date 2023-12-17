using Common.Interfaces;

namespace Year2023;

public class Day11Solver : IPuzzleSolver
{
	public string Title => "Cosmic Expansion";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private const char Galaxy = '#';

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var galaxyPositions = new List<(int Row, int Col)>();

		var rowIndicesWithoutGalaxies = Enumerable.Range(0, input.Length).ToList();
		var colIndicesWithoutGalaxies = Enumerable.Range(0, input[0].Length).ToList();

		// Overview
		for (var iRow = 0; iRow < input.Length; iRow++)
		{
			for (var iCol = 0; iCol < input[0].Length; iCol++)
			{
				if (input[iRow][iCol] == Galaxy)
				{
					galaxyPositions.Add((iRow, iCol));
					rowIndicesWithoutGalaxies.Remove(iRow);
					colIndicesWithoutGalaxies.Remove(iCol);
				}
			}
		}
		
		var shortestPathSum = 0;

		// Calculation
		for (var iFirstGalaxy = 0; iFirstGalaxy < galaxyPositions.Count; iFirstGalaxy++)
		{
			for (var iSecondGalaxy = iFirstGalaxy + 1; iSecondGalaxy < galaxyPositions.Count; iSecondGalaxy++)
			{
				var firstGalaxyPosition = galaxyPositions[iFirstGalaxy];
				var secondGalaxyPosition = galaxyPositions[iSecondGalaxy];

				var iRowMin = Math.Min(firstGalaxyPosition.Row, secondGalaxyPosition.Row);
				var iRowMax = Math.Max(firstGalaxyPosition.Row, secondGalaxyPosition.Row);
				
				var iColMin = Math.Min(firstGalaxyPosition.Col, secondGalaxyPosition.Col);
				var iColMax = Math.Max(firstGalaxyPosition.Col, secondGalaxyPosition.Col);

				var originalDistance = (iRowMax - iRowMin) + (iColMax - iColMin);

				var extraDistance = rowIndicesWithoutGalaxies.Count(i => i > iRowMin && i < iRowMax) +
				                    colIndicesWithoutGalaxies.Count(i => i > iColMin && i < iColMax);

				shortestPathSum += originalDistance + extraDistance;
			}
		}

		return shortestPathSum;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var galaxyPositions = new List<(int Row, int Col)>();

		var rowIndicesWithoutGalaxies = Enumerable.Range(0, input.Length).ToList();
		var colIndicesWithoutGalaxies = Enumerable.Range(0, input[0].Length).ToList();

		// Overview
		for (var iRow = 0; iRow < input.Length; iRow++)
		{
			for (var iCol = 0; iCol < input[0].Length; iCol++)
			{
				if (input[iRow][iCol] == Galaxy)
				{
					galaxyPositions.Add((iRow, iCol));
					rowIndicesWithoutGalaxies.Remove(iRow);
					colIndicesWithoutGalaxies.Remove(iCol);
				}
			}
		}
		
		var shortestOriginalPathSum = 0;
		var rowAndColumnCountToExpand = 0L;

		// Calculation
		for (var iFirstGalaxy = 0; iFirstGalaxy < galaxyPositions.Count; iFirstGalaxy++)
		{
			for (var iSecondGalaxy = iFirstGalaxy + 1; iSecondGalaxy < galaxyPositions.Count; iSecondGalaxy++)
			{
				var firstGalaxyPosition = galaxyPositions[iFirstGalaxy];
				var secondGalaxyPosition = galaxyPositions[iSecondGalaxy];

				var iRowMin = Math.Min(firstGalaxyPosition.Row, secondGalaxyPosition.Row);
				var iRowMax = Math.Max(firstGalaxyPosition.Row, secondGalaxyPosition.Row);
				
				var iColMin = Math.Min(firstGalaxyPosition.Col, secondGalaxyPosition.Col);
				var iColMax = Math.Max(firstGalaxyPosition.Col, secondGalaxyPosition.Col);

				var originalDistance = (iRowMax - iRowMin) + (iColMax - iColMin);

				shortestOriginalPathSum += originalDistance;

				rowAndColumnCountToExpand += rowIndicesWithoutGalaxies.Count(i => i > iRowMin && i < iRowMax) +
				                             colIndicesWithoutGalaxies.Count(i => i > iColMin && i < iColMax);
			}
		}

		return shortestOriginalPathSum + (1000000 - 1) * rowAndColumnCountToExpand;
	}
}