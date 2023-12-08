using Common.Interfaces;

namespace Year2023;

public class Day08Solver : IPuzzleSolver
{
	public string Title => "Haunted Wasteland";
	public bool UsePartSpecificExampleInputFiles => true;

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private const string StartNode = "AAA";
	private const string TargetNode = "ZZZ";

	private static readonly Dictionary<char, int> DestinationIndexByLeftRightInstruction = new()
	{
		['L'] = 0,
		['R'] = 1,
	};

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var leftRightInstructions = input[0]
			.Select(instruction => DestinationIndexByLeftRightInstruction[instruction])
			.ToArray();

		var destinationsBySourceNode = new Dictionary<string, string[]>();

		var nodeDescriptions = input[2..];

		foreach (var line in nodeDescriptions)
		{
			var sourceNodeAndDestinationNodes = line.Split('=', StringSplitOptions.TrimEntries);

			var destinationNodes = sourceNodeAndDestinationNodes[1]
				.Substring(1, 8)
				.Split(',', StringSplitOptions.TrimEntries);

			destinationsBySourceNode[sourceNodeAndDestinationNodes[0]] = destinationNodes;
		}

		var node = StartNode;

		var stepCount = 0;
		var instructionIndex = 0;

		while (node != TargetNode)
		{
			var destinationIndex = leftRightInstructions[instructionIndex];

			node = destinationsBySourceNode[node][destinationIndex];

			instructionIndex = instructionIndex == leftRightInstructions.Length - 1
				? 0
				: instructionIndex + 1;

			stepCount++;
		}

		return stepCount;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}
}