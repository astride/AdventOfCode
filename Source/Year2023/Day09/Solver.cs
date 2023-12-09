using Common.Interfaces;

namespace Year2023;

public class Day09Solver : IPuzzleSolver
{
	public string Title => "Mirage Maintenance";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var nextValueSum = 0L;

		foreach (var line in input)
		{
			var extrapolatedSequence = line.Split(' ', StringSplitOptions.TrimEntries)
				.Select(long.Parse)
				.ToArray();

			var historyEndings = new List<long>();

			var extrapolatedNumbersCount = extrapolatedSequence.Length - 1;
			var zeroNumberCount = extrapolatedSequence.Length - extrapolatedNumbersCount;

			while (extrapolatedSequence.Any(number => number != 0))
			{
				historyEndings.Add(extrapolatedSequence[^1]);

				for (var i = extrapolatedSequence.Length - 1; i > zeroNumberCount; i--)
				{
					extrapolatedSequence[i] -= extrapolatedSequence[i - 1];
				}

				for (var i = 0; i <= zeroNumberCount; i++)
				{
					extrapolatedSequence[i] = 0;
				}
				
				extrapolatedNumbersCount--;
				zeroNumberCount++;
			}

			nextValueSum += historyEndings.Sum();
		}

		return nextValueSum;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}
}