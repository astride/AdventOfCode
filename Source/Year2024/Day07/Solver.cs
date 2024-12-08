using Common.Interfaces;

namespace Year2024;

public class Day07Solver : IPuzzleSolver
{
	public string Title => "Bridge Repair";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private const bool ShouldLog = false;

	private static readonly Func<long, long, long>[] Calculations =
	{
		(long first, long second) => first + second,
		(long first, long second) => first * second,
		// (long first, long second) => first - second,
		// (long first, long second) => first / second,
	};

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var totalCalibrationResult = input.Select(GetCalibrationResult).Sum();

		return totalCalibrationResult;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}

	private static long GetCalibrationResult(string operatorlessEquation)
	{
		var result = long.Parse(operatorlessEquation.Split(':')[0]);
		
		MaybeLog(string.Empty, operatorlessEquation, "------------");

		var values = operatorlessEquation.Split(' ').Skip(1).Select(long.Parse).ToArray();

		var minResult = values.Sum();

		if (minResult > result)
		{
			MaybeLog("Values are too large");
			return 0;
		}

		var maxResult = values.Aggregate(1L, (previousProduct, next) => previousProduct * next);

		if (maxResult < result)
		{
			MaybeLog("Values are too small");

			return 0;
		}

		var calculatedResults = CalculateResults(values[0], values[1..]);

		var foundMatch = calculatedResults.Contains(result);
		
		if (!foundMatch) MaybeLog("(no match found)");
		
		return foundMatch ? result : 0;
	}

	private static IEnumerable<long> CalculateResults(long number, long[] remainingNumbers)
	{
		foreach (var calculation in Calculations)
		{
			var result = calculation(number, remainingNumbers[0]);

			if (remainingNumbers.Length is 1)
			{
				MaybeLog(result);
				yield return result;
			}
			else
			{
				foreach (var nextResult in CalculateResults(result, remainingNumbers[1..]))
				{
					yield return nextResult;
				}
			}
		}
	}

	private static void MaybeLog(params object[] text)
	{
		if (ShouldLog) foreach (var line in text) Console.WriteLine(line);
	}

	private static void MaybeLog(params string[] text)
	{
		if (ShouldLog) foreach (var line in text) Console.WriteLine(line);
	}
}
