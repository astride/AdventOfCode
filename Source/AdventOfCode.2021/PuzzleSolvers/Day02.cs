using AdventOfCode.Common;
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
				.ToArray();

			Part1Solution = SolvePart1(input);
			Part2Solution = SolvePart2(input);
		}

		private const string Forward = "forward";
		private const string Down = "down";
		private const string Up = "up";

		private int SolvePart1(string[] commands)
		{
			int x = 0;
			int depth = 0;

			foreach (var command in commands)
			{
				x += GetHorizontalPositionChangeFrom(command);
				depth += GetDepthChangeFrom(command);
			}

			return x * depth;
		}

		private static string[] commandParts = new string[2];

		int GetHorizontalPositionChangeFrom(string command)
		{
			commandParts = command.Split(' ');

			return commandParts[0] == Forward
				? int.Parse(commandParts[1])
				: 0;
		}

		int GetDepthChangeFrom(string command)
		{
			commandParts = command.Split(' ');

			return commandParts[0] == Down
				? int.Parse(commandParts[1])
				: commandParts[0] == Up
					? (-1) * int.Parse(commandParts[1])
					: 0;
		}

		private int SolvePart2(string[] commands)
		{
			int aim = 0;
			int x = 0;
			int depth = 0;

			foreach (var command in commands)
			{
				aim += GetAimChangeFrom(command);
				x += GetHorizontalPositionChangeFrom(command);
				depth += GetDepthChangeFrom(command, aim);
			}

			return x * depth;
		}

		private static int GetAimChangeFrom(string command)
		{
			commandParts = command.Split(' ');

			if (commandParts[0] == Forward) return 0;

			return commandParts[0] == Down
				? int.Parse(commandParts[1])
				: (-1) * int.Parse(commandParts[1]);
		}

		private static int GetDepthChangeFrom(string command, int aim)
		{
			commandParts = command.Split(' ');

			return commandParts[0] == Forward
				? aim * int.Parse(commandParts[1])
				: 0;
		}
	}
}
