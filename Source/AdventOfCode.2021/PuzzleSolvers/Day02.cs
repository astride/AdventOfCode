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
				.Select(entry => (entry[0], int.Parse(entry[1])))
				.ToList();

			Part1Solution = SolvePart1(input);
			Part2Solution = SolvePart2(input);
		}

		private const string Forward = "forward";
		private const string Down = "down";
		private const string Up = "up";

		private int SolvePart1(List<(string Action, int Units)> commands)
		{
			int pos = 0;
			int depth = 0;

			foreach (var command in commands)
			{
				pos += GetHorizontalChange(command.Action, command.Units);
				depth += GetVerticalChange(command.Action, command.Units);
			}

			return pos * depth;
		}

		private int SolvePart2(List<(string Action, int Units)> commands)
		{
			int pos = 0;
			int aim = 0;
			int depth = 0;

			foreach (var command in commands)
			{
				pos += GetHorizontalChange(command.Action, command.Units);
				aim += GetVerticalChange(command.Action, command.Units);

				if (command.Action == Forward)
				{
					depth += aim * command.Units;
				}
			}

			return pos * depth;
		}

		private static int GetHorizontalChange(string action, int value) => action == Forward ? value : 0;

		private static int GetVerticalChange(string action, int value)
		{
			//TODO C# 8 --> switch expression
			return action == Down
				? value
				: action == Up
					? (-1) * value
					: 0;
		}
	}
}
