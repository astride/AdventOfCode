using Common.Interfaces;

namespace Year2024;

public class Day11Solver : IPuzzleSolver
{
	public string Title => "Plutonian Pebbles";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

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
			if (stone == "0")
			{
				yield return "1";
			}
			else if (stone.Length % 2 == 0)
			{
				yield return string.Join(string.Empty, stone[..(stone.Length / 2)]);

				var rightHalf = long.Parse(stone[(stone.Length / 2)..]);

				yield return rightHalf.ToString();
			}
			else
			{
				yield return (long.Parse(stone) * 2024).ToString();
			}
		}
	}
}