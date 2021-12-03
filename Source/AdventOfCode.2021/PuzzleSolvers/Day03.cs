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
			var bitCount = diagnosticReport.First().Length;
			
			IEnumerable<int> nthPositionBits;
			List<int> gammaRateBinary = new List<int>();

			for (var i = 0; i < bitCount; i++)
			{
				nthPositionBits = diagnosticReport
					.Select(entry => entry[0].ToString())
					.Select(entry => int.Parse(entry));

				gammaRateBinary.Add(nthPositionBits.MostCommonBitOr1());
			}

			var epsilonRateBinary = gammaRateBinary.FlipBitValues();

			decimal powerConsumption = gammaRateBinary.ToDecimal() * epsilonRateBinary.ToDecimal();

			return powerConsumption;
		}

		private static decimal SolvePart2(string[] diagnosticReport)
		{
			var binaryOxygenGeneratorRating = diagnosticReport.BinaryRating(OxygenCriteriaValidator);
			var binaryCo2ScrubberRating = diagnosticReport.BinaryRating(Co2CriteriaValidator);

			decimal lifeSupportRating = binaryOxygenGeneratorRating.ToDecimal() * binaryCo2ScrubberRating.ToDecimal();

			return lifeSupportRating;
		}

		private static bool OxygenCriteriaValidator(int bit, IEnumerable<int> bits) => bit == bits.MostCommonBitOr1();
		private static bool Co2CriteriaValidator(int bit, IEnumerable<int> bits) => bit == bits.LeastCommonBitOr0();
	}

	static class Day03Helpers
	{
		public static int LeastCommonBitOr0(this IEnumerable<int> bits)
		{
			return bits
				.GroupedAndOrderedByAscending()
				.First().Key;
		}

		public static int MostCommonBitOr1(this IEnumerable<int> bits)
		{
			return bits
				.GroupedAndOrderedByAscending()
				.Reverse()
				.First().Key;
		}

		public static IOrderedEnumerable<IGrouping<int, int>> GroupedAndOrderedByAscending(this IEnumerable<int> collection)
		{
			return collection
				.GroupBy(b => b)
				.OrderBy(b => b.Count())
				.ThenBy(b => b.Key);
		}

		public static int[] FlipBitValues(this IList<int> bits)
		{
			return bits
				.Select(b => b == 0 ? 1 : 0)
				.ToArray();
		}

		public static decimal ToDecimal(this IList<int> bits)
		{
			decimal value = bits.Last(); // value = least significant bit (lsb)

			var sbReversed = bits
				.Reverse()
				.Skip(1) // exclude lsb
				.ToList();

			for (var i = 0; i < sbReversed.Count(); i++)
			{
				value += (decimal)Math.Pow(2 * sbReversed[i], i + 1);
			}

			return value;
		}

		public static int[] BinaryRating(this string[] report, Func<int, IEnumerable<int>, bool> criteriaValidator)
		{
			IEnumerable<int> nthPositionBits;

			var diagnostics = report.ToList();

			for (var i = 0; i < report.First().Length; i++)
			{
				nthPositionBits = diagnostics
					.Select(entry => int.Parse(entry[i].ToString()));

				diagnostics = diagnostics.Where(entry => criteriaValidator(int.Parse(entry[i].ToString()), nthPositionBits)).ToList();

				if (diagnostics.Count() == 1)
				{
					var rating = diagnostics.Single();

					return rating.Select(ch => int.Parse(ch.ToString())).ToArray();
				}
			}

			// Will (in theory) never be reached
			return Array.Empty<int>();
		}
	}
}
