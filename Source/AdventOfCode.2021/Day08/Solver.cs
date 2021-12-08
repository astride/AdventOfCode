using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day08Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			List<(string[] InputValues, string[] OutputValues)> input;

			if (rawInput.First().Last() == '|')
            {
				//We have example data
				var inputValues = rawInput.Where(entry => entry.Last() == '|').ToList();
				var outputValues = rawInput.Where(entry => entry.Last() != '|').ToList();

				input = new List<(string[] InputValues, string[] OutputValues)>();

				foreach (var i in Enumerable.Range(0, inputValues.Count()))
                {
					input.Add((inputValues[i].Split(' '), outputValues[i].Split(' ')));
                }
            }
			else
            {
				//We have real data
				input = rawInput
				.Where(entry => !string.IsNullOrWhiteSpace(entry))
				.Select(entry => entry.Split('|'))
				.Select(entry =>
					(entry[0]
						.Split(' ')
						.Where(inputValue => !string.IsNullOrWhiteSpace(inputValue))
						.ToArray(),
					entry[1]
						.Split(' ')
						.Where(outputValue => !string.IsNullOrWhiteSpace(outputValue))
						.ToArray()))
				.ToList();
			}

			Part1Solution = SolvePart1(input).ToString();
		}

		private const int CharCountDigit1 = 2;
		private const int CharCountDigit4 = 4;
		private const int CharCountDigit7 = 3;
		private const int CharCountDigit8 = 7;

		private readonly static int[] SimpleDigits = new int[] { CharCountDigit1, CharCountDigit4, CharCountDigit7, CharCountDigit8 };

		private static int SolvePart1(List<(string[] InputValues, string[] OutputValues)> input)
		{
			var simpleDigitCount = input
				.SelectMany(input => input.OutputValues)
				.Count(outputValue => SimpleDigits.Contains(outputValue.Count()));

			return simpleDigitCount;
		}
	}

	public static class Day08Helpers
	{

	}
}
