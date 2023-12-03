using System.Text.RegularExpressions;
using Common.Interfaces;

namespace Year2023;

public class Day03Solver : IPuzzleSolver
{
	public string Title => "Gear Ratios";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private static readonly Regex NumberRegex = new(@"\d*");
	private static readonly Regex SymbolRegex = new(@"\W");
	private static readonly Regex NoDotRegex = new(@"[^\.]");

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var sum = 0;

		const int minX = 0;
		const int minY = 0;

		var maxX = input[0].Length - 1;
		var maxY = input.Length - 1;
		
		for (var i = 0; i < input.Length; i++)
		{
			var searchFromIndex = 0;

			var numbersInLine = NumberRegex.Matches(input[i])
				.Select(match => match.ToString())
				.Where(match => NoDotRegex.IsMatch(match))
				.ToList();

			foreach (var number in numbersInLine)
			{
				var numberStartIndex = input[i].IndexOf(number, searchFromIndex, StringComparison.Ordinal);
				var numberEndIndex = numberStartIndex + number.Length - 1;

				var adjacentMinX = numberStartIndex == minX ? minX : numberStartIndex - 1;
				var adjacentMaxX = numberEndIndex == maxX ? maxX : numberEndIndex + 1;

				var adjacentMinY = i == minY ? minY : i - 1;
				var adjacentMaxY = i == maxY ? maxY : i + 1;

				var adjacentRowLength = adjacentMaxX - adjacentMinX + 1;

				bool CellPrecedingNumberIsSymbol()
				{
					if (numberStartIndex == adjacentMinX)
					{
						return false;
					}

					var cellContent = input[i].Substring(adjacentMinX, 1);

					return ContainsSymbol(cellContent);
				}

				bool CellSucceedingNumberIsSymbol()
				{
					if (numberEndIndex == adjacentMaxX)
					{
						return false;
					}

					var cellContent = input[i].Substring(adjacentMaxX, 1);

					return ContainsSymbol(cellContent);
				}

				bool RowPrecedingNumberIsSymbol()
				{
					if (i == adjacentMinY)
					{
						return false;
					}

					var rowContent = input[adjacentMinY].Substring(adjacentMinX, adjacentRowLength);

					return ContainsSymbol(rowContent);
				}

				bool RowSucceedingNumberIsSymbol()
				{
					if (i == adjacentMaxY)
					{
						return false;
					}

					var rowContent = input[adjacentMaxY].Substring(adjacentMinX, adjacentRowLength);

					return ContainsSymbol(rowContent);
				}

				if (CellPrecedingNumberIsSymbol() ||
				    CellSucceedingNumberIsSymbol() ||
				    RowPrecedingNumberIsSymbol() ||
				    RowSucceedingNumberIsSymbol())
				{
					sum += int.Parse(number);
				}
				
				searchFromIndex = numberEndIndex + 1;
			}
		}

		return sum;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}

	private static bool ContainsSymbol(string text)
	{
		return NoDotRegex.IsMatch(text) && SymbolRegex.IsMatch(text);
	}
}