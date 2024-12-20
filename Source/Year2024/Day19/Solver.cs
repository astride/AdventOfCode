using Common.Interfaces;

namespace Year2024;

public class Day19Solver : IPuzzleSolver
{
	public string Title => string.Empty;

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var towelPatternsByFirstChar = input.First()
			.Split(',', StringSplitOptions.TrimEntries)
			.GroupBy(pattern => pattern[0])
			.ToDictionary(group => group.Key);

		var maxTowelPatternLengthByFirstChar = towelPatternsByFirstChar
			.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Max(pattern => pattern.Length));

		var desiredDesigns = input[2..];

		var possibleDesigns = desiredDesigns.Where(DesignIsPossible);

		return possibleDesigns.Count();

		bool DesignIsPossible(string design)
		{
			var nextIndicesToCheck = new List<int> { 0 };
			var indicesMarkedForChecking = new HashSet<int> { 0 };

			while (nextIndicesToCheck.Count > 0)
			{
				var indexToCheck = nextIndicesToCheck.Min();
				nextIndicesToCheck.Remove(indexToCheck);

				var firstPatternChar = design[indexToCheck];

				if (!maxTowelPatternLengthByFirstChar.TryGetValue(firstPatternChar, out var maxPatternLength))
				{
					continue;
				}

				for (var iEnd = 0; iEnd <= maxPatternLength; iEnd++)
				{
					var nextIndexNotIncludedInPattern = indexToCheck + iEnd + 1;

					if (indexToCheck + iEnd == design.Length)
					{
						break;
					}
					
					var pattern = design[indexToCheck..nextIndexNotIncludedInPattern];

					if (towelPatternsByFirstChar[firstPatternChar].Contains(pattern))
					{
						if (indexToCheck + iEnd == design.Length - 1)
						{
							Console.WriteLine("Possible design: " + design);
							return true;
						}

						if (indicesMarkedForChecking.Add(nextIndexNotIncludedInPattern))
						{
							nextIndicesToCheck.Add(nextIndexNotIncludedInPattern);
						}
					}
				}
			}

			return false;
		}
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}
}