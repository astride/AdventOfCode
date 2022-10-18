﻿using Common.Interfaces;

namespace Year2021;

public class Day08Solver : IPuzzleSolver
{
	public string Part1Solution { get; set; } = string.Empty;
	public string Part2Solution { get; set; } = string.Empty;

	public void SolvePuzzle(string[] rawInput)
	{
		List<(string[] InputValues, string[] OutputValues)> input;

		if (rawInput.Any(entry => !entry.Contains(Separator)))
		{
			//We have example input
			var outputValues = rawInput
				.Where(entry => !entry.Contains(Separator))
				.ToList();

			var inputValues = rawInput
				.Except(outputValues)
				.Select(inputValue => inputValue.Remove(inputValue.IndexOf(Separator)).Trim()) //Remove separator and preceding space
				.ToList();

			input = Enumerable.Range(0, inputValues.Count)
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

	private const char A = 'a';
	private const char B = 'b';
	private const char C = 'c';
	private const char D = 'd';
	private const char E = 'e';
	private const char F = 'f';
	private const char G = 'g';

	private const string AllChars = "abcdefg";

	private static readonly IDictionary<int, string> CharsMakingDigit = new Dictionary<int, string>
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

	private static readonly int CharCountDigit1 = CharsMakingDigit[1].Length;
	private static readonly int CharCountDigit4 = CharsMakingDigit[4].Length;
	private static readonly int CharCountDigit7 = CharsMakingDigit[7].Length;
	private static readonly int CharCountDigit8 = CharsMakingDigit[8].Length;

	private static readonly int[] SimpleDigits = { CharCountDigit1, CharCountDigit4, CharCountDigit7, CharCountDigit8 };

	private static readonly int SignalLinesSharingB = CharsMakingDigit.Values.Count(value => value.Contains(B));
	private static readonly int SignalLinesSharingE = CharsMakingDigit.Values.Count(value => value.Contains(E));
	private static readonly int SignalLinesSharingF = CharsMakingDigit.Values.Count(value => value.Contains(F));

	private static int SolvePart1(List<(string[] InputValues, string[] OutputValues)> input)
	{
		var simpleDigitCount = input
			.SelectMany(entry => entry.OutputValues)
			.Count(outputValue => SimpleDigits.Contains(outputValue.Length));

		return simpleDigitCount;
	}

	private static int SolvePart2(List<(string[] InputValues, string[] OutputValues)> input)
	{
		var inputCharFromGoalChar = new Dictionary<char, char>();

		var total = 0;

		foreach (var line in input)
		{
			inputCharFromGoalChar.Clear();

			IEnumerable<(char Char, int Count)> charAndCount = string.Concat(line.InputValues)
				.GroupBy(ch => ch)
				.Select(gr => (gr.Key, gr.Count()))
				.ToList();

			inputCharFromGoalChar[B] = charAndCount
				.Single(ch => ch.Count == SignalLinesSharingB)
				.Char;

			inputCharFromGoalChar[E] = charAndCount
				.Single(ch => ch.Count == SignalLinesSharingE)
				.Char;

			inputCharFromGoalChar[F] = charAndCount
				.Single(ch => ch.Count == SignalLinesSharingF)
				.Char;

			inputCharFromGoalChar[C] = line.InputValues
				.Single(entry => entry.Length == CharCountDigit1)
				.Single(ch => ch != inputCharFromGoalChar[F]);

			inputCharFromGoalChar[A] = line.InputValues
				.Single(entry => entry.Length == CharCountDigit7)
				.Single(ch =>
					ch != inputCharFromGoalChar[C] &&
					ch != inputCharFromGoalChar[F]);

			inputCharFromGoalChar[D] = line.InputValues
				.Single(entry => entry.Length == CharCountDigit4)
				.Single(ch =>
					ch != inputCharFromGoalChar[B] &&
					ch != inputCharFromGoalChar[C] &&
					ch != inputCharFromGoalChar[F]);

			inputCharFromGoalChar[G] = AllChars
				.Except(inputCharFromGoalChar.Values)
				.Single();

			var output = string.Concat(line.OutputValues
				.Select(value => CharsMakingDigit
					.Single(digitsAndNeededChars => 
						digitsAndNeededChars.Value.Length == value.Length &&
						digitsAndNeededChars.Value
							.Select(ch => inputCharFromGoalChar[ch])
							.All(value.Contains))
					.Key));

			total += int.Parse(string.Concat(output));
		}

		return total;
	}
}