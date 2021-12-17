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

			var xTargetRange = input
				.Single(entry => entry.Contains('x'));

			var yTargetRange = input
				.Single(entry => entry.Contains('y'));

			(int Min, int Max) xTarget = (xTargetRange.GetMinTargetValue(), xTargetRange.GetMaxTargetValue());
			(int Min, int Max) yTarget = (yTargetRange.GetMinTargetValue(), yTargetRange.GetMaxTargetValue());

			Part1Solution = SolvePart1(xTarget, yTarget).ToString();
		}

		private static int SolvePart1((int Min, int Max) xTarget, (int Min, int Max) yTarget)
		{
			var yMax = 0;

			Coordinate currentPosition;
			var positionsOnPath = new List<Coordinate>();
			int stepCount;

            foreach (var velocityX in Enumerable.Range(1, 108)) // range length found experimentally
            {
				foreach (var velocityY in Enumerable.Range(1, 108)) // range length found experimentally
                {
					currentPosition = Coordinate.Origo;
					positionsOnPath.Clear();
					stepCount = 1;

					while (!currentPosition.IsInside(xTarget, yTarget) &&
						!currentPosition.HasPassed(xTarget, yTarget))
                    {
						positionsOnPath.Add(currentPosition);
						
						currentPosition = (velocityX, velocityY).GetPositionAfterSteps(stepCount);

						if (currentPosition.IsInside(xTarget, yTarget))
                        {
							if (positionsOnPath.Any(pos => pos.Y > yMax))
                            {
								yMax = positionsOnPath
									.Select(pos => pos.Y)
									.Max();
                            }
                        }

						stepCount++;
					}
				}
            }

            return yMax;
		}
	}

	public static class Day17Helpers
	{
		public static int GetMinTargetValue(this string targetRange)
		{
			return int.Parse(string.Concat(targetRange.Skip(2).TakeWhile(ch => ch != '.')));
		}
			
		public static int GetMaxTargetValue(this string targetRange)
		{
			return int.Parse(string.Concat(targetRange.SkipWhile(ch => ch != '.').SkipWhile(ch => ch == '.')));
		}

		public static bool IsInside(this Coordinate position, (int Min, int Max) xTarget, (int Min, int Max) yTarget)
        {
			return position.X >= xTarget.Min && position.X <= xTarget.Max &&
				position.Y >= yTarget.Min && position.Y <= yTarget.Max;

        }

		public static bool HasPassed(this Coordinate position, (int Min, int Max) xTarget, (int Min, int Max) yTarget)
        {
			return
				position.X > xTarget.Max ||
				position.Y < yTarget.Min;
		}
		
		public static Coordinate GetPositionAfterSteps(this (int X, int Y) initialVelocity, int stepCount)
        {
			return new Coordinate
				(initialVelocity.GetXAfterSteps(stepCount),
				initialVelocity.GetYAfterSteps(stepCount));
        }

		private static int GetXAfterSteps(this (int X, int Y) initialVelocity, int stepCount)
        {
			//we know that initial position is (0, 0) <-- no need to include

			return Enumerable.Range(1, stepCount)
				.Select(step =>
					Math.Max(0, initialVelocity.X - (step - 1)))
				.Sum();
		}

		private static int GetYAfterSteps(this (int X, int Y) initialVelocity, int stepCount)
		{
			//we know that initial position is (0, 0) <-- no need to include

			return Enumerable.Range(1, stepCount)
				.Select(step =>
					initialVelocity.Y - (step - 1))
				.Sum();
		}
	}

	public class Coordinate
	{
        public Coordinate(int x, int y)
        {
			X = x;
			Y = y;
        }

		public int X { get; set; }

		public int Y { get; set; }

		public static Coordinate Origo => new Coordinate(0, 0);
	}
}
