using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.PuzzleSolvers
{
	class Day03 : IPuzzleSolver<decimal>
	{
		public decimal Part1Solution { get; set; }
		public decimal Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var input = rawInput
				.Where(entry => !string.IsNullOrWhiteSpace(entry))
				.ToArray();

			Part1Solution = SolvePart1(input);
		}

		private static decimal SolvePart1(string[] diagnosticReport)
		{
			var bitCount = diagnosticReport.First().Length;
			
			IEnumerable<char> nthPositionBits;
			List<int> gammaRateBinary = new List<int>();

			for (var i = 0; i < bitCount; i++)
			{
				nthPositionBits = diagnosticReport.Select(entry => entry[i]).ToList();

				gammaRateBinary.Add(nthPositionBits.MostCommonBit());
			}

			var epsilonRateBinary = gammaRateBinary.ReverseBits();

			decimal powerConsumption = gammaRateBinary.ToDecimal() * epsilonRateBinary.ToDecimal();

			return powerConsumption;
		}
	}

	static class Day03Helpers
	{
		public static int MostCommonBit(this IEnumerable<char> bits)
		{
			return int.Parse(bits
				.GroupBy(b => b)
				.OrderByDescending(b => b.Count())
				.First().Key.ToString());
		}

		public static int[] ReverseBits(this IList<int> bits)
		{
			return bits
				.Select(b => b == 0 ? 1 : 0)
				.ToArray();
		}

		public static decimal ToDecimal(this IList<int> bits)
		{
			var lsb = bits.Last();

			var sbReversed = bits.Take(bits.Count() - 1).ToList(); //skip least significant bit (lsb)
			sbReversed.Reverse();

			decimal value = lsb;

			for (var i = 0; i < sbReversed.Count(); i++)
			{
				value += (decimal)Math.Pow(2 * sbReversed[i], i + 1);
			}

			return value;
		}
	}
}
