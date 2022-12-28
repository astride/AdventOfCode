using Common.Interfaces;

namespace Year2021;

public class Day11Solver : IPuzzleSolver
{
	public string Title => "Dumbo Octopus";
	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input)
	{
		var octopusEnergyLevels = GetOctopusEnergyLevels(input);
		
		var energyLevelsFlattened = octopusEnergyLevels.ToFlattened();
		var flashCounts = energyLevelsFlattened.Simulate(100);

		return flashCounts.Sum();
	}

	public object GetPart2Solution(string[] input)
	{
		var octopusEnergyLevels = GetOctopusEnergyLevels(input);
		
		var energyLevelsFlattened = octopusEnergyLevels.ToFlattened();
		var simultaneousStep = energyLevelsFlattened.SimulateUntilSynchronized();

		return simultaneousStep;
	}

	private static string[] GetOctopusEnergyLevels(string[] input)
	{
		return input
			.Where(entry => !string.IsNullOrWhiteSpace(entry))
			.ToArray();
	}
}

internal static class Day11Helpers
{
	private const int EnergyLevelAfterFlash = 0;
	private const int EnergyLevelFlash = 10;

	private static int EnergyLevelRowSize;
	private static int EnergyLevelCount;

	public static List<int> ToFlattened(this string[] energyLevels)
	{
		EnergyLevelRowSize = energyLevels.First().Length;

		var flattened = energyLevels
			.SelectMany(energyLevelRow => energyLevelRow
				.ToCharArray()
				.Select(energyLevel => int.Parse(energyLevel.ToString())))
			.ToList();

		EnergyLevelCount = flattened.Count;

		return flattened;
	}

	public static IEnumerable<int> Simulate(this List<int> energyLevels, int steps)
	{
		foreach (var step in Enumerable.Range(1, steps))
		{
			yield return energyLevels.ExecuteStepAndGetFlashCount();
		}
	}

	public static int SimulateUntilSynchronized(this List<int> energyLevels)
	{
		var stepForSimultaneousFlash = 1;

		while (energyLevels.ExecuteStepAndGetFlashCount() != EnergyLevelCount)
		{
			stepForSimultaneousFlash++;
		}

		return stepForSimultaneousFlash;
	}

	private static int ExecuteStepAndGetFlashCount(this List<int> energyLevels)
	{
		foreach (var i in Enumerable.Range(0, EnergyLevelCount))
		{
			energyLevels[i]++;
		}

		energyLevels.DoTheFlashDance();

		foreach (var i in Enumerable.Range(0, EnergyLevelCount))
		{
			if (energyLevels[i] >= EnergyLevelFlash)
			{
				energyLevels[i] = EnergyLevelAfterFlash;
			}
		}

		return energyLevels.Count(level => level == EnergyLevelAfterFlash);
	}

	private static void DoTheFlashDance(this List<int> energyLevels)
	{
		while (energyLevels.Any(level => level == EnergyLevelFlash))
		{
			foreach (var i in Enumerable.Range(0, EnergyLevelCount))
			{
				if (energyLevels[i] == EnergyLevelFlash)
				{
					energyLevels.Flash(i);
				}
			}
		}
	}

	private static void Flash(this List<int> energyLevels, int index)
	{
		// increase energy level of self
		energyLevels[index]++;

		// determine location of self
		var isUppermost = index < EnergyLevelRowSize;
		var isLowermost = index >= EnergyLevelCount - EnergyLevelRowSize;
		var isRightmost = index % EnergyLevelRowSize == EnergyLevelRowSize - 1;
		var isLeftmost = index % EnergyLevelRowSize == 0;

		// find adjacent indices
		var adjacentIndices = new List<int>();

		if (!isUppermost) adjacentIndices.Add(index - EnergyLevelRowSize);
		if (!isLowermost) adjacentIndices.Add(index + EnergyLevelRowSize);

		if (!isLeftmost) adjacentIndices.Add(index - 1);
		if (!isRightmost) adjacentIndices.Add(index + 1);

		if (!isUppermost && !isLeftmost) adjacentIndices.Add(index - (EnergyLevelRowSize + 1));
		if (!isUppermost && !isRightmost) adjacentIndices.Add(index - (EnergyLevelRowSize - 1));

		if (!isLowermost && !isLeftmost) adjacentIndices.Add(index + (EnergyLevelRowSize - 1));
		if (!isLowermost && !isRightmost) adjacentIndices.Add(index + (EnergyLevelRowSize + 1));

		// increase every adjacent energy level that is not about to flash themselves
		var energyLevelsToIncrease = adjacentIndices
			.Where(i =>
				i >= 0 &&
				i < EnergyLevelCount &&
				energyLevels[i] < EnergyLevelFlash);

		foreach (var i in energyLevelsToIncrease)
		{
			energyLevels[i]++;
		}
	}
}
