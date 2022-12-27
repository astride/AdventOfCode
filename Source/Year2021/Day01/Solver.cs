using Common.Interfaces;

namespace Year2021;

public class Day01Solver : IPuzzleSolver
{
	public string Title => "Sonar Sweep";
	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input)
	{
		var sonarSweepReport = GetSonarSweepReport(input);
		
		var prevDepth = sonarSweepReport[0];
		var increaseCount = 0;

		foreach (var depth in sonarSweepReport.Skip(1))
		{
			if (depth > prevDepth)
			{
				increaseCount++;
			}

			prevDepth = depth;
		}

		return increaseCount;
	}

	public object GetPart2Solution(string[] input)
	{
		var sonarSweepReport = GetSonarSweepReport(input);
		
		var prevDepthSum = sonarSweepReport.GetDepthSumForWindowAt(0);
		var increaseCount = 0;

		for (var i = 1; i <= sonarSweepReport.GetLastWindowIndex(); i++)
		{
			var depthSum = sonarSweepReport.GetDepthSumForWindowAt(i);

			if (depthSum > prevDepthSum)
			{
				increaseCount++;
			}

			prevDepthSum = depthSum;
		}

		return increaseCount;
	}

	private static IReadOnlyList<int> GetSonarSweepReport(string[] input)
	{
		return input
			.Where(entry => !string.IsNullOrWhiteSpace(entry))
			.Select(int.Parse)
			.ToArray();
	}
}

internal static class Day01Part2Helper
{
	private const int WindowSize = 3;

	public static int GetLastWindowIndex(this IReadOnlyList<int> report)
	{
		return report.Count - WindowSize;
	}

	public static int GetDepthSumForWindowAt(this IReadOnlyList<int> report, int index)
	{
		return report.Skip(index).Take(WindowSize).Sum();
	}
}
