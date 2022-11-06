using System.Globalization;
using Common.Interfaces;

namespace Year2021;

public class Day07Solver : IPuzzleSolver
{
	public string Title => "The Treachery of Whales";
	public string Part1Solution { get; set; } = string.Empty;
	public string Part2Solution { get; set; } = string.Empty;

	public void SolvePuzzle(string[] rawInput)
	{
		var input = rawInput
			.Single(entry => !string.IsNullOrWhiteSpace(entry))
			.Split(',')
			.Select(int.Parse)
			.ToArray();

		Part1Solution = SolvePart1(input).ToString();
		Part2Solution = SolvePart2(input).ToString(CultureInfo.InvariantCulture);
	}

	private static int SolvePart1(IReadOnlyCollection<int> crabPositions)
	{
		var minPos = crabPositions.Min();
		var maxPos = crabPositions.Max();

		var cheapestFuel = Enumerable.Range(minPos, maxPos - minPos + 1)
			.Select(finalPos => crabPositions
				.Sum(pos => Math.Abs(finalPos - pos)))
			.Min();

		return cheapestFuel;
	}

	private static double SolvePart2(int[] crabPositions)
	{
		List<(int Position, int Count)> positionAndCrabCount = crabPositions
			.GroupBy(pos => pos)
			.Select(gr => (gr.Key, gr.Count()))
			.ToList();

		var minFinalPosition = crabPositions.Min();
		var maxFinalPosition = crabPositions.Max();

		var possibleFinalPositions =
			Enumerable.Range(minFinalPosition, (maxFinalPosition - minFinalPosition) + 1).ToList();

		var cheapestFuel = double.MaxValue;

		foreach (var finalPos in possibleFinalPositions)
		{
			var requiredStepsFromPosition = positionAndCrabCount
				.Select(pacc => pacc.Position)
				.Select(pos => Math.Abs(pos - finalPos));

			var requiredFuel = requiredStepsFromPosition
				.Zip(positionAndCrabCount.Select(pacc => pacc.Count),
					(requiredSteps, crabCount) => crabCount * requiredSteps.CalculateSpentFuel())
				.Sum();

			if (requiredFuel < cheapestFuel)
			{
				cheapestFuel = requiredFuel;
			}
		}

		return cheapestFuel;
	}
}

internal static class Day07Helpers
{
	public static double CalculateSpentFuel(this int steps)
	{
		if (steps == 0)
		{
			return 0;
		}

		return (double)steps * (steps + 1) / 2;
	}
}
