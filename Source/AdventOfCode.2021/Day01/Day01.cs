using AdventOfCode.Common;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day01 : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var input = rawInput
				.Where(entry => !string.IsNullOrWhiteSpace(entry))
				.Select(entry => int.Parse(entry))
				.ToArray();

			Part1Solution = SolvePart1(input).ToString();
			Part2Solution = SolvePart2(input).ToString();
		}

		private int SolvePart1(int[] sonarSweepReport)
		{
			int prevDepth = sonarSweepReport.First();
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

		private int SolvePart2(int[] sonarSweepReport)
		{
			int depthSum;
			int prevDepthSum = sonarSweepReport.GetDepthSumForWindowAt(0);
			var increaseCount = 0;

			for (var i = 1; i <= sonarSweepReport.GetLastWindowIndex(); i++)
			{
				depthSum = sonarSweepReport.GetDepthSumForWindowAt(i);

				if (depthSum > prevDepthSum)
				{
					increaseCount++;
				}

				prevDepthSum = depthSum;
			}

			return increaseCount;
		}
	}

	static class Day01Part2Helper
	{
		private const int WindowSize = 3;

		public static int GetLastWindowIndex(this int[] report)
		{
			return report.Length - WindowSize;
		}

		public static int GetDepthSumForWindowAt(this int[] report, int index)
		{
			return report.Skip(index).Take(WindowSize).Sum();
		}
	}
}
