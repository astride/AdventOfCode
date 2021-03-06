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
				.Select(entry => int.Parse(entry))
				.ToArray();

			Part1Solution = SolvePart1(input).ToString();
			Part2Solution = SolvePart2(input).ToString();
		}

		private static int SolvePart1(int[] timerValues)
		{
			var shoal = timerValues.ToList();

			foreach (var _ in Enumerable.Range(0, 80))
			{
				shoal.SimulatePopulationChange();
			}

			return shoal.Count();
		}

		private static decimal SolvePart2(int[] timerValues)
		{
			var fishCountPerTimerValue = Enumerable.Repeat((decimal)0, Day06Helpers.TimerValueForSpawn + 1).ToList();

			foreach (var timerValue in timerValues)
			{
				fishCountPerTimerValue[timerValue]++;
			}

			foreach (var _ in Enumerable.Range(0, 256))
			{
				fishCountPerTimerValue.SimulatePopulationChange();
			}

			return fishCountPerTimerValue.Sum();
		}
	}

	public static class Day06Helpers
	{
		public const int TimerValueForSpawn = 8;
		private const int TimerValuePreSpawning = 0;
		private const int TimerValuePostSpawning = 6;

		#region Part 1
		public static void SimulatePopulationChange(this List<int> population)
		{
			var spawnCount = population.Count(entity => entity.IsReadyToSpawn());

			population.UpdateInternalTimers();
			population.Spawn(spawnCount);
		}

		private static bool IsReadyToSpawn(this int maternalTimer)
		{
			return maternalTimer == TimerValuePreSpawning;
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
			return value == TimerValuePreSpawning
				? TimerValuePostSpawning
				: value - 1;
		}

		private static void Spawn(this List<int> population, int count)
		{
			if (count == 0) return;

			population.AddRange(Enumerable.Repeat(TimerValueForSpawn, count));
		}
		#endregion

		#region Part 2
		public static void SimulatePopulationChange(this List<decimal> entityCountPerTimerValue)
		{
			var entityCountAboutToSpawn = entityCountPerTimerValue[TimerValuePreSpawning];

			for (var i = 0; i < TimerValueForSpawn; i++)
			{
				entityCountPerTimerValue[i] = entityCountPerTimerValue[i + 1];
			}

			entityCountPerTimerValue[TimerValuePostSpawning] += entityCountAboutToSpawn;
			entityCountPerTimerValue[TimerValueForSpawn] = entityCountAboutToSpawn;
		}
		#endregion
	}
}
