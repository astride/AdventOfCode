using Common.Interfaces;

namespace Year2024;

public class Day11Solver : IPuzzleSolver
{
	public string Title => "Plutonian Pebbles";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }
	
	private static readonly Dictionary<string, string[]> StonesAfterBlinkingByStone = new();

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var stones = input.Single().Split(" ").AsEnumerable();

		for (var i = 0; i < 25; i++)
		{
			stones = GetStonesAfterBlinking(stones);
		}

		return stones.Count();
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}

	private static IEnumerable<string> GetStonesAfterBlinking(IEnumerable<string> stones)
	{
		foreach (var stone in stones)
		{
			if (!StonesAfterBlinkingByStone.TryGetValue(stone, out var stonesAfterBlinking))
			{
				stonesAfterBlinking = CalculateStonesAfterBlinking();

				StonesAfterBlinkingByStone[stone] = stonesAfterBlinking;
			}

			foreach (var stoneAfter in stonesAfterBlinking)
			{
				yield return stoneAfter;
			}

			continue;

			string[] CalculateStonesAfterBlinking()
			{
				if (stone == "0")
				{
					return new[] { "1" };
				}

				if (stone.Length % 2 == 1)
				{
					return new[] { (long.Parse(stone) * 2024).ToString() };
				}

				var leftHalf = stone[..(stone.Length / 2)];
				var rightHalf = long.Parse(stone[(stone.Length / 2)..]).ToString();

				return new[] { leftHalf, rightHalf };
			}
		}
	}
}