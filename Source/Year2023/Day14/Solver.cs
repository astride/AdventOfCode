using Common.Interfaces;

namespace Year2023;

public class Day14Solver : IPuzzleSolver
{
	public string Title => string.Empty;

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var totalLoad = 0;

		var rows = input.Length;
		var cols = input[0].Length;

		var previousStayInPlaceRockIndexInCol = new int[cols];
		var rockCountSouthOfPreviousStayInPlaceRockInCol = new int[cols];

		Array.Fill(previousStayInPlaceRockIndexInCol, -1);
		Array.Fill(rockCountSouthOfPreviousStayInPlaceRockInCol, 0);
		
		for (var iRow = 0; iRow < rows; iRow++)
		{
			var isLastRow = iRow == rows - 1;

			for (var iCol = 0; iCol < cols; iCol++)
			{
				var item = input[iRow][iCol];

				if (item == 'O')
				{
					rockCountSouthOfPreviousStayInPlaceRockInCol[iCol]++;
				}

				if (item == '#' || isLastRow)
				{
					var distanceFromSouthEdge = rows - (1 + previousStayInPlaceRockIndexInCol[iCol]);

					for (var i = 0; i < rockCountSouthOfPreviousStayInPlaceRockInCol[iCol]; i++)
					{
						totalLoad += distanceFromSouthEdge - i;
					}

					rockCountSouthOfPreviousStayInPlaceRockInCol[iCol] = 0;
					previousStayInPlaceRockIndexInCol[iCol] = iRow;
				}
			}
		}

		return totalLoad;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}
}