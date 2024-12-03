using Common.Interfaces;

namespace Year2024;

public class Day02Solver : IPuzzleSolver
{
	public string Title => "Red-Nosed Reports";
	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }
	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var safeReportCount = input
			.Where(IsSafeReport)
			.Count();

		return safeReportCount;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var safeReportCount = input
			.Where(IsSafeReportWithProblemDampener)
			.Count();

		return safeReportCount;
	}

	private static bool IsSafeReport(string report)
	{
		var levels = report.Split(" ");

		return IsSafeReport(levels);
	}

	private static bool IsSafeReport(string[] levels)
	{
		if (levels.Length < 2)
		{
			return true;
		}

		var firstLevelDifference = int.Parse(levels[1]) - int.Parse(levels[0]);

		var (lowerLimit, upperLimit) = firstLevelDifference < 0 ? (-3, -1) : (1, 3);

		if (firstLevelDifference < lowerLimit || firstLevelDifference > upperLimit)
		{
			return false;
		}

		for (var i = 2; i < levels.Length; i++)
		{
			var levelDifference = int.Parse(levels[i]) - int.Parse(levels[i - 1]);

			if (levelDifference < lowerLimit || levelDifference > upperLimit)
			{
				return false;
			}
		}

		return true;
	}
	
	private static bool IsSafeReportWithProblemDampener(string report)
	{
		Console.Write(report + ": ");
		var levels = report.Split(" ");

		if (levels.Length < 2)
		{
			return true;
		}

		if (IsSafeReport(levels))
		{
			Console.WriteLine("Safe");
			Console.WriteLine();
			return true;
		}

		if (IsSafeReport(levels[1..]))
		{
			Console.WriteLine("This report is safe without the first level");
			Console.WriteLine();
			return true;
		}

		if (IsSafeReport(levels[..^1]))
		{
			Console.WriteLine("This report is safe without the last level");
			Console.WriteLine();
			return true;
		}

		var firstLevelDifference = int.Parse(levels[1]) - int.Parse(levels[0]);

		var (lowerLimit, upperLimit) = firstLevelDifference < 0 ? (-3, -1) : (1, 3);

		if (firstLevelDifference < lowerLimit || firstLevelDifference > upperLimit)
		{
			var subReport = levels[..1].Concat(levels[2..]).ToArray();

			if (IsSafeReport(subReport))
			{
				Console.Write($"This report is safe without level {levels[1]} (index 1): {string.Join(" ", subReport)}");
				Console.WriteLine();
				return true;
			}

			Console.Write("NOT SAFE");
			Console.WriteLine();
			return false;
		}

		if (levels.Length < 3)
		{
			return true;
		}

		for (var i = 2; i < levels.Length; i++)
		{
			var levelDifference = int.Parse(levels[i]) - int.Parse(levels[i - 1]);

			if (levelDifference >= lowerLimit && levelDifference <= upperLimit)
			{
				continue;
			}

			var subReport = levels[..(i - 1)].Concat(levels[i..]).ToArray();
			
			if (IsSafeReport(subReport))
			{
				Console.Write($"This report is safe without level {levels[i - 1]} (index {i - 1}): {string.Join(" ", subReport)}");
				Console.WriteLine();
				return true;
			}

			var subReport2 = levels[..i].Concat(levels[(i + 1)..]).ToArray();

			if (IsSafeReport(subReport2))
			{
				Console.Write($"This report is safe without level {levels[i]} (index {i}): {string.Join(" ", subReport2)}");
				Console.WriteLine();
				return true;
			}

			Console.Write("NOT SAFE");
			Console.WriteLine();
			return false;
		}

		Console.WriteLine("Safe");
		Console.WriteLine();
		return true;
	}
}