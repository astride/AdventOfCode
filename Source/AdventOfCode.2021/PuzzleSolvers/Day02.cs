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

			Part1Solution = Solve(input);
			Part2Solution = Solve(input, true);
		}

		private const string Forward = "forward";
		private const string Down = "down";
		private const string Up = "up";

		private int Solve(
			List<(string Action, int Value)> commands, 
			bool includeAim = false)
		{
			int aim = 0;
			int x = 0;
			int depth = 0;

			foreach (var command in commands)
			{
				if (includeAim)
				{
					aim += GetAimChangeFrom(command.Action, command.Value);
				}

				x += GetHorizontalPositionChangeFrom(command.Action, command.Value);

				depth += includeAim
					? GetDepthChangeFrom(command.Action, command.Value, aim)
					: GetDepthChangeFrom(command.Action, command.Value);
			}

			return includeAim
				? x * depth * aim
				: x * depth;
		}

		int GetHorizontalPositionChangeFrom(string action, int value)
		{
			return action == Forward ? value : 0;
		}

		int GetDepthChangeFrom(string action, int value, int? aim = null)
		{
			if (aim.HasValue)
			{
				return action == Forward ? value * aim.Value : 0;
			}

			//TODO C# 8 --> switch expression
			return action == Down
				? value
				: action == Up
					? (-1) * value
					: 0;
		}

		private static int GetAimChangeFrom(string action, int value)
		{
			if (action == Forward) return 0;

			return action == Down ? value : (-1) * value;
		}
	}
}
