using Common.Interfaces;

namespace Year2021;

public class Day18Solver : IPuzzleSolver
{
	public string Title => "Snailfish";
	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input)
	{
		var numbers = GetAssignment(input);
		
		var param = new List<object>(numbers[0]);

		foreach (var i in Enumerable.Range(1, numbers.Count - 1))
		{
			param.SnailFishAdd(numbers[i]);
			param.SnailFishReduce();
		}

		return param.GetMagnitude();
	}

	public object GetPart2Solution(string[] input)
	{
		var numbers = GetAssignment(input);
		
		var firstAddends = numbers
			.Select(number => number.ToFirstAddend())
			.ToList();

		return Day18Helpers.FindLargestMagnitude(firstAddends, numbers);
	}

	private static List<List<object>> GetAssignment(string[] input)
	{
		return input
			.Where(entry => !string.IsNullOrWhiteSpace(entry))
			.Select(entry => entry.AsSnailFishNumber())
			.ToList();
	}
}

internal static class Day18Helpers
{
	private const int DeepestNestingLevel = 4;
	private const int ThresholdValue = 9;

	private const char PairOpeningChar = '[';
	private const char PairClosingChar = ']';
	private const char Separator = ',';

	public static List<object> AsSnailFishNumber(this string rawNumber)
	{
		var number = new List<object>();

		while (rawNumber.Length > 0)
		{
			var ch = rawNumber[0];

			if (ch is PairOpeningChar or PairClosingChar)
			{
				number.Add(ch);
			}
			else if (ch != Separator)
			{
				number.Add(int.Parse(ch.ToString()));
			}

			rawNumber = rawNumber[1..];
		}

		return number;
	}

	public static void SnailFishAdd(this List<object> number, IEnumerable<object> addend)
	{
		number.Insert(0, PairOpeningChar);
		number.AddRange(addend);
		number.Add(PairClosingChar);
	}

	public static void SnailFishReduce(this List<object> number)
	{
		while (true)
		{
			var explosionIndex = number.GetExplosionIndex(DeepestNestingLevel);
			var splitIndex = number.GetSplitIndex();

			if (explosionIndex.HasValue) number.ExplodePairAt(explosionIndex.Value);
			else if (splitIndex.HasValue) number.SplitNumberAt(splitIndex.Value);
			else return;
		}
	}

	private static int? GetExplosionIndex(this List<object> number, int deepestNestingLevel)
	{
		var openingCharCount = 0;

		foreach (var index in Enumerable.Range(0, number.Count))
		{
			if (number[index] is not char ch)
			{
				continue;
			}

			// Assuming the nesting is never past one level deeper than the deepest nesting level
			if (ch == PairOpeningChar &&
			    openingCharCount == deepestNestingLevel)
			{
				return index;
			}

			switch (ch)
			{
				case PairOpeningChar:
					openingCharCount++;
					break;
				case PairClosingChar:
					openingCharCount--;
					break;
			}
		}

		return null;
	}

	private static int? GetSplitIndex(this List<object> number)
	{
		var itemToSplit = number
			.FirstOrDefault(item => item is int num && num > ThresholdValue);

		return itemToSplit != default
			? number.IndexOf(itemToSplit)
			: null;
	}

	private static void ExplodePairAt(this List<object> number, int index)
	{
		var explodingValueLeft = (int)number[index + 1];
		var explodingValueRight = (int)number[index + 2];

		// Replace pair with 0
		number.RemoveRange(index, 4);
		number.Insert(index, 0);

		// Update number to the left, if any
		var leftNumber = number.Take(index).LastOrDefault(item => item is int);

		if (leftNumber != default)
		{
			var leftNumberIndex = number.LastIndexOf(leftNumber, index - 1);
			var leftNumberValue = (int)number[leftNumberIndex];

			number[leftNumberIndex] = leftNumberValue + explodingValueLeft;
		}

		// Update number to the right, if any
		var rightNumber = number.Skip(index + 1).FirstOrDefault(item => item is int);

		if (rightNumber != default)
		{
			var rightNumberIndex = number.IndexOf(rightNumber, index + 1);
			var rightNumberValue = (int)number[rightNumberIndex];

			number[rightNumberIndex] = rightNumberValue + explodingValueRight;
		}
	}

	private static void SplitNumberAt(this List<object> number, int index)
	{
		decimal half = (decimal)(int)number[index] / 2;

		number.RemoveAt(index);

		number.InsertRange(index, new List<object> 
		{ 
			PairOpeningChar, 
			(int)Math.Floor(half), 
			(int)Math.Ceiling(half), 
			PairClosingChar
		});
	}

	public static int GetMagnitude(this List<object> number)
	{
		while (number.Count > 1)
		{
			number.MagnitudinizeFirstPair();
		}

		return (int)number.Single();
	}

	private static int GetMagnitude(int x, int y) => 3 * x + 2 * y;

	private static void MagnitudinizeFirstPair(this List<object> number)
	{
		foreach (var index in Enumerable.Range(0, number.Count - 1))
		{
			if (number[index] is int && number[index + 1] is int)
			{
				var magnitude = GetMagnitude((int)number[index], (int)number[index + 1]);

				number.RemoveRange(index - 1, 4);
				number.Insert(index - 1, magnitude);

				return;
			}
		}
	}

	public static List<object> ToFirstAddend(this List<object> number)
	{
		// Explode until there's nothing more to explode without affecting the (currently nonexisting) second addend
		var addend = new List<object>(number);

		while (true)
		{
			var explosionIndex = addend.GetExplosionIndex(DeepestNestingLevel - 1);

			if (explosionIndex == null || explosionIndex == addend.LastNumericIndex()) return addend;
			else addend.ExplodePairAt(explosionIndex.Value);
		}
	}

	private static int LastNumericIndex(this List<object> number) => number.FindLastIndex(item => item is int);

	public static int FindLargestMagnitude(List<List<object>> firstAddends, List<List<object>> secondAddends)
	{
		var largestMagnitude = 0;

		foreach (var firstIndex in Enumerable.Range(0, firstAddends.Count))
		{
			foreach (var secondIndex in Enumerable.Range(0, secondAddends.Count))
			{
				if (secondIndex == firstIndex) continue;

				var sum = new List<object>(firstAddends[firstIndex]);

				sum.SnailFishAdd(secondAddends[secondIndex]);
				sum.SnailFishReduce();

				largestMagnitude = Math.Max(largestMagnitude, sum.GetMagnitude());
			}
		}

		return largestMagnitude;
	}
}
