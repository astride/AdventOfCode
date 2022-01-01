using AdventOfCode.Common;
using AdventOfCode.Common.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day17Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var input = rawInput
				.Single(entry => !string.IsNullOrWhiteSpace(entry))
				.Substring("target area: ".Length)
				.Split(',')
				.Select(entry => entry.Trim());

			var targetArea = input.GetTargetArea();

			Part1Solution = SolvePart1(targetArea).ToString();
			Part2Solution = SolvePart2(targetArea).ToString();
		}

		private static int SolvePart1(AreaXY target)
		{
			var yMax = 0;

			VelocityXY initialVelocity;
			CoordinateXY position;

			var positionsOnPath = new List<CoordinateXY>();
			int stepCount;

			foreach (var velocityX in Enumerable.Range(1, 108)) // range length found experimentally
			{
				foreach (var velocityY in Enumerable.Range(1, 108)) // range length found experimentally
				{
					initialVelocity = new VelocityXY(velocityX, velocityY);
					position = CoordinateXY.Origo;
					positionsOnPath.Clear();
					stepCount = 1;

					while (!position.IsInside(target) && !position.HasPassed(target))
					{
						positionsOnPath.Add(position);

						position = initialVelocity.GetPositionAfterSteps(stepCount);

						if (position.IsInside(target))
						{
							if (positionsOnPath.Any(pos => pos.Y > yMax))
							{
								yMax = positionsOnPath.Select(pos => pos.Y).Max();
							}
						}

						stepCount++;
					}
				}
			}

			return yMax;
		}

		private static int SolvePart2(AreaXY target)
        {
			var possibleInitialVelocities = new List<VelocityXY>();

			VelocityXY initialVelocity;
			CoordinateXY position;

			int stepCount;

			foreach (var velocityX in Enumerable.Range(1, target.Max.X))
			{
				foreach (var velocityY in Enumerable.Range(target.Min.Y, Math.Abs(2 * target.Min.Y))) // range length found experimentally
				{
					initialVelocity = new VelocityXY(velocityX, velocityY);
					position = CoordinateXY.Origo;
					stepCount = 1;

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

			return possibleInitialVelocities.Count();
		}
	}

	public static class Day17Helpers
	{
		public static AreaXY GetTargetArea(this IEnumerable<string> targetRanges)
        {
			var xTargetRange = targetRanges
				.Single(entry => entry.Contains('x'));

			var yTargetRange = targetRanges
				.Single(entry => entry.Contains('y'));

			return new AreaXY(
				new CoordinateXY(xTargetRange.GetMinValue(), yTargetRange.GetMinValue()),
				new CoordinateXY(xTargetRange.GetMaxValue(), yTargetRange.GetMaxValue()));
		}

		public static int GetMinValue(this string range)
		{
			return int.Parse(string.Concat(range.Skip(2).TakeWhile(ch => ch != '.')));
		}
			
		public static int GetMaxValue(this string range)
		{
			return int.Parse(string.Concat(range.SkipWhile(ch => ch != '.').SkipWhile(ch => ch == '.')));
		}

		public static bool IsInside(this CoordinateXY position, AreaXY area)
        {
			return
				position.X >= area.Min.X &&
				position.X <= area.Max.X &&
				position.Y >= area.Min.Y &&
				position.Y <= area.Max.Y;
        }

		public static bool HasPassed(this CoordinateXY position, AreaXY area)
        {
			return
				position.X > area.Max.X ||
				position.Y < area.Min.Y;
		}
		
		public static CoordinateXY GetPositionAfterSteps(this VelocityXY initialVelocity, int stepCount)
        {
			return new CoordinateXY
				(initialVelocity.GetXAfterSteps(stepCount),
				initialVelocity.GetYAfterSteps(stepCount));
        }

		private static int GetXAfterSteps(this VelocityXY initialVelocity, int stepCount)
        {
			//we know that initial position is (0, 0) <-- no need to include

			return Enumerable.Range(1, stepCount)
				.Select(step =>
					Math.Max(0, initialVelocity.X - (step - 1)))
				.Sum();
		}

		private static int GetYAfterSteps(this VelocityXY initialVelocity, int stepCount)
		{
			//we know that initial position is (0, 0) <-- no need to include

			return Enumerable.Range(1, stepCount)
				.Select(step =>
					initialVelocity.Y - (step - 1))
				.Sum();
		}
	}
}
