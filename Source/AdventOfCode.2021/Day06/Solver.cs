using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day06Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var input = rawInput
				.Where(entry => !string.IsNullOrWhiteSpace(entry))
				.Single()
				.Split(',')
				.Where(entry => !string.IsNullOrWhiteSpace(entry))
				.Select(entry => int.Parse(entry));

			Part1Solution = SolvePart1(input).ToString();
		}

		private const int Days = 80;

		private static int SolvePart1(IEnumerable<int> timerValues)
		{
			var shoal = timerValues.ToList();

			foreach (var _ in Enumerable.Range(0, Days))
			{
				shoal.SimulatePopulationChange();
			}

			return shoal.Count();
		}
	}

	public static class Day06Helpers
	{
		private const int TimerValueInit = 8;
		private const int TimerValueAboutToSpawn = 0;
		private const int TimerValueAfterSpawning = 6;

		public static void SimulatePopulationChange(this List<int> population)
		{
			var spawnCount = population.Count(entity => entity.IsReadyToSpawn());

			population.UpdateInternalTimers();
			population.Spawn(spawnCount);
		}

		private static bool IsReadyToSpawn(this int maternalTimer)
		{
			return maternalTimer == TimerValueAboutToSpawn;
		}

		private static void UpdateInternalTimers(this List<int> timers)
		{
			for (var i = 0; i < timers.Count(); i++)
			{
				timers[i] = timers[i].GetNextValue();
			}
		}

		private static int GetNextValue(this int value)
		{
			return value == TimerValueAboutToSpawn
				? TimerValueAfterSpawning
				: value - 1;
		}

		private static void Spawn(this List<int> population, int count)
		{
			if (count == 0) return;

			population.AddRange(Enumerable.Repeat(TimerValueInit, count));
		}
	}
}
