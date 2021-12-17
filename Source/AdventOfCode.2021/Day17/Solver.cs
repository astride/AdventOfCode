using AdventOfCode.Common;
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

		private static int SolvePart1(Area target)
		{
			var yMax = 0;

			Velocity initialVelocity;
			Coordinate position;

			var positionsOnPath = new List<Coordinate>();
			int stepCount;

			foreach (var velocityX in Enumerable.Range(1, 108)) // range length found experimentally
			{
				foreach (var velocityY in Enumerable.Range(1, 108)) // range length found experimentally
				{
					initialVelocity = new Velocity(velocityX, velocityY);
					position = Coordinate.Origo;
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

		private static int SolvePart2(Area target)
        {
			var possibleInitialVelocities = new List<Velocity>();

			Velocity initialVelocity;
			Coordinate position;

			int stepCount;

			foreach (var velocityX in Enumerable.Range(1, target.Max.X))
			{
				foreach (var velocityY in Enumerable.Range(target.Min.Y, Math.Abs(2 * target.Min.Y))) // range length found experimentally
				{
					initialVelocity = new Velocity(velocityX, velocityY);
					position = Coordinate.Origo;
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
		public static Area GetTargetArea(this IEnumerable<string> targetRanges)
        {
			var xTargetRange = targetRanges
				.Single(entry => entry.Contains('x'));

			var yTargetRange = targetRanges
				.Single(entry => entry.Contains('y'));

			return new Area(
				new Coordinate(xTargetRange.GetMinValue(), yTargetRange.GetMinValue()),
				new Coordinate(xTargetRange.GetMaxValue(), yTargetRange.GetMaxValue()));
		}

		public static int GetMinValue(this string range)
		{
			return int.Parse(string.Concat(range.Skip(2).TakeWhile(ch => ch != '.')));
		}
			
		public static int GetMaxValue(this string range)
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

	public class XY
    {
		public XY(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int X { get; set; }

		public int Y { get; set; }
	}

	public class Coordinate : XY
	{
		public Coordinate(int x, int y) : base(x, y) { }

		public static Coordinate Origo => new Coordinate(0, 0);
	}

    public class Velocity : XY
    {
        public Velocity(int x, int y) : base(x, y) { }
    }

    public class Area
	{
        public Area(Coordinate min, Coordinate max)
        {
			Min = min;
			Max = max;
        }

		public Coordinate Min { get; set; }

		public Coordinate Max { get; set; }
	}
}
