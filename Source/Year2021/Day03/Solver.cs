using Common.Interfaces;

namespace Year2021;

public class Day03Solver : IPuzzleSolver
{
	public string Title => "Binary Diagnostic";
	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input)
	{
		var diagnosticReport = GetDiagnosticReport(input);
		
		var bitCount = diagnosticReport.First().Length;
		
		var gammaRateBinary = new List<int>();

		for (var i = 0; i < bitCount; i++)
		{
			var nthPositionBits = diagnosticReport
				.Select(entry => entry[i].ToBit());

			gammaRateBinary.Add(nthPositionBits.MostCommonBitOr1());
		}

		var epsilonRateBinary = gammaRateBinary.FlipBitValues();

		decimal powerConsumption = gammaRateBinary.ToDecimal() * epsilonRateBinary.ToDecimal();

		return powerConsumption;
	}

	public object GetPart2Solution(string[] input)
	{
		var diagnosticReport = GetDiagnosticReport(input);
		
		var binaryOxygenGeneratorRating = diagnosticReport.BinaryRating(OxygenCriteriaValidator);
		var binaryCo2ScrubberRating = diagnosticReport.BinaryRating(Co2CriteriaValidator);

		decimal lifeSupportRating = binaryOxygenGeneratorRating.ToDecimal() * binaryCo2ScrubberRating.ToDecimal();

		return lifeSupportRating;
	}

	private string[] GetDiagnosticReport(string[] input)
	{
		return input
			.Where(entry => !string.IsNullOrWhiteSpace(entry))
			.ToArray();
	}

	private static bool OxygenCriteriaValidator(int bit, IEnumerable<int> bits) => bit == bits.MostCommonBitOr1();
	private static bool Co2CriteriaValidator(int bit, IEnumerable<int> bits) => bit == bits.LeastCommonBitOr0();
}

internal static class Day03Helpers
{
	public static int ToBit(this char ch) => int.Parse(ch.ToString());
	
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

		for (var i = 0; i < sbReversed.Count; i++)
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
				.Select(entry => entry[i].ToBit());

			diagnostics = diagnostics.Where(entry => criteriaValidator(entry[i].ToBit(), nthPositionBits)).ToList();

			if (diagnostics.Count == 1)
			{
				var rating = diagnostics.Single();

				return rating.Select(ch => ch.ToBit()).ToArray();
			}
		}

		// Will (in theory) never be reached
		return Array.Empty<int>();
	}

	private static IOrderedEnumerable<IGrouping<int, int>> GroupedAndOrderedByAscending(this IEnumerable<int> collection)
	{
		return collection
			.GroupBy(b => b)
			.OrderBy(b => b.Count())
			.ThenBy(b => b.Key);
	}
}
