﻿using AdventOfCode.Common;
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

			foreach (var i in Enumerable.Range(1, numbers.Count() - 1))
			{
				param.Add(numbers[i]);
				param.Reduce();
			}

			var magnitude = param.GetMagnitude();

			return magnitude;
		}
	}

	public class SnailFishNumber
	{
		public SnailFishNumber(string number) : this(GetXYStringsOf(number)) { }

		private static (string X, string Y) GetXYStringsOf(string number)
		{
			var content = number.Substring(1, number.Length - 2); // remove outer brackets

			var pairOpeningCharCount = 0;
			var pairClosingCharCount = 0;
			var i = 0;

			while (i < content.Length)
			{
				if (content[i] == ValueSeparator &&
					pairClosingCharCount == pairOpeningCharCount)
				{
					var xValue = content.Substring(0, i);
					var yValue = content.Substring(i + 1);

					return (xValue, yValue);
				}

				if (content[i] == PairOpeningChar) pairOpeningCharCount++;
				else if (content[i] == PairClosingChar) pairClosingCharCount++;

				i++;
			}

			return (null, null);
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
					{ } when X != null && Y != null => new SnailFishNumber(X.Value, Y.Value),
					{ } when X != null && Ypair != null => new SnailFishNumber(X.Value, Ypair),
					{ } when Xpair != null && Y != null => new SnailFishNumber(Xpair, Y.Value),
					{ } when Xpair != null && Ypair != null => new SnailFishNumber(Xpair, Ypair),
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

		public int GetMagnitude()
		{
			return
				3 * (X ?? Xpair.GetMagnitude()) +
				2 * (Y ?? Ypair.GetMagnitude());
		}

		private SnailFishNumber((string X, string Y) value)
		{
			var isX = int.TryParse(value.X, out var x);
			var isY = int.TryParse(value.Y, out var y);

			if (isX && isY)
			{
				X = x;
				Y = y;
			}
			else if (isX)
			{
				X = x;
				Ypair = new SnailFishNumber(value.Y);
			}
			else if (isY)
			{
				Xpair = new SnailFishNumber(value.X);
				Y = y;
			}
			else
			{
				Xpair = new SnailFishNumber(value.X);
				Ypair = new SnailFishNumber(value.Y);
			}
		}

		private SnailFishNumber(int x, int y)
		{
			X = x;
			Y = y;
		}

		private SnailFishNumber(int x, SnailFishNumber yPair)
		{
			X = x;
			Ypair = yPair;
		}

		private SnailFishNumber(SnailFishNumber xPair, int y)
		{
			Xpair = xPair;
			Y = y;
		}

		private SnailFishNumber(SnailFishNumber xPair, SnailFishNumber yPair)
		{
			Xpair = xPair;
			Ypair = yPair;
		}

		private const int DeepestNestingLevel = 4; // assuming there is never nesting beyond level 4
		private const int ThresholdValue = 9;
		private const char PairOpeningChar = '[';
		private const char PairClosingChar = ']';
		private const char ValueSeparator = ',';

		private bool ContainsPairNestedAtLevel(int level)
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

		private bool ContainsValueGreaterThan(int value)
		{
			return
				X > value ||
				Y > value ||
				(Xpair != null && Xpair.ContainsValueGreaterThan(value)) ||
				(Ypair != null && Ypair.ContainsValueGreaterThan(value));
		}

		private void Explode()
		{
			XY leftmostPairNestedInsideFourPairs = GetLeftmostPairNestedAtLevel(DeepestNestingLevel);

			UpdateValueLeftOf(leftmostPairNestedInsideFourPairs);
			UpdateValueRightOf(leftmostPairNestedInsideFourPairs);
			UpdateNumberContaining(leftmostPairNestedInsideFourPairs);
		}

		private XY GetLeftmostPairNestedAtLevel(int level)
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

		private void UpdateValueLeftOf(XY pair)
		{
			var numberContainingPair = GetLeftmostNumberContaining(pair);

			var pairIsXpair = numberContainingPair?.Xpair?.X == pair.X && numberContainingPair?.Xpair?.Y == pair.Y;
			var pairIsYpair = numberContainingPair?.Ypair?.X == pair.X && numberContainingPair?.Ypair?.Y == pair.Y;

			if (pairIsXpair)
			{
				// find closest ancestor where Xpair is descendant of Ypair
				var ancestor = GetInnermostAncestorWhereXpairIsDescendantOfYpair(numberContainingPair);
				if (ancestor == null) return;

				var descendant = ancestor.GetDescendantToUpdateYvalueOf();
				if (descendant == null) return;

				if (descendant.Y != null)
				{
					descendant.Y += pair.X; 
				}
				else if (descendant.Ypair != null)
				{
					descendant.Ypair.Y += pair.X;
				}
			}
			else if (pairIsYpair)
			{
				if (numberContainingPair?.X != null)
				{
					numberContainingPair.X += pair.X;
				}
				else if (numberContainingPair?.Xpair != null)
				{
					numberContainingPair.Xpair.Y += pair.X;
				}
			}
		}

		private void UpdateValueRightOf(XY pair)
		{
			var numberContainingPair = GetLeftmostNumberContaining(pair);

			var pairIsXpair = numberContainingPair?.Xpair?.X == pair.X && numberContainingPair?.Xpair?.Y == pair.Y;
			var pairIsYpair = numberContainingPair?.Ypair?.X == pair.X && numberContainingPair?.Ypair?.Y == pair.Y;

			if (pairIsXpair)
			{
				if (numberContainingPair?.Y != null)
				{
					numberContainingPair.Y += pair.Y;
				}
				else if (numberContainingPair?.Ypair != null)
				{
					numberContainingPair.Ypair.X += pair.Y;
				}
			}
			else if (pairIsYpair)
			{
				// find closest ancestor where Ypair is descendant of Xpair
				var ancestor = GetInnermostAncestorWhereNumberIsDescendantOfXpair(numberContainingPair);
				if (ancestor == null) return;

				var descendant = ancestor.GetDescendantToUpdateXvalueOf();
				if (descendant == null) return;

				if (descendant.X != null)
				{
					descendant.X += pair.Y;
				}
				else if (descendant.Xpair != null)
				{
					descendant.Xpair.X += pair.Y;
				}
			}
		}

		private SnailFishNumber GetInnermostAncestorWhereXpairIsDescendantOfYpair(SnailFishNumber number)
		{
			// find innermost ancestor where number is a descendant of Ypair
			if (!ParentExistsFor(number)) return null;

			if (GetParentOf(number).Ypair == number) return GetParentOf(number);

			return GetInnermostAncestorWhereXpairIsDescendantOfYpair(GetParentOf(number));
		}

		private SnailFishNumber GetInnermostAncestorWhereNumberIsDescendantOfXpair(SnailFishNumber number)
		{
			// find innermost ancestor where number is a descendant of Xpair
			if (!ParentExistsFor(number)) return null;
			
			if (GetParentOf(number).Xpair == number) return GetParentOf(number);

			return GetInnermostAncestorWhereNumberIsDescendantOfXpair(GetParentOf(number));
		}

		private bool ParentExistsFor(SnailFishNumber number)
		{
			if (Xpair == null && Ypair == null) return false;

			if (Xpair == number || Ypair == number) return true;

			return Xpair.ParentExistsFor(number) || Ypair.ParentExistsFor(number);
		}

		private bool ContainsDescendant(SnailFishNumber descendant)
		{
			if (Xpair == null && Ypair == null) return false;

			if (Xpair == descendant || Ypair == descendant) return true;

			return Xpair.ContainsDescendant(descendant) || Ypair.ContainsDescendant(descendant);
		}

		private SnailFishNumber GetParentOf(SnailFishNumber number)
		{
			if (Xpair == null && Ypair == null) return null;

			if (Xpair == number || Ypair == number) return this;

			return Xpair.GetParentOf(number) ?? Ypair.GetParentOf(number);
		}

		private SnailFishNumber GetDescendantToUpdateYvalueOf()
		{
			// find desc to update Y value of: drill down through Ypair of descs until reaching desc with Y value
			if (Y != null) return this;
			if (Ypair != null) return Ypair.GetDescendantToUpdateYvalueOf();

			return null;
		}

		private SnailFishNumber GetDescendantToUpdateXvalueOf()
		{
			// find desc to update X value of: drill down through Xpair of descs until reaching desc with X value
			if (X != null) return this;
			if (Xpair != null) return Xpair.GetDescendantToUpdateYvalueOf();

			return null;
		}

		private void UpdateNumberContaining(XY pair)
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

		private SnailFishNumber GetLeftmostNumberContaining(XY pair)
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

		private void Split()
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

		private SnailFishNumber GetLeftmostNumberContainingValueGreaterThan(int value)
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
	}
}
