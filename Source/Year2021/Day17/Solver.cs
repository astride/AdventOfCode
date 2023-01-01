using Common.Interfaces;
using Common.Models;

namespace Year2021;

public class Day17Solver : IPuzzleSolver
{
	public string Title => "Trick Shot";
	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var target = GetTargetArea(input);
		
		var yMax = 0;

		var positionsOnPath = new List<Coordinate>();

		foreach (var velocityX in Enumerable.Range(1, 108)) // range length found experimentally
		{
			foreach (var velocityY in Enumerable.Range(1, 108)) // range length found experimentally
			{
				var initialVelocity = new Velocity(velocityX, velocityY);
				var position = Coordinate.Origin;
				var stepCount = 1;

				positionsOnPath.Clear();

				while (!position.IsInside(target) && !position.HasPassed(target))
				{
					positionsOnPath.Add(position);

					position = initialVelocity.GetPositionAfterSteps(stepCount);

					if (position.IsInside(target))
					{
						var yMaxOnPath = positionsOnPath.Max(pos => pos.Y);

						if (yMaxOnPath > yMax)
						{
							yMax = yMaxOnPath;
						}
					}

					stepCount++;
				}
			}
		}

		return yMax;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var target = GetTargetArea(input);
		
		var possibleInitialVelocities = new List<Velocity>();

		foreach (var velocityX in Enumerable.Range(1, target.Max.X))
		{
			foreach (var velocityY in Enumerable.Range(target.Min.Y, Math.Abs(2 * target.Min.Y))) // range length found experimentally
			{
				var initialVelocity = new Velocity(velocityX, velocityY);
				var position = Coordinate.Origin;
				var stepCount = 1;

				while (!position.IsInside(target) && !position.HasPassed(target))
				{
					position = initialVelocity.GetPositionAfterSteps(stepCount);

					if (position.IsInside(target))
					{
						possibleInitialVelocities.Add(initialVelocity);
					}

					stepCount++;
				}
			}
		}

		return possibleInitialVelocities.Count;
	}

	private static Area GetTargetArea(string[] input)
	{
		var targetRanges = input
			.Single(entry => !string.IsNullOrWhiteSpace(entry))
			.Substring("target area: ".Length)
			.Split(',')
			.Select(entry => entry.Trim())
			.ToList();

		return targetRanges.GetTargetArea();
	}
}

internal static class Day17Helpers
{
	public static Area GetTargetArea(this IReadOnlyCollection<string> targetRanges)
    {
		var xTargetRange = targetRanges
			.Single(entry => entry.Contains('x'));

		var yTargetRange = targetRanges
			.Single(entry => entry.Contains('y'));

		return new Area(
			new Coordinate(xTargetRange.GetMinValue(), yTargetRange.GetMinValue()),
			new Coordinate(xTargetRange.GetMaxValue(), yTargetRange.GetMaxValue()));
	}

	private static int GetMinValue(this string range)
	{
		return int.Parse(string.Concat(range.Skip(2).TakeWhile(ch => ch != '.')));
	}
		
	private static int GetMaxValue(this string range)
	{
		return int.Parse(string.Concat(range.SkipWhile(ch => ch != '.').SkipWhile(ch => ch == '.')));
	}

	public static bool IsInside(this Coordinate position, Area area)
    {
		return
			position.X >= area.Min.X &&
			position.X <= area.Max.X &&
			position.Y >= area.Min.Y &&
			position.Y <= area.Max.Y;
    }

	public static bool HasPassed(this Coordinate position, Area area)
    {
		return
			position.X > area.Max.X ||
			position.Y < area.Min.Y;
	}
	
	public static Coordinate GetPositionAfterSteps(this Velocity initialVelocity, int stepCount)
    {
		return new Coordinate
			(initialVelocity.GetXAfterSteps(stepCount),
			initialVelocity.GetYAfterSteps(stepCount));
    }

	private static int GetXAfterSteps(this Velocity initialVelocity, int stepCount)
    {
		//we know that initial position is (0, 0) <-- no need to include

		return Enumerable.Range(1, stepCount)
			.Select(step =>
				Math.Max(0, initialVelocity.X - (step - 1)))
			.Sum();
	}

	private static int GetYAfterSteps(this Velocity initialVelocity, int stepCount)
	{
		//we know that initial position is (0, 0) <-- no need to include

		return Enumerable.Range(1, stepCount)
			.Select(step =>
				initialVelocity.Y - (step - 1))
			.Sum();
	}
}
