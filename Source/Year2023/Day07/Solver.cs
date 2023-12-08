using Common.Interfaces;

namespace Year2023;

public class Day07Solver : IPuzzleSolver
{
	public string Title => "CamelCards";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private const char Joker = 'J';

	private static readonly Dictionary<char, int> WorthByCharPart1 = new()
	{
		['A'] = 1,
		['K'] = 2,
		['Q'] = 3,
		['J'] = 4,
		['T'] = 5,
		['9'] = 6,
		['8'] = 7,
		['7'] = 8,
		['6'] = 9,
		['5'] = 10,
		['4'] = 11,
		['3'] = 12,
		['2'] = 13,
	};

	private static readonly Dictionary<char, int> WorthByCharPart2 = new()
	{
		['A'] = 1,
		['K'] = 2,
		['Q'] = 3,
		['T'] = 5,
		['9'] = 6,
		['8'] = 7,
		['7'] = 8,
		['6'] = 9,
		['5'] = 10,
		['4'] = 11,
		['3'] = 12,
		['2'] = 13,
		['J'] = 14,
	};

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var camelCardHands = input
			.Select(line => new CamelCardHand(line))
			.ToList();

		var orderedCamelCardHands = camelCardHands
			.OrderBy(hand => hand.HandType)
			.ThenBy(hand => WorthByCharPart1[hand.Hand[0]])
			.ThenBy(hand => WorthByCharPart1[hand.Hand[1]])
			.ThenBy(hand => WorthByCharPart1[hand.Hand[2]])
			.ThenBy(hand => WorthByCharPart1[hand.Hand[3]])
			.ThenBy(hand => WorthByCharPart1[hand.Hand[4]])
			.ToList();

		var totalWinnings = 0L;

		for (var i = 0; i < orderedCamelCardHands.Count; i++)
		{
			totalWinnings += orderedCamelCardHands[i].Bid * (orderedCamelCardHands.Count - i);
		}

		return totalWinnings;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var camelCardHands = input
			.Select(line => new CamelCardHand(line, activateJoker: true))
			.ToList();

		var orderedCamelCardHands = camelCardHands
			.OrderBy(hand => hand.HandType)
			.ThenBy(hand => WorthByCharPart2[hand.Hand[0]])
			.ThenBy(hand => WorthByCharPart2[hand.Hand[1]])
			.ThenBy(hand => WorthByCharPart2[hand.Hand[2]])
			.ThenBy(hand => WorthByCharPart2[hand.Hand[3]])
			.ThenBy(hand => WorthByCharPart2[hand.Hand[4]])
			.ToList();

		var totalWinnings = 0L;

		for (var i = 0; i < orderedCamelCardHands.Count; i++)
		{
			totalWinnings += orderedCamelCardHands[i].Bid * (orderedCamelCardHands.Count - i);
		}

		return totalWinnings;
	}

	private enum HandType
	{
		FiveOfAKind,
		FourOfAKind,
		FullHouse,
		ThreeOfAKind,
		TwoPair,
		OnePair,
		HighCard,
	}

	private class CamelCardHand
	{
		public string Hand { get; }
		public long Bid { get; }
		public HandType HandType { get; }

		public CamelCardHand(string handInfo, bool activateJoker = false)
		{
			var handInfoSplit = handInfo.Split(' ');

			Hand = handInfoSplit[0];
			Bid = long.Parse(handInfoSplit[1]);

			var cardsGroupedByValue = Hand.GroupBy(card => card).ToList();

			HandType = activateJoker
				? GetHandTypeWithJoker(cardsGroupedByValue)
				: GetHandType(cardsGroupedByValue);
		}

		private static HandType GetHandType(ICollection<IGrouping<char, char>> cardsGroupedByValue)
		{
			return cardsGroupedByValue.Count switch
			{
				1 => HandType.FiveOfAKind,
				2 when cardsGroupedByValue.Any(group => group.Count() == 4) => HandType.FourOfAKind,
				2 => HandType.FullHouse,
				3 when cardsGroupedByValue.Any(group => group.Count() == 3) => HandType.ThreeOfAKind,
				3 => HandType.TwoPair,
				4 => HandType.OnePair,
				5 => HandType.HighCard,
				_ => throw new Exception(),
			};
		}

		private static HandType GetHandTypeWithJoker(ICollection<IGrouping<char, char>> cardsGroupedByValue)
		{
			if (cardsGroupedByValue.All(kvp => kvp.Key != Joker))
			{
				return GetHandType(cardsGroupedByValue);
			}

			var jokerCount = cardsGroupedByValue.Single(kvp => kvp.Key == Joker).Count();

			if (jokerCount > 3)
			{
				return HandType.FiveOfAKind;
			}

			var charToConvertJokerInto = cardsGroupedByValue
				.Where(kvp => kvp.Key != Joker)
				.MaxBy(kvp => kvp.Count())
				.Key;

			var newHand = cardsGroupedByValue
				.SelectMany(group => group)
				.Select(card => card == Joker ? charToConvertJokerInto : card);

			return GetHandType(newHand.GroupBy(card => card).ToList());
		}
	}
}