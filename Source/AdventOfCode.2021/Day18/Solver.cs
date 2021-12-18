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
			bool containsPairNestedAtLevel4;
			bool containsRegularNumberGreaterThan9;

			while (true)
			{
				containsPairNestedAtLevel4 = ContainsPairNestedAtLevel(4);
				containsRegularNumberGreaterThan9 = ContainsRegularNumberGreaterThan(9);

				if (containsPairNestedAtLevel4)
				{
					Explode();
				}
				else if (containsRegularNumberGreaterThan9)
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
			//TODO
		}

		public void Split()
		{
			//TODO
		}

		public int GetMagnitude()
		{
			//TODO
			return -1;
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

		public bool ContainsRegularNumberGreaterThan(int number)
		{
			return
				X > number ||
				Y > number ||
				(Xpair != null && Xpair.ContainsRegularNumberGreaterThan(number)) ||
				(Ypair != null && Ypair.ContainsRegularNumberGreaterThan(number));
		}
	}
}
