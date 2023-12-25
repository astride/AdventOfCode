using Common.Interfaces;

namespace Year2023;

public class Day24Solver : IPuzzleSolver
{
	public string Title => "Never Tell Me The Odds";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var testAreaMin = isExampleInput ? 7L : 200000000000000;
		var testAreaMax = isExampleInput ? 27L : 400000000000000;

		var hailstoneCrossingCalculator = new HailstoneCrossingCalculator(testAreaMin, testAreaMax);

		var hailstones = input.Select(Hailstone.DescribedBy).ToList();

		var intersectionsWithinTestAreaCount = 0;

		for (var iFirst = 0; iFirst < hailstones.Count - 1; iFirst++)
		{
			for (var iSecond = iFirst + 1; iSecond < hailstones.Count; iSecond++)
			{
				if (hailstoneCrossingCalculator.HailstonesIntersectWithinTestArea(hailstones[iFirst], hailstones[iSecond]))
				{
					intersectionsWithinTestAreaCount++;
				}
			}
		}

		return intersectionsWithinTestAreaCount;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}

	private class HailstoneCrossingCalculator
	{
		public HailstoneCrossingCalculator(long testAreaMinValue, long testAreaMaxValue)
		{
			_testAreaMinValue = testAreaMinValue;
			_testAreaMaxValue = testAreaMaxValue;
		}

		private readonly long _testAreaMinValue;
		private readonly long _testAreaMaxValue;

		public bool HailstonesIntersectWithinTestArea(Hailstone a, Hailstone b)
		{
			var nanosecondCrossingDivisorA = a.Vx/b.Vx - a.Vy/b.Vy;

			if (nanosecondCrossingDivisorA == 0)
			{
				return false;
			}

			var nanosecondCrossingDividendA = (a.Py - b.Py) / b.Vy - (a.Px - b.Px) / b.Vx;

			var nanosecondCrossingA = nanosecondCrossingDividendA/nanosecondCrossingDivisorA;

			if (nanosecondCrossingA <= 0)
			{
				return false;
			}
			
			var nanosecondCrossingB = (nanosecondCrossingA * a.Vx + a.Px - b.Px ) / b.Vx;

			if (nanosecondCrossingB <= 0)
			{
				return false;
			}

			var crossingX = nanosecondCrossingA * a.Vx + a.Px;

			if (crossingX < _testAreaMinValue || crossingX > _testAreaMaxValue)
			{
				return false;
			}

			var crossingY = nanosecondCrossingA * a.Vy + a.Py;

			return crossingY >= _testAreaMinValue && crossingY <= _testAreaMaxValue;
		}
	}

	private class Hailstone
	{
		private Hailstone(long px, long py, decimal vx, decimal vy)
		{
			Px = px;
			Py = py;
			Vx = vx;
			Vy = vy;
		}
		
		public long Px { get; }
		public long Py { get; }
		public decimal Vx { get; }
		public decimal Vy { get; }

		public static Hailstone DescribedBy(string description)
		{
			var positionAndVelocity = description
				.Split('@', StringSplitOptions.TrimEntries)
				.SelectMany(positionAndVelocity => positionAndVelocity.Split(',', StringSplitOptions.TrimEntries))
				.ToList();

			var px = long.Parse(positionAndVelocity[0]);
			var py = long.Parse(positionAndVelocity[1]);
			var vx = decimal.Parse(positionAndVelocity[3]);
			var vy = decimal.Parse(positionAndVelocity[4]);
			
			return new(px, py, vx, vy);
		}
	}
}