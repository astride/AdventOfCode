using AdventOfCode.Common;
using System;
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
				.Select(entry => new Tuple<string, int> ( entry[0], int.Parse(entry[1]) ))
				.ToList();

			Part1Solution = SolvePart1(input);
			Part2Solution = SolvePart2(input);
		}

		private const string Forward = "forward";
		private const string Down = "down";
		private const string Up = "up";

		private int SolvePart1(List<Tuple<string, int>> commands)
		{
			int x = 0;
			int depth = 0;

			foreach (var command in commands)
			{
				x += GetHorizontalPositionChangeFrom(command.Item1, command.Item2);
				depth += GetDepthChangeFrom(command.Item1, command.Item2);
			}

			return x * depth;
		}

		private static string[] commandParts = new string[2];

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

		private int SolvePart2(List<Tuple<string, int>> commands)
		{
			int aim = 0;
			int x = 0;
			int depth = 0;

			foreach (var command in commands)
			{
				aim += GetAimChangeFrom(command.Item1, command.Item2);
				x += GetHorizontalPositionChangeFrom(command.Item1, command.Item2);
				depth += GetDepthChangeFrom(command.Item1, command.Item2, aim);
			}

			return x * depth;
		}

		private static int GetAimChangeFrom(string action, int value)
		{
			if (action == Forward) return 0;

			return action == Down ? value : (-1) * value;
		}

		private static int GetDepthChangeFrom(string action, int value, int aimValue)
		{
			return action == Forward ? aimValue * value : 0;
		}
	}
}
