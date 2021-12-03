using AdventOfCode.Common;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.PuzzleSolvers
{
	class Day02 : IPuzzleSolver<int>
	{
		public int Part1Solution { get; set; }
		public int Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var input = rawInput
				.Where(entry => !string.IsNullOrWhiteSpace(entry))
				.Select(entry => entry.Split(' '))
				.Select(entry => (Action: entry[0], Value: int.Parse(entry[1])))
				.ToList();

			Part1Solution = SolvePart1(input);
			Part2Solution = SolvePart2(input);
		}

		private const string Forward = "forward";
		private const string Down = "down";
		private const string Up = "up";

		private int SolvePart1(List<(string Action, int Value)> commands)
		{
			int x = 0;
			int depth = 0;

			foreach (var command in commands)
			{
				x += GetHorizontalPositionChangeFrom(command.Action, command.Value);
				depth += GetDepthChangeFrom(command.Action, command.Value);
			}

			return x * depth;
		}

		private int SolvePart2(List<(string Action, int Value)> commands)
		{
			int aim = 0;
			int x = 0;
			int depth = 0;

			foreach (var command in commands)
			{
				aim += GetAimChangeFrom(command.Action, command.Value);
				x += GetHorizontalPositionChangeFrom(command.Action, command.Value);
				depth += GetDepthChangeFrom(command.Action, command.Value, aim);
			}

			return x * depth;
		}

		int GetHorizontalPositionChangeFrom(string action, int value)
		{
			return action == Forward ? value : 0;
		}

		int GetDepthChangeFrom(string action, int value)
		{
			//TODO C# 8 --> switch expression
			return action == Down
				? value
				: action == Up
					? (-1) * value
					: 0;
		}

		private static int GetDepthChangeFrom(string action, int value, int aimValue)
		{
			return action == Forward ? aimValue * value : 0;
		}

		private static int GetAimChangeFrom(string action, int value)
		{
			if (action == Forward) return 0;

			return action == Down ? value : (-1) * value;
		}
	}
}
