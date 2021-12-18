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

		public void Add(object obj) //CORRECT
		{
			if (obj is SnailFishNumber addend)
			{
				Console.WriteLine("\n------------- Adding ---------------");

				Xpair = this switch
				{
					{ } when X != null && Y != null => new SnailFishNumber(X.Value, Y.Value),
					{ } when X != null && Ypair != null => new SnailFishNumber(X.Value, Ypair),
					{ } when Xpair != null && Y != null => new SnailFishNumber(Xpair, Y.Value),
					{ } when Xpair != null && Ypair != null => new SnailFishNumber(Xpair, Ypair),
					_ => Xpair
				};

				Ypair = addend;

				Print();
			}
		}

		public void Reduce()
		{
			SnailFishNumber numberReadyToExplode;
			SnailFishNumber numberReadyToSplit;

			while (true)
			{
				numberReadyToExplode = GetLeftmostNumberContainingPairNestedAtLevel(DeepestNestingLevel);
				numberReadyToSplit = GetLeftmostNumberContainingValueGreaterThan(ThresholdValue);

				if (numberReadyToExplode != null)
				{
					Explode(numberReadyToExplode);
				}
				else if (numberReadyToSplit != null)
				{
					Split(numberReadyToSplit);
				}
				else
				{
					Console.WriteLine("\n\nReduction finished.");
					return;
				}

				Print();
			}
		}

		public int GetMagnitude()
		{
			return
				3 * (X ?? Xpair.GetMagnitude()) +
				2 * (Y ?? Ypair.GetMagnitude());
		}

		public void Print()
		{
			PrintValues();
			Console.WriteLine();
		}
		
		public void PrintValues()
		{
			Console.Write(PairOpeningChar);

			if (X.HasValue) Console.Write(X);
			else Xpair.PrintValues();

			Console.Write(ValueSeparator);

			if (Y.HasValue) Console.Write(Y);
			else Ypair.PrintValues();

			Console.Write(PairClosingChar);
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

		private void Explode(SnailFishNumber number)
		{
			Console.WriteLine($"\n\nEXPLODING number [{number.X},{number.Y}]");

			UpdateValueLeftOf(number);
			UpdateValueRightOf(number);
			UpdateNumberContaining(number);
		}

		private SnailFishNumber GetLeftmostNumberContainingPairNestedAtLevel(int level)
		{
			if (level == 0)
			{
				if (X == null || Y == null)
				{
					return null;
				}

				return this;
			}

			if (Xpair == null && Ypair == null)
			{
				return null;
			}

			return 
				Xpair?.GetLeftmostNumberContainingPairNestedAtLevel(level - 1) ??
				Ypair?.GetLeftmostNumberContainingPairNestedAtLevel(level - 1);
		}

		private void UpdateValueLeftOf(SnailFishNumber number)
		{
			var parent = GetParentOf(number);

			var numberIsXpair = parent?.Xpair?.X == number.X && parent?.Xpair?.Y == number.Y;
			var numberIsYpair = parent?.Ypair?.X == number.X && parent?.Ypair?.Y == number.Y;

			if (!numberIsXpair && !numberIsYpair) return;

			if (numberIsXpair)
			{
				var ancestor = GetNearestAncestorWhereNumberIsDescendantOfYpair(parent);

				if (ancestor == null) return;

				if (ancestor.X != null)
				{
					ancestor.X += number.X;
					return;
				}

				if (ancestor.Xpair == null) return;

				if (ancestor.Xpair.Y != null)
				{
					ancestor.Xpair.Y += number.X;
					return;
				}

				var descendant = ancestor?.Xpair?.GetDescendantToUpdateYvalueOf();

				if (descendant == null) return;

				if (descendant.Y != null)
				{
					descendant.Y += number.X;
					return;
				}
			}
			else if (numberIsYpair)
			{
				if (parent.X != null)
				{
					parent.X += number.X;
					return;
				}

				if (parent.Xpair == null) return;

				if (parent.Xpair.Y != null)
				{
					parent.Xpair.Y += number.X;
					return;
				}

				var descendant = parent.Xpair.GetDescendantToUpdateYvalueOf();

				if (descendant == null) return;

				if (descendant.Y != null)
				{
					descendant.Y += number.X;
					return;
				}
			}
		}

		private void UpdateValueRightOf(SnailFishNumber number)
		{
			var parent = GetParentOf(number);

			var numberIsXpair = parent?.Xpair?.X == number.X && parent?.Xpair?.Y == number.Y;
			var numberIsYpair = parent?.Ypair?.X == number.X && parent?.Ypair?.Y == number.Y;

			if (!numberIsXpair && !numberIsYpair) return;

			if (numberIsXpair)
			{
				if (parent.Y != null)
				{
					parent.Y += number.Y;
					return;
				}

				if (parent.Ypair == null) return;

				if (parent.Ypair.X != null)
				{
					parent.Ypair.X += number.Y;
					return;
				}

				var descendant = parent.Ypair.GetDescendantToUpdateXvalueOf();

				if (descendant == null) return;

				if (descendant.X != null)
				{
					descendant.X += number.Y;
					return;
				}
			}
			else if (numberIsYpair)
			{
				var ancestor = GetNearestAncestorWhereNumberIsDescendantOfXpair(parent);

				if (ancestor == null) return;

				if (ancestor.Y != null)
				{
					ancestor.Y += number.Y;
					return;
				}

				if (ancestor.Ypair == null) return;

				if (ancestor.Ypair.X != null)
				{
					ancestor.Ypair.X += number.Y;
					return;
				}

				var descendant = ancestor?.Ypair?.GetDescendantToUpdateXvalueOf();

				if (descendant == null) return;

				if (descendant.X != null)
				{
					descendant.X += number.Y;
					return;
				}
			}
		}

		private SnailFishNumber GetNearestAncestorWhereNumberIsDescendantOfYpair(SnailFishNumber number)
		{
			// find innermost ancestor where number is a descendant of Ypair
			if (!ParentExistsFor(number)) return null;

			if (GetParentOf(number).Ypair == number) return GetParentOf(number);

			return GetNearestAncestorWhereNumberIsDescendantOfYpair(GetParentOf(number));
		}

		private SnailFishNumber GetNearestAncestorWhereNumberIsDescendantOfXpair(SnailFishNumber number)
		{
			// find innermost ancestor where number is a descendant of Xpair
			if (!ParentExistsFor(number)) return null;
			
			if (GetParentOf(number).Xpair == number) return GetParentOf(number);

			return GetNearestAncestorWhereNumberIsDescendantOfXpair(GetParentOf(number));
		}

		private bool ParentExistsFor(SnailFishNumber number)
		{
			if (Xpair == null && Ypair == null) return false;

			if (Xpair == number || Ypair == number) return true;

			return 
				(Xpair != null && Xpair.ParentExistsFor(number)) ||
				(Ypair != null && Ypair.ParentExistsFor(number));
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

			return Xpair?.GetParentOf(number) ?? Ypair?.GetParentOf(number);
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
			if (Xpair != null) return Xpair.GetDescendantToUpdateXvalueOf();

			return null;
		}

		private void UpdateNumberContaining(SnailFishNumber number)
		{
			var parent = GetParentOf(number);

			if (parent == null) return;

			if (parent.Xpair?.X == number.X &&
				parent.Xpair?.Y == number.Y)
			{
				parent.X = 0;
				parent.Xpair = null;
			}
			else if (
				parent.Ypair?.X == number.X &&
				parent.Ypair?.Y == number.Y)
			{
				parent.Y = 0;
				parent.Ypair = null;
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

		private void Split(SnailFishNumber number)
		{
			decimal half;

			if (number.X > ThresholdValue)
			{
				Console.WriteLine("\n\nSPLITTING number " + number.X);
				
				half = (decimal)number.X.Value / 2;

				number.Xpair = new SnailFishNumber((int)Math.Floor(half), (int)Math.Ceiling(half));
				number.X = null;
			}
			else if (number.Y > ThresholdValue)
			{
				Console.WriteLine("\n\nSPLITTING number " + number.Y);

				half = (decimal)number.Y.Value / 2;

				number.Ypair = new SnailFishNumber((int)Math.Floor(half), (int)Math.Ceiling(half));
				number.Y = null;
			}
		}

		private SnailFishNumber GetLeftmostNumberContainingValueGreaterThan(int value)
		{
			if (X > value || Y > value) return this;

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
