using System.Text.RegularExpressions;
using Common.Helpers;
using Common.Interfaces;

namespace Year2023;

public class Day04Solver : IPuzzleSolver
{
	public string Title => "Scratchcards";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private const char NumberSeparator = '|';

	private static readonly Regex CardIntroRegex = new(@"Card\s+\d+: ");

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var points = 0d;

		foreach (var cardInformation in input)
		{
			var numberInformation = CardIntroRegex.Replace(cardInformation, string.Empty);

			var splitNumbers = numberInformation.Split(NumberSeparator, StringSplitOptions.TrimEntries);

			var winningNumbers = RegexHelper.GetAllNumbersAsString(splitNumbers[0])
				.ToHashSet();

			var matchCount = RegexHelper.GetAllNumbersAsString(splitNumbers[1])
				.Count(winningNumbers.Contains);

			if (matchCount > 0)
			{
				points += Math.Pow(2, matchCount - 1);
			}
		}

		return points;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var copyCountByCardNumber = new Dictionary<int, int>();

		var totalCardCount = 0;

		foreach (var cardInformation in input)
		{
			var cardIntro = CardIntroRegex.Match(cardInformation).ToString();
			var cardNumber = RegexHelper.GetFirstNumberAsInt(cardIntro);

			// Update total card count
			copyCountByCardNumber.TryGetValue(cardNumber, out var cardCopyCount);

			var cardCount = 1 + cardCopyCount;

			totalCardCount += cardCount;

			// Find number matches in card
			var numberInformation = CardIntroRegex.Replace(cardInformation, string.Empty);

			var splitNumberInformation = numberInformation.Split(NumberSeparator, StringSplitOptions.TrimEntries);

			var winningNumbers = RegexHelper.GetAllNumbersAsString(splitNumberInformation[0])
				.ToHashSet();

			var matchCount = RegexHelper.GetAllNumbersAsString(splitNumberInformation[1])
				.Count(winningNumbers.Contains);

			if (matchCount == 0)
			{
				continue;
			}

			// Update copies of succeeding cards
			for (var i = 1; i <= matchCount; i++)
			{
				var succeedingCardNumber = cardNumber + i;

				if (!copyCountByCardNumber.ContainsKey(succeedingCardNumber))
				{
					copyCountByCardNumber[succeedingCardNumber] = 0;
				}

				copyCountByCardNumber[succeedingCardNumber] += cardCount;
			}
		}

		return totalCardCount;
	}
}
