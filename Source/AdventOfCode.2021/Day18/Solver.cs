using AdventOfCode.Common;
using AdventOfCode.Common.Classes;
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
			var numbers = rawInput
				.Where(entry => !string.IsNullOrWhiteSpace(entry))
				.Select(entry => new SnailFishNumber(entry))
				.ToList();

			Part1Solution = SolvePart1(numbers).ToString();
		}

		private static int SolvePart1(List<SnailFishNumber> numbers)
		{
			SnailFishNumber param = numbers.First();
			SnailFishNumber addend;

			foreach (var i in Enumerable.Range(1, numbers.Count() - 1))
			{
				addend = numbers[i];

				param.Add(addend);
				param.Reduce();
			}

			var magnitude = param.GetMagnitude();

			return magnitude;
		}
	}

	public static class Day18Helpers
	{

	}

	public class SnailFishNumber
	{
		private const int DeepestNestingLevel = 4; // assuming there is never nesting beyond level 4
		private const int ThresholdValue = 9;

		public SnailFishNumber(string numberAsString)
		{
			//TODO
		}

		public SnailFishNumber(int x, int y)
		{
			X = x;
			Y = y;
		}

		public SnailFishNumber(int x, SnailFishNumber yPair)
		{
			X = x;
			Ypair = yPair;
		}

		public SnailFishNumber(SnailFishNumber xPair, int y)
		{
			Xpair = xPair;
			Y = y;
		}

		public SnailFishNumber(SnailFishNumber xPair, SnailFishNumber yPair)
		{
			Xpair = xPair;
			Ypair = yPair;
		}

		public int? X { get; set; }
		public int? Y { get; set; }
		public SnailFishNumber Xpair { get; set; }
		public SnailFishNumber Ypair { get; set; }

		public void Add(object obj)
		{
			if (obj is SnailFishNumber addend)
			{
				Xpair = this switch
				{
					{ } when X != null		&& Y != null		=> new SnailFishNumber(X.Value, Y.Value),
					{ } when X != null		&& Ypair != null	=> new SnailFishNumber(X.Value, Ypair),
					{ } when Xpair != null	&& Y != null		=> new SnailFishNumber(Xpair, Y.Value),
					{ } when Xpair != null	&& Ypair != null	=> new SnailFishNumber(Xpair, Ypair),
					_ => Xpair
				};

				Ypair = addend;
			}
		}

		public void Reduce()
		{
			bool readyToExplode;
			bool readyToSplit;

			while (true)
			{
				readyToExplode = ContainsPairNestedAtLevel(DeepestNestingLevel);
				readyToSplit = ContainsValueGreaterThan(ThresholdValue);

				if (readyToExplode)
				{
					Explode();
				}
				else if (readyToSplit)
				{
					Split();
				}
				else
				{
					return;
				}
			}
		}

		public void Explode()
		{
			XY leftmostPairNestedInsideFourPairs = GetLeftmostPairNestedAtLevel(DeepestNestingLevel);

			UpdateValueLeftOf(leftmostPairNestedInsideFourPairs);
			UpdateValueRightOf(leftmostPairNestedInsideFourPairs);
			UpdateNumberContaining(leftmostPairNestedInsideFourPairs);
		}

		public void UpdateValueLeftOf(XY pair)
		{
			//TODO
		}

		public void UpdateValueRightOf(XY pair)
		{
			//TODO
		}

		public void UpdateNumberContaining(XY pair)
		{
			var number = GetLeftmostNumberContaining(pair);

			if (number.Xpair?.X == pair.X &&
				number.Xpair?.Y == pair.Y)
			{
				number.X = 0;
				number.Xpair = null;
			}
			else if (
				number.Ypair?.X == pair.X &&
				number.Ypair?.Y == pair.Y)
			{
				number.Y = 0;
				number.Ypair = null;
			}
		}

		public SnailFishNumber GetLeftmostNumberContaining(XY pair)
		{
			if (Xpair?.X == pair.X &&
				Xpair?.Y == pair.Y)
			{
				return this;
			}

			if (Ypair?.X == pair.X &&
				Ypair?.Y == pair.Y)
			{
				return this;
			}

			var matchInXpair = Xpair != null
				? Xpair.GetLeftmostNumberContaining(pair)
				: null;

			var matchInYPair = Ypair != null
				? Ypair.GetLeftmostNumberContaining(pair)
				: null;

			return matchInXpair ?? matchInYPair;
		}

		public void Split()
		{
			var number = GetLeftmostNumberContainingValueGreaterThan(ThresholdValue);
			decimal half;

			if (number.X > ThresholdValue)
			{
				half = number.X.Value / 2;

				number.Xpair = new SnailFishNumber((int)Math.Floor(half), (int)Math.Ceiling(half));
				number.X = null;
			}
			else if (number.Y > ThresholdValue)
			{
				half = number.Y.Value / 2;

				number.Ypair = new SnailFishNumber((int)Math.Floor(half), (int)Math.Ceiling(half));
				number.Y = null;
			}
		}

		public SnailFishNumber GetLeftmostNumberContainingValueGreaterThan(int value)
		{
			if (X == value || Y == value) return this;

			var matchInXpair = Xpair != null
				? Xpair.GetLeftmostNumberContainingValueGreaterThan(value)
				: null;

			var matchInYpair = Ypair != null
				? Ypair.GetLeftmostNumberContainingValueGreaterThan(value)
				: null;

			return matchInXpair ?? matchInYpair;
		}

		public int GetMagnitude()
		{
			//TODO
			return -1;
		}

		public XY GetLeftmostPairNestedAtLevel(int level)
		{
			if (level == 0)
			{
				if (X == null || Y == null)
				{
					return null;
				}

				return new XY(X.Value, Y.Value);
			}

			if (Xpair == null && Ypair == null)
			{
				return null;
			}

			var matchInXpair = Xpair?.GetLeftmostPairNestedAtLevel(level - 1);
			var matchInYpair = Ypair?.GetLeftmostPairNestedAtLevel(level - 1);

			return matchInXpair ?? matchInYpair;
		}

		public bool ContainsPairNestedAtLevel(int level)
		{
			if (level == 0)
			{
				return true; // should also check if this is valid snail fish number? (X, Y) / (X, Ypair) / (Ypair, X) / (Ypair, Xpair)
			}

			if (Xpair == null && Ypair == null)
			{
				return false;
			}

			return
				(Xpair != null && Xpair.ContainsPairNestedAtLevel(level - 1)) ||
				(Ypair != null && Ypair.ContainsPairNestedAtLevel(level - 1));
		}

		public bool ContainsValueGreaterThan(int value)
		{
			return
				X > value ||
				Y > value ||
				(Xpair != null && Xpair.ContainsValueGreaterThan(value)) ||
				(Ypair != null && Ypair.ContainsValueGreaterThan(value));
		}
	}
}
