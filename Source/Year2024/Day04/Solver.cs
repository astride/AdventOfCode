using Common.Interfaces;

namespace Year2024;

public class Day04Solver : IPuzzleSolver
{
	public string Title => "Ceres Search";
	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private const char X = 'X';
	private const char M = 'M';
	private const char A = 'A';
	private const char S = 'S';

	private const int XMAS = 4;

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var rows = input.Length;
		var cols = input[0].Length;

		var xmasCount = 0;

		for (var iRow = 0; iRow < rows; iRow++)
		{
			for (var iCol = 0; iCol < cols; iCol++)
			{
				if (input[iRow][iCol] != X)
				{
					continue;
				}

				xmasCount += new[]
				{
					CanCheckNorth() && IsMatchNorth(),
					CanCheckSouth() && IsMatchSouth(),
					CanCheckEast() && IsMatchEast(),
					CanCheckWest() && IsMatchWest(),
					CanCheckNorthEast() && IsMatchNorthEast(),
					CanCheckNorthWest() && IsMatchNorthWest(),
					CanCheckSouthEast() && IsMatchSouthEast(),
					CanCheckSouthWest() && IsMatchSouthWest(),
				}
					.Count(isMatch => isMatch);

				continue;
				
				bool CanCheckNorth() => iRow >= XMAS - 1;
				bool CanCheckSouth() => iRow <= rows - XMAS;
				bool CanCheckEast() => iCol <= cols - XMAS;
				bool CanCheckWest() => iCol >= XMAS - 1;
				bool CanCheckNorthEast() => CanCheckNorth() && CanCheckEast();
				bool CanCheckNorthWest() => CanCheckNorth() && CanCheckWest();
				bool CanCheckSouthEast() => CanCheckSouth() && CanCheckEast();
				bool CanCheckSouthWest() => CanCheckSouth() && CanCheckWest();

				bool IsMatchNorth() => IsM(-1, 0) && IsA(-2, 0) && IsS(-3, 0);
				bool IsMatchSouth() => IsM(+1, 0) && IsA(+2, 0) && IsS(+3, 0);
				bool IsMatchEast() => IsM(0, +1) && IsA(0, +2) && IsS(0, +3);
				bool IsMatchWest() => IsM(0, -1) && IsA(0, -2) && IsS(0, -3);
				bool IsMatchNorthEast() => IsM(-1, +1) && IsA(-2, +2) && IsS(-3, +3);
				bool IsMatchNorthWest() => IsM(-1, -1) && IsA(-2, -2) && IsS(-3, -3);
				bool IsMatchSouthEast() => IsM(+1, +1) && IsA(+2, +2) && IsS(+3, +3);
				bool IsMatchSouthWest() => IsM(+1, -1) && IsA(+2, -2) && IsS(+3, -3);

				bool IsM(int rowShift, int colShift) => IsMatch(M, rowShift, colShift);
				bool IsA(int rowShift, int colShift) => IsMatch(A, rowShift, colShift);
				bool IsS(int rowShift, int colShift) => IsMatch(S, rowShift, colShift);
				
				bool IsMatch(char ch, int rowShift, int colShift) => input[iRow + rowShift][iCol + colShift] == ch;
			}
		}
		
		return xmasCount;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var rows = input.Length;
		var cols = input[0].Length;

		var masCrossCount = 0;

		// Looking for the A, which cannot be along the border
		for (var iRow = 1; iRow < rows - 1; iRow++)
		{
			for (var iCol = 1; iCol < cols - 1; iCol++)
			{
				if (input[iRow][iCol] != A)
				{
					continue;
				}
				
				if (IsM(-1, -1) && IsM(-1, +1) && IsS(+1, +1) && IsS(+1, -1) ||
				    IsS(-1, -1) && IsM(-1, +1) && IsM(+1, +1) && IsS(+1, -1) ||
				    IsS(-1, -1) && IsS(-1, +1) && IsM(+1, +1) && IsM(+1, -1) ||
				    IsM(-1, -1) && IsS(-1, +1) && IsS(+1, +1) && IsM(+1, -1))
				{
					masCrossCount++;
				}

				continue;

				bool IsM(int rowShift, int colShift) => IsMatch(M, rowShift, colShift);
				bool IsS(int rowShift, int colShift) => IsMatch(S, rowShift, colShift);
				
				bool IsMatch(char ch, int rowShift, int colShift) => input[iRow + rowShift][iCol + colShift] == ch;
			}
		}
		
		return masCrossCount;
	}
}