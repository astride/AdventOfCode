using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day14Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var polymerTemplate = rawInput.First();

			IDictionary<string, string> resultForPair = rawInput
				.Skip(2)
				.Select(entry => entry.Split(' '))
				.ToDictionary(
					rule => rule.First(), 
					rule => rule.First().First() + rule.Last());

			Part1Solution = SolvePart1(polymerTemplate, resultForPair).ToString();
		}

		private static int SolvePart1(string polymerTemplate, IDictionary<string, string> resultForPair)
		{
			foreach (var _ in Enumerable.Range(0, 10))
			{
				polymerTemplate = polymerTemplate.GetNextPolymer(resultForPair);
			}

			var countPerElement = polymerTemplate
				.GroupBy(ch => ch)
				.Select(gr => gr.Count());

			return countPerElement.Max() - countPerElement.Min();
		}
	}

	public static class Day14Helpers
	{
		public static string GetNextPolymer(this string polymerTemplate, IDictionary<string, string> resultForPair)
		{
			var results = new List<string>();

			foreach (var i in Enumerable.Range(0, polymerTemplate.Length - 1))
			{
				results.Add(resultForPair[polymerTemplate.Substring(i, 2)]);
			}

			results.Add(polymerTemplate.Last().ToString());

			return string.Concat(results);
		}
	}
}
