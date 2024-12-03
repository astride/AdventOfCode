using Common.Interfaces;

namespace Year2024;

public class Day01Solver : IPuzzleSolver
{
	public string Title => "Historian Hysteria";
	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }
	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var pairs = input
			.Select(line => line.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries))
			.ToList();

		var leftColumn = new int[input.Length];
		var rightColumn = new int[input.Length];

		for (var i = 0; i < pairs.Count; i++)
		{
			leftColumn[i] = int.Parse(pairs[i][0]);
			rightColumn[i] = int.Parse(pairs[i][1]);
		}

		var leftColumnSorted = leftColumn.OrderBy(value => value).ToList();
		var rightColumnSorted = rightColumn.OrderBy(value => value).ToList();

		var totalDistance = 0;

		for (var i = 0; i < pairs.Count; i++)
		{
			totalDistance += Math.Abs(leftColumnSorted[i] - rightColumnSorted[i]);
		}

		return totalDistance;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var pairs = input
			.Select(line => line.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries))
			.ToList();

		var leftCountByValue = new Dictionary<string, int>();
		var rightCountByValue = new Dictionary<string, int>();

		foreach (var pair in pairs)
		{
			var leftValue = pair[0];
			var rightValue = pair[1];

			if (!leftCountByValue.TryAdd(leftValue, 1))
			{
				leftCountByValue[leftValue]++;
			}

			if (!rightCountByValue.TryAdd(rightValue, 1))
			{
				rightCountByValue[rightValue]++;
			}
		}

		var similarityScore = 0;

		foreach (var kvp in leftCountByValue)
		{
			if (rightCountByValue.TryGetValue(kvp.Key, out var rightCount))
			{
				similarityScore += int.Parse(kvp.Key) * kvp.Value * rightCount;
			}
		}

		return similarityScore;
	}
}