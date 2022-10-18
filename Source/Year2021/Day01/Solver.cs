using Common.Interfaces;

namespace Year2021;

public class Day01Solver : IPuzzleSolver
{
	public string Part1Solution { get; set; } = string.Empty;
	public string Part2Solution { get; set; } = string.Empty;

	public void SolvePuzzle(string[] rawInput)
	{
		var input = rawInput
			.Where(entry => !string.IsNullOrWhiteSpace(entry))
			.Select(int.Parse)
			.ToArray();

		Part1Solution = SolvePart1(input).ToString();
		Part2Solution = SolvePart2(input).ToString();
	}

	private static int SolvePart1(IReadOnlyList<int> sonarSweepReport)
	{
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

	private static int SolvePart2(IReadOnlyList<int> sonarSweepReport)
	{
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
