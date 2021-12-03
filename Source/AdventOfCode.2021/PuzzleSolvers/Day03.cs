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
			Part2Solution = SolvePart2(input);
		}

		private static decimal SolvePart1(string[] diagnosticReport)
		{
			var binaryNumberSize = diagnosticReport.First().Length;
			List<int> gammaRateBits = new List<int>();

			for (var i = 0; i < binaryNumberSize; i++)
			{
				gammaRateBits.Add(
					GetMostCommonBit(diagnosticReport
						.Select(entry => entry[i])));
			}

			var epsilonRateBits = ReverseBitValues(gammaRateBits);

			decimal powerConsumption =
				gammaRateBits.ToDecimal() * epsilonRateBits.ToDecimal();

			return powerConsumption;
		}

		private static decimal SolvePart2(string[] diagnosticReport)
		{

		}

		private static int GetMostCommonBit(IEnumerable<char> bits)
		{
			return int.Parse(bits
				.GroupBy(b => b)
				.OrderByDescending(b => b.Count())
				.First().Key.ToString());
		}

		private static List<int> ReverseBitValues(IEnumerable<int> bits)
		{
			return bits
				.Select(b => b == 0 ? 1 : 0)
				.ToList();
		}
	}

	static class Day03Helpers
	{
		public static decimal ToDecimal(this List<int> bits)
		{
			var bitsTemp = bits.ToList();
			bitsTemp.Reverse();

			decimal temp = 0;

			for (var i = 0; i < bitsTemp.Count(); i++)
			{
				temp += i == 0
					? bitsTemp[i]
					: (decimal)Math.Pow(2 * bitsTemp[i], i);
			}

			return (decimal)temp;
		}
	}
}
