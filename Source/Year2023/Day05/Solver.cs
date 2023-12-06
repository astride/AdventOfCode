using Common.Helpers;
using Common.Interfaces;

namespace Year2023;

public class Day05Solver : IPuzzleSolver
{
	public string Title => "If You Give A Seed A Fertilizer";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var sourceValues = RegexHelper.GetAllNumbersAsLong(input[0]).ToArray();

		var destinationValues = new long[sourceValues.Length];

		var remainingInput = input.Skip(2).ToList();

		while (remainingInput.Any())
		{
			var map = remainingInput
				.TakeWhile(line => !string.IsNullOrEmpty(line))
				.Skip(1)
				.Select(line => line.Split(' '))
				.Select(line => (DestinationRangeStart: long.Parse(line[0]), SourceRangeStart: long.Parse(line[1]), Range: long.Parse(line[2])))
				.OrderByDescending(rangeLine => rangeLine.SourceRangeStart)
				.ToList();

			remainingInput = remainingInput.Skip(map.Count + 2).ToList();

			var minSourceRangeStart = map.Min(rangeLine => rangeLine.SourceRangeStart);

			for (var i = 0; i < sourceValues.Length; i++)
			{
				if (sourceValues[i] < minSourceRangeStart)
				{
					destinationValues[i] = sourceValues[i];
					continue;
				}

				var targetRangeLine = map.First(rangeLine => rangeLine.SourceRangeStart <= sourceValues[i]);

				if (sourceValues[i] > targetRangeLine.SourceRangeStart + targetRangeLine.Range)
				{
					destinationValues[i] = sourceValues[i];
					continue;
				}

				var distanceFromRangeStart = sourceValues[i] - targetRangeLine.SourceRangeStart;

				destinationValues[i] = targetRangeLine.DestinationRangeStart + distanceFromRangeStart;
			}

			sourceValues = destinationValues.ToArray();
			Array.Clear(destinationValues);
		}

		var lowestLocationNumber = sourceValues.Min();

		return lowestLocationNumber;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}
}
