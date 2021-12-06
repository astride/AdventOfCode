using AdventOfCode.Common;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day02Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var input = rawInput
				.Where(entry => !string.IsNullOrWhiteSpace(entry))
				.Select(entry => entry.Split(' '))
				.Select(entry => (entry[0], int.Parse(entry[1])))
				.ToList();

			Part1Solution = SolvePart1(input).ToString();
			Part2Solution = SolvePart2(input).ToString();
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

		private static int GetHorizontalChange(string action, int value) => action switch
		{
			Forward => value,
			_ => 0
		};

		private static int GetVerticalChange(string action, int value) => action switch
		{
			Down	=> value,
			Up		=> value * (-1),
			_		=> 0
		};
	}
}
