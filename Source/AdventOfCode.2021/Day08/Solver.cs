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

			if (rawInput.Any(entry => !entry.Contains(Separator)))
			{
				//We have example input
				var outputValues = rawInput.Where(entry => !entry.Contains(Separator)).ToList();

				var inputValues = rawInput
					.Except(outputValues)
					.Select(input => input.Remove(input.IndexOf(Separator)).Trim()) //Remove separator and preceding space
					.ToList();

				input = Enumerable.Range(0, inputValues.Count())
					.Select(i => 
						(inputValues[i].Split(' '),
						outputValues[i].Split(' ')))
					.ToList();
			}
			else
			{
				//We have real input
				input = rawInput
					.Where(entry => !string.IsNullOrWhiteSpace(entry))
					.Select(entry => entry.Split(Separator))
					.Select(entry =>
						(entry[0].Split(' ').ToArray(),
						entry[1].Split(' ').ToArray()))
					.ToList();
			}

			Part1Solution = SolvePart1(input).ToString();
			Part2Solution = SolvePart2(input).ToString();
		}

		private const char Separator = '|';

		private const char a = 'a';
		private const char b = 'b';
		private const char c = 'c';
		private const char d = 'd';
		private const char e = 'e';
		private const char f = 'f';
		private const char g = 'g';

		private readonly static string AllChars = "abcdefg";

		private readonly static IDictionary<int, string> _charsMakingDigit = new Dictionary<int, string>
		{
			[0] = "abcefg",
			[1] = "cf",
			[2] = "acdeg",
			[3] = "acdfg",
			[4] = "bcdf",
			[5] = "abdfg",
			[6] = "abdefg",
			[7] = "acf",
			[8] = "abcdefg",
			[9] = "abcdfg"
		};

		private static readonly int CharCountDigit1 = _charsMakingDigit[1].Length;
		private static readonly int CharCountDigit4 = _charsMakingDigit[4].Length;
		private static readonly int CharCountDigit7 = _charsMakingDigit[7].Length;
		private static readonly int CharCountDigit8 = _charsMakingDigit[8].Length;

		private readonly static int[] SimpleDigits = new int[] { CharCountDigit1, CharCountDigit4, CharCountDigit7, CharCountDigit8 };

		private static readonly int SignalLinesSharingB = _charsMakingDigit.Values.Count(value => value.Contains(b));
		private static readonly int SignalLinesSharingE = _charsMakingDigit.Values.Count(value => value.Contains(e));
		private static readonly int SignalLinesSharingF = _charsMakingDigit.Values.Count(value => value.Contains(f));

		private static int SolvePart1(List<(string[] InputValues, string[] OutputValues)> input)
		{
			var simpleDigitCount = input
				.SelectMany(input => input.OutputValues)
				.Count(outputValue => SimpleDigits.Contains(outputValue.Count()));

			return simpleDigitCount;
		}

		private static int SolvePart2(List<(string[] InputValues, string[] OutputValues)> input)
		{
			IDictionary<char, char> inputCharFromGoalChar = new Dictionary<char, char> { };

			IEnumerable<(char Char, int Count)> charAndCount;
			string output;
			int total = 0;

			foreach (var line in input)
			{
				inputCharFromGoalChar.Clear();

				charAndCount = string.Concat(line.InputValues)
					.GroupBy(ch => ch)
					.Select(gr => (gr.Key, gr.Count()));

				inputCharFromGoalChar[b] = charAndCount
					.Single(ch => ch.Count == SignalLinesSharingB)
					.Char;

				inputCharFromGoalChar[e] = charAndCount
					.Single(ch => ch.Count == SignalLinesSharingE)
					.Char;

				inputCharFromGoalChar[f] = charAndCount
					.Single(ch => ch.Count == SignalLinesSharingF)
					.Char;

				inputCharFromGoalChar[c] = line.InputValues
					.Single(entry => entry.Length == CharCountDigit1)
					.Single(ch => ch != inputCharFromGoalChar[f]);

				inputCharFromGoalChar[a] = line.InputValues
					.Single(entry => entry.Length == CharCountDigit7)
					.Single(ch =>
						ch != inputCharFromGoalChar[c] &&
						ch != inputCharFromGoalChar[f]);

				inputCharFromGoalChar[d] = line.InputValues
					.Single(entry => entry.Length == CharCountDigit4)
					.Single(ch =>
						ch != inputCharFromGoalChar[b] &&
						ch != inputCharFromGoalChar[c] &&
						ch != inputCharFromGoalChar[f]);

				inputCharFromGoalChar[g] = AllChars
					.Except(inputCharFromGoalChar.Values)
					.Single();

				output = string.Concat(line.OutputValues
					.Select(value => _charsMakingDigit
						.Single(digitsAndNeededChars => 
							digitsAndNeededChars.Value.Length == value.Length &&
							digitsAndNeededChars.Value
								.Select(ch => inputCharFromGoalChar[ch])
								.All(ch => value.Contains(ch)))
						.Key));

				total += int.Parse(string.Concat(output));
			}

			return total;
		}
	}
}
