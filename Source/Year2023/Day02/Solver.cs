using System.Text.RegularExpressions;
using Common.Interfaces;

namespace Year2023;

public class Day02Solver : IPuzzleSolver
{
	public string Title => "Cube Conundrum";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private static readonly Regex NumberRegex = new(@"\d+");
	private static readonly Regex WordRegex = new(@"[a-z]+");
	private static readonly Regex RevealedCubeRegex = new(@"(\d+ [a-z]+)");

	private static readonly Dictionary<string, int> MaxCubeCountByColor = new()
	{
		["blue"] = 14,
		["green"] = 13,
		["red"] = 12,
	};

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var idSum = 0;

		foreach (var gameDescription in input)
		{
			var gameIdAndRevealedCubes = gameDescription.Split(':', StringSplitOptions.TrimEntries);

			var revealedCubes = gameIdAndRevealedCubes[1];

			if (AnyCubeCountOvergoesMaxCount(revealedCubes))
			{
				continue;
			}

			var gameId = gameIdAndRevealedCubes[0];

			var id = int.Parse(NumberRegex.Match(gameId).Value);

			idSum += id;
		}

		return idSum;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}

	private static bool AnyCubeCountOvergoesMaxCount(string revealedCubes)
	{
		foreach (var revealedCubeMatch in RevealedCubeRegex.Matches(revealedCubes))
		{
			var revealedCube = revealedCubeMatch.ToString() ?? string.Empty;

			var cubeCount = int.Parse(NumberRegex.Match(revealedCube).Value);

			var maxCubeCount = MaxCubeCountByColor[WordRegex.Match(revealedCube).Value];

			if (cubeCount > maxCubeCount)
			{
				return true;
			}
		}

		return false;
	}
}