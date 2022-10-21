using Common.Interfaces;

namespace Year2021;

public class Day14Solver : IPuzzleSolver
{
	public string Part1Solution { get; set; } = string.Empty;
	public string Part2Solution { get; set; } = string.Empty;

	public void SolvePuzzle(string[] rawInput)
	{
		var polymerTemplate = rawInput.First();

		IDictionary<string, char> charToAddForPair = rawInput
			.Skip(2)
			.Select(entry => entry.Split(' '))
			.ToDictionary(
				rule => rule.First(), 
				rule => rule.Last().Single());

		Part1Solution = SolvePart1(polymerTemplate, charToAddForPair).ToString();
		Part2Solution = SolvePart2(polymerTemplate, charToAddForPair).ToString();
	}

	private static int SolvePart1(string polymerTemplate, IDictionary<string, char> charToAddForPair)
	{
		var resultForPair = charToAddForPair.GetResultForPairDict();

		foreach (var _ in Enumerable.Range(0, 10))
		{
			polymerTemplate = polymerTemplate.GetNextPolymer(resultForPair);
		}

		var countPerElement = polymerTemplate
			.GroupBy(ch => ch)
			.Select(gr => gr.Count());

		return countPerElement.Max() - countPerElement.Min();
	}

	private static double SolvePart2(string polymerTemplate, IDictionary<string, char> charToAddForPair)
	{
		charToAddForPair.PrepareDictionaries();

		var polymerConfig = polymerTemplate.PreparePolymer();

		foreach (var _ in Enumerable.Range(1, 40))
		{
			polymerConfig = polymerConfig.InsertPairs();
		}

		var elementCount = polymerConfig.GetElementCount();

		return elementCount.Max() - elementCount.Min();
	}
}

public static class Day14Helpers
{
	static IDictionary<string, (string Pair1, string Pair2)> ResultOfPair;
	static IEnumerable<char> Chars;

	#region Part 1
	public static IDictionary<string, string> GetResultForPairDict(this IDictionary<string, char> charCreatedFromPair)
	{
		return charCreatedFromPair
			.ToDictionary(
				entry => entry.Key,
				entry => string.Concat(new char[] { entry.Key.First(), entry.Value }));
	}

	public static string GetNextPolymer(this string polymerTemplate, IDictionary<string, string> resultForPair)
	{
		var results = new List<string>();

		foreach (var i in Enumerable.Range(0, polymerTemplate.Length - 1))
		{
			results.Add(resultForPair[polymerTemplate.Substring(i, 2)]);
		}

		results.Add(polymerTemplate.Last().ToString());

		return string.Concat(results);
	}
	#endregion

	#region Part 2
	public static void PrepareDictionaries(this IDictionary<string, char> charToAddForPair)
	{
		Chars = charToAddForPair
			.Select(kvp => kvp.Value)
			.Distinct();

		ResultOfPair = charToAddForPair
			.ToDictionary(
				entry => entry.Key,
				entry => (
					(string.Concat(new char[] { entry.Key.First(), entry.Value }),
					string.Concat(new char[] { entry.Value, entry.Key.Last() }))));
	}

	public static IDictionary<string, double> PreparePolymer(this string template)
	{
		var polymerConfig = ResultOfPair
			.ToDictionary(
				entry => entry.Key,
				entry => (double)0);

		foreach (var i in Enumerable.Range(0, template.Length - 1))
		{
			polymerConfig[template.Substring(i, 2)]++;
		}

		return polymerConfig;
	}

	public static IDictionary<string, double> InsertPairs(this IDictionary<string, double> polymerConfig)
	{
		var updatedConfig = polymerConfig
			.ToDictionary(
				entry => entry.Key,
				entry => (double)0);

		(string Pair1, string Pair2) resultOfPair;

		foreach (var pair in polymerConfig.Where(entry => entry.Value > 0))
		{
			resultOfPair = ResultOfPair[pair.Key];

			updatedConfig[resultOfPair.Pair1] += pair.Value;
			updatedConfig[resultOfPair.Pair2] += pair.Value;
		}

		return updatedConfig;
	}

	public static IEnumerable<double> GetElementCount(this IDictionary<string, double> polymerConfig)
	{
		var charCount = Chars
			.ToDictionary(
				ch => ch,
				ch => (double)0);

		foreach (var ch in Chars)
		{
			charCount[ch] = polymerConfig
				.Where(config => config.Key.Contains(ch))
				.Select(configWithCh => configWithCh.Value)
				.Sum() / 2;

			charCount[ch] += polymerConfig
				.Single(config => config.Key.All(configCh => configCh == ch))
				.Value / 2;
		}

		return charCount.Select(entry => entry.Value);
	}
	#endregion
}
