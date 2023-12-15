using Common.Interfaces;

namespace Year2023;

public class Day15Solver : IPuzzleSolver
{
	public string Title => "Lens Library";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var resultByStepType = new Dictionary<string, int>();

		var resultAfterMultiplicationAndDivisionByValue = new Dictionary<int, int>();

		var resultSum = 0L;

		foreach (var step in input.Single().Split(','))
		{
			if (resultByStepType.TryGetValue(step, out var stepTypeResult))
			{
				resultSum += stepTypeResult;
				continue;
			}

			var currentValue = 0;

			foreach (var instruction in step)
			{
				currentValue += instruction;

				if (resultAfterMultiplicationAndDivisionByValue.TryGetValue(currentValue, out var result))
				{
					currentValue = result;
				}
				else
				{
					var resultAfterMultiplication = currentValue * 17;
					var resultAfterDivision = resultAfterMultiplication % 256;

					resultAfterMultiplicationAndDivisionByValue[currentValue] = resultAfterDivision;

					currentValue = resultAfterDivision;
				}
			}

			resultByStepType[step] = currentValue;

			resultSum += currentValue;
		}

		return resultSum;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}
}