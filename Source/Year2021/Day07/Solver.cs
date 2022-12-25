using Common.Interfaces;

namespace Year2021;

public class Day07Solver : IPuzzleSolver
{
	public string Title => "The Treachery of Whales";
	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input)
	{
		var crabPositions = GetCrabPositions(input);
		
		var minPos = crabPositions.Min();
		var maxPos = crabPositions.Max();

		var cheapestFuel = Enumerable.Range(minPos, maxPos - minPos + 1)
			.Select(finalPos => crabPositions
				.Sum(pos => Math.Abs(finalPos - pos)))
			.Min();

		return cheapestFuel;
	}

	public object GetPart2Solution(string[] input)
	{
		var crabPositions = GetCrabPositions(input);
		
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

	private static IReadOnlyCollection<int> GetCrabPositions(string[] input)
	{
		return input
			.Single(entry => !string.IsNullOrWhiteSpace(entry))
			.Split(',')
			.Select(int.Parse)
			.ToArray();
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
