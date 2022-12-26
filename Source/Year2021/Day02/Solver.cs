using Common.Interfaces;

namespace Year2021;

public class Day02Solver : IPuzzleSolver
{
	public string Title => "Dive!";
	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private const string Forward = "forward";
	private const string Down = "down";
	private const string Up = "up";

	public object GetPart1Solution(string[] input)
	{
		var commands = GetCommands(input);
		
		var pos = 0;
		var depth = 0;

		foreach (var command in commands)
		{
			pos += GetHorizontalChange(command.Action, command.Units);
			depth += GetVerticalChange(command.Action, command.Units);
		}

		return pos * depth;
	}

	public object GetPart2Solution(string[] input)
	{
		var commands = GetCommands(input);
		
		var pos = 0;
		var aim = 0;
		var depth = 0;

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

	private static List<(string Action, int Units)> GetCommands(string[] input)
	{
		return input
			.Where(entry => !string.IsNullOrWhiteSpace(entry))
			.Select(entry => entry.Split(' '))
			.Select(entry => (entry[0], int.Parse(entry[1])))
			.ToList();
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
