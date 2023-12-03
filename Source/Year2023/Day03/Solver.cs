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
	private static readonly Regex GearSymbolRegex = new(@"\*");

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
		const int partNumberCountForGear = 2;

		var adjacentNumbersOfGearSymbolCoors = new Dictionary<(int X, int Y), List<string>>();

		// Find coors of all gear symbols
		for (var i = 0; i < input.Length; i++)
		{
			for (var j = 0; j < input[0].Length; j++)
			{
				if (GearSymbolRegex.IsMatch(input[i].Substring(j, 1)))
				{
					adjacentNumbersOfGearSymbolCoors[(i, j)] = new List<string>();
				}
			}
		}
		
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

				bool CellPrecedingNumberIsGearSymbol(out (int X, int Y) gearSymbolCoordinate)
				{
					if (numberStartIndex == adjacentMinX)
					{
						gearSymbolCoordinate = default;
						return false;
					}

					var cellContent = input[i].Substring(adjacentMinX, 1);

					if (!GearSymbolRegex.IsMatch(cellContent))
					{
						gearSymbolCoordinate = default;
						return false;
					}

					gearSymbolCoordinate = (i, adjacentMinX);
					return true;
				}

				bool CellSucceedingNumberIsGearSymbol(out (int X, int Y) gearSymbolCoordinate)
				{
					if (numberEndIndex == adjacentMaxX)
					{
						gearSymbolCoordinate = default;
						return false;
					}

					var cellContent = input[i].Substring(adjacentMaxX, 1);

					if (!GearSymbolRegex.IsMatch(cellContent))
					{
						gearSymbolCoordinate = default;
						return false;
					}

					gearSymbolCoordinate = (i, adjacentMaxX);
					return true;
				}

				List<(int X, int Y)> GetGearSymbolCoordinatesInRow(int rowIndex)
				{
					var coors = input[rowIndex]
						.Select((character, index) => (Index: index, Character: character))
						.Where(indexAndChar => indexAndChar.Index >= adjacentMinX && indexAndChar.Index <= adjacentMaxX)
						.Where(indexAndChar => GearSymbolRegex.IsMatch(indexAndChar.Character.ToString()))
						.Select(indexAndCharOfGearSymbol => (rowIndex, indexAndCharOfGearSymbol.Index))
						.ToList();

					return coors;
				}

				bool RowPrecedingNumberContainsGearSymbol(out List<(int X, int Y)> gearSymbolCoordinates)
				{
					if (i == adjacentMinY)
					{
						gearSymbolCoordinates = new List<(int X, int Y)>();
						return false;
					}

					var rowContent = input[adjacentMinY].Substring(adjacentMinX, adjacentRowLength);

					if (!GearSymbolRegex.IsMatch(rowContent))
					{
						gearSymbolCoordinates = new List<(int X, int Y)>();
						return false;
					}

					gearSymbolCoordinates = GetGearSymbolCoordinatesInRow(adjacentMinY);

					return true;
				}

				bool RowSucceedingNumberContainsGearSymbol(out List<(int X, int Y)> gearSymbolCoordinates)
				{
					if (i == adjacentMaxY)
					{
						gearSymbolCoordinates = new List<(int X, int Y)>();
						return false;
					}

					var rowContent = input[adjacentMaxY].Substring(adjacentMinX, adjacentRowLength);

					if (!GearSymbolRegex.IsMatch(rowContent))
					{
						gearSymbolCoordinates = new List<(int X, int Y)>();
						return false;
					}

					gearSymbolCoordinates = GetGearSymbolCoordinatesInRow(adjacentMaxY);

					return true;
				}

				if (CellPrecedingNumberIsGearSymbol(out var gearSymbolBeforeNumber))
				{
					adjacentNumbersOfGearSymbolCoors[gearSymbolBeforeNumber].Add(number);
				}

				if (CellSucceedingNumberIsGearSymbol(out var gearSymbolAfterNumber))
				{
					adjacentNumbersOfGearSymbolCoors[gearSymbolAfterNumber].Add(number);
				}

				if (RowPrecedingNumberContainsGearSymbol(out var gearSymbolsAboveNumber))
				{
					foreach (var coor in gearSymbolsAboveNumber)
					{
						adjacentNumbersOfGearSymbolCoors[coor].Add(number);
					}
				}

				if (RowSucceedingNumberContainsGearSymbol(out var gearSymbolsBelowNumber))
				{
					foreach (var coor in gearSymbolsBelowNumber)
					{
						adjacentNumbersOfGearSymbolCoors[coor].Add(number);
					}
				}

				searchFromIndex = numberEndIndex + 1;
			}
		}
		
		var gearRatioSum = adjacentNumbersOfGearSymbolCoors.Values
			.Where(adjacentNumbers => adjacentNumbers.Count == partNumberCountForGear)
			.Sum(adjacentNumbers => int.Parse(adjacentNumbers[0]) * int.Parse(adjacentNumbers[1]));

		return gearRatioSum;
	}

	private static bool ContainsSymbol(string text)
	{
		return NoDotRegex.IsMatch(text) && SymbolRegex.IsMatch(text);
	}
}