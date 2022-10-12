using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Year2021;

public class Day07Solver : IPuzzleSolver
{
	public string Part1Solution { get; set; }
	public string Part2Solution { get; set; }

	public void SolvePuzzle(string[] rawInput)
	{
		var input = rawInput
			.Where(entry => !string.IsNullOrWhiteSpace(entry))
			.Single()
			.Split(',')
			.Select(entry => int.Parse(entry))
			.ToArray();

		Part1Solution = SolvePart1(input).ToString();
		Part2Solution = SolvePart2(input).ToString();
	}

	private static int SolvePart1(int[] crabPositions)
	{
		var minPos = crabPositions.Min();
		var maxPos = crabPositions.Max();

		var cheapestFuel = crabPositions.Length * (maxPos - minPos); // Max possible spent fuel
		int requiredFuel;

		foreach (var finalPos in Enumerable.Range(minPos, maxPos))
		{
			requiredFuel = crabPositions.Select(pos => Math.Abs(pos - finalPos)).Sum();

			if (requiredFuel < cheapestFuel)
			{
				cheapestFuel = requiredFuel;
			}
		}

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
		var possibleFinalPositions = Enumerable.Range(minFinalPosition, (maxFinalPosition - minFinalPosition) + 1);
		
		var startPositionIndices = Enumerable.Range(0, positionAndCrabCount.Count());

		var requiredStepsFromPosition = new List<int>();
		double requiredFuel;

		var cheapestFuel = double.MaxValue;

		foreach (var position in possibleFinalPositions)
		{
			requiredStepsFromPosition = positionAndCrabCount
				.Select(pacc => Math.Abs(pacc.Position - position))
				.ToList();

			requiredFuel = startPositionIndices
				.Select(i => positionAndCrabCount[i].Count * requiredStepsFromPosition[i].CalculateSpentFuel())
				.Sum();

			if (requiredFuel < cheapestFuel)
			{
				cheapestFuel = requiredFuel;
			}
		}

		return cheapestFuel;
	}
}

public static class Day07Helpers
{
	public static double CalculateSpentFuel(this int steps)
	{
		if (steps == 0) return 0;

		return (steps * (steps + 1) / 2);
	}
}
