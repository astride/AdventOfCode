using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day18Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var assignment = rawInput
				.Where(entry => !string.IsNullOrWhiteSpace(entry))
				.Select(entry => entry.AsSnailFishNumber())
				.ToList();

			Part1Solution = SolvePart1(assignment).ToString();
		}

		private static int SolvePart1(List<List<object>> numbers)
		{
			var param = numbers.First();

			foreach (var i in Enumerable.Range(1, numbers.Count() - 1))
			{
				param.SnailFishAdd(numbers[i]);
				param.SnailFishReduce();
			}

			param.Print();

			return param.GetMagnitude();
		}
	}

	public static class Day18Helpers
	{
		private const int DeepestNestingLevel = 4;
		private const int ThresholdValue = 9;

		private const char PairOpeningChar = '[';
		private const char PairClosingChar = ']';
		private const char ValueSeparator = ',';

		public static List<object> AsSnailFishNumber(this string rawNumber)
		{
			var snailFishNumber = new List<object>();
			char temp;

			while (rawNumber.Length > 0)
			{
				temp = rawNumber.First();

				if (temp == PairOpeningChar || temp == PairClosingChar)
				{
					snailFishNumber.Add(temp);
				}
				else if (temp != ValueSeparator)
				{
					snailFishNumber.Add(int.Parse(temp.ToString()));
				}

				rawNumber = rawNumber.Substring(1);
			}

			return snailFishNumber;
		}

		public static void SnailFishAdd(this List<object> number, List<object> addend)
		{
			number.Insert(0, PairOpeningChar);
			number.AddRange(addend);
			number.Add(PairClosingChar);
		}

		public static void SnailFishReduce(this List<object> number)
		{
			int? explosionIndex;
			int? splitIndex;

			while (true)
			{
				explosionIndex = number.GetExplosionIndex();
				splitIndex = number.GetSplitIndex();

				if (explosionIndex.HasValue) number.ExplodePairAt(explosionIndex.Value);
				else if (splitIndex.HasValue) number.SplitNumberAt(splitIndex.Value);
				else return;
			}
		}

		private static int? GetExplosionIndex(this List<object> number)
		{
			var openingCharCount = 0;
			object item;

			foreach (var index in Enumerable.Range(0, number.Count))
			{
				item = number[index];

				if (item is char ch && ch == PairOpeningChar)
				{
					openingCharCount++;

					if (openingCharCount > DeepestNestingLevel)
					{
						// Assuming the nesting is never deeper than the deepest nesting level + 1
						return index;
					}
				}
				else if (item is char ch2 && ch2 == PairClosingChar)
				{
					openingCharCount--;
				}
			}

			return null;
		}

		private static int? GetSplitIndex(this List<object> number)
		{
			var itemToSplit = number.FirstOrDefault(item => item is int num && num > ThresholdValue);

			return itemToSplit != default
				? (int?)number.IndexOf(itemToSplit)
				: null;
		}

		private static void ExplodePairAt(this List<object> number, int index)
		{
			var explodingLeft = (int)number[index + 1];
			var explodingRight = (int)number[index + 2];

			number.RemoveRange(index, 4);
			number.Insert(index, 0);

			var leftNumber = number.Take(index).LastOrDefault(item => item is int);

			if (leftNumber != default)
			{
				var leftNumberIndex = number.LastIndexOf(leftNumber, index - 1);
				var leftNumberValue = (int)number[leftNumberIndex];

				number[leftNumberIndex] = leftNumberValue + explodingLeft;
			}

			var rightNumber = number.Skip(index + 1).FirstOrDefault(item => item is int);

			if (rightNumber != default)
			{
				var rightNumberIndex = number.IndexOf(rightNumber, index + 1);
				var rightNumberValue = (int)number[rightNumberIndex];

				number[rightNumberIndex] = rightNumberValue + explodingRight;
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

		public static void Print(this List<object> number)
		{
			Console.WriteLine();

			bool isNumber;
			bool isOpeningChar;

			bool prevIsNumber;
			bool prevIsClosingChar;

			foreach (var index in Enumerable.Range(0, number.Count))
			{
				isNumber = number[index] is int;
				isOpeningChar = number[index].ToString() == "[";
				
				prevIsNumber = index > 0 && number[index - 1] is int;
				prevIsClosingChar = index > 0 && number[index - 1].ToString() == "]";

				if (index > 0 &&
					((isOpeningChar && prevIsClosingChar) ||
					(isNumber && prevIsNumber) ||
					(isNumber && prevIsClosingChar) ||
					(isOpeningChar && prevIsNumber)))
				{
					Console.Write(",");
				}

				Console.Write(number[index]);
			}

			Console.WriteLine();
		}
	}
}
