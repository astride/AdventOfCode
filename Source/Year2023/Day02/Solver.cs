using System.Text.RegularExpressions;
using Common.Interfaces;

namespace Year2023;

public class Day02Solver : IPuzzleSolver
{
	public string Title => "Cube Conundrum";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private static readonly Dictionary<string, int> MaxCubeCountByColor = new()
	{
		["blue"] = 14,
		["green"] = 13,
		["red"] = 12,
	};

	private static readonly Regex[] RevealedCubeByColorRegexes = MaxCubeCountByColor.Keys
		.Select(key => new Regex($@"(\d+ {key})"))
		.ToArray();

	private static readonly Regex NumberRegex = new(@"\d+");
	private static readonly Regex WordRegex = new(@"[a-z]+");
	private static readonly Regex RevealedCubeRegex = new(@"(\d+ [a-z]+)");

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
		var powerSum = 0;

		foreach (var gameDescription in input)
		{
			var gameIdAndRevealedCubes = gameDescription.Split(':', StringSplitOptions.TrimEntries);

			var revealedCubes = gameIdAndRevealedCubes[1];

			powerSum += GetPower(revealedCubes);
		}

		return powerSum;
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

	private static int GetPower(string revealedCubes)
	{
		var power = 1;

		foreach (var revealedCubeByColorRegex in RevealedCubeByColorRegexes)
		{
			var revealedCubeMatches = revealedCubeByColorRegex.Matches(revealedCubes);

			var maxRevealedCubeCount = revealedCubeMatches
				.Max(revealedCubeMatch => int.Parse(NumberRegex.Match(revealedCubeMatch.ToString()).Value));

			power *= maxRevealedCubeCount;
		}

		return power;
	}
}