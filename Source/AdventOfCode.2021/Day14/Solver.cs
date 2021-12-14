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

			IDictionary<string, char> charToAddForPair = rawInput
				.Skip(2)
				.Select(entry => entry.Split(' '))
				.ToDictionary(
					rule => rule.First(), 
					rule => rule.Last().Single());

			Part1Solution = SolvePart1(polymerTemplate, charToAddForPair).ToString();
			Part2Solution = SolvePart2(polymerTemplate, charToAddForPair).ToString();
		}

		private static int SolvePart1(string polymerTemplate, IDictionary<string, char> charToAddForPair)
		{
			var resultForPair = charToAddForPair.GetResultForPairDict();

			foreach (var _ in Enumerable.Range(0, 10))
			{
				polymerTemplate = polymerTemplate.GetNextPolymer(resultForPair);
			}

			var countPerElement = polymerTemplate
				.GroupBy(ch => ch)
				.Select(gr => gr.Count());

			return countPerElement.Max() - countPerElement.Min();
		}

		private static int SolvePart2(string polymerTemplate, IDictionary<string, char> charToAddForPair)
		{
			charToAddForPair.PrepareDictionaries();

			var polymerConfig = polymerTemplate.PreparePolymer();

			//TODO

			return -1;
		}
	}

	public static class Day14Helpers
	{
		static IDictionary<string, ((string, string) CreatedPairs, char RedundantChar)> ResultOfPair;
		static IDictionary<char, double> RedundantOccurrencesOfChar;

		#region Part 1
		public static IDictionary<string, string> GetResultForPairDict(this IDictionary<string, char> charCreatedFromPair)
		{
			return charCreatedFromPair
				.ToDictionary(
					entry => entry.Key,
					entry => string.Concat(new char[] { entry.Key.First(), entry.Value }));
		}

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
		#endregion

		#region Part 2
		public static void PrepareDictionaries(this IDictionary<string, char> charToAddForPair)
		{
			RedundantOccurrencesOfChar = charToAddForPair
				.Select(kvp => kvp.Value)
				.Distinct()
				.ToDictionary(ch => ch, ch => (double)0);

			ResultOfPair = charToAddForPair
				.ToDictionary(
					entry => entry.Key,
					entry => (
						(string.Concat(new char[] { entry.Key.First(), entry.Value }),
						string.Concat(new char[] { entry.Value, entry.Key.Last() })),
						entry.Value));
		}

		public static IDictionary<string, double> PreparePolymer(this string template)
		{
			var polymerConfig = ResultOfPair
				.ToDictionary(
					entry => entry.Key,
					entry => (double)0);

			foreach (var i in Enumerable.Range(0, template.Length - 1))
			{
				polymerConfig[template.Substring(i, 2)]++;
			}

			foreach (var pair in polymerConfig)
			{
				RedundantOccurrencesOfChar[(ResultOfPair[pair.Key]).RedundantChar] += pair.Value;
			}

			return polymerConfig;
		}

		//public static IDictionary<string, double> InsertPairs(this IDictionary<string, double> polymerConfig)
		//{

		//}
		#endregion
	}
}
