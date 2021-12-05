using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day05 : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var input = rawInput
				.Where(line => !string.IsNullOrEmpty(line))
				.Select(line => line.Split(' '))
				.Select(coor => new Segment(coor[0], coor[2]));

			var xMax = input
				.Select(segment => Math.Max(segment.X1, segment.X2))
				.Distinct()
				.OrderByDescending(x => x)
				.First();

			var yMax = input
				.Select(segment => Math.Max(segment.Y1, segment.Y2))
				.Distinct()
				.OrderByDescending(y => y)
				.First();

			Part1Solution = SolvePart1(input, xMax, yMax).ToString();
		}

		private static int SolvePart1(IEnumerable<Segment> linesOfVents, int maxRowIndex, int maxColIndex)
		{
			var horizontalLines = linesOfVents
				.Where(line => line.Y1 == line.Y2)
				.Select(line => new HorizontalSegment(line.X1, line.X2, line.Y1));

			var verticalLines = linesOfVents
				.Where(line => line.X1 == line.X2)
				.Select(line => new VerticalSegment(line.X1, line.Y1, line.Y2));

			var overlapMap = new int[maxRowIndex + 1, maxColIndex + 1];

			foreach (var line in horizontalLines)
			{
				overlapMap.UpdateWith(line);
			}

			foreach (var line in verticalLines)
			{
				overlapMap.UpdateWith(line);
			}

			var dangerousOverlapCount = overlapMap.Cast<int>()
				.Count(overlapCount => overlapCount >= 2);

			return dangerousOverlapCount;
		}
	}

	static class Day05Helpers
	{
		public static void UpdateWith(this int[,] map, HorizontalSegment segment)
		{
			var xStart = Math.Min(segment.X1, segment.X2);
			var xEnd = Math.Max(segment.X1, segment.X2);

			for (var x = xStart; x <= xEnd; x++)
			{
				map[x, segment.Y]++;
			}
		}

		public static void UpdateWith(this int[,] map, VerticalSegment segment)
		{
			var yStart = Math.Min(segment.Y1, segment.Y2);
			var yEnd = Math.Max(segment.Y1, segment.Y2);

			for (var y = yStart; y <= yEnd; y++)
			{
				map[segment.X, y]++;
			}
		}
	}

	public class HorizontalSegment
	{
		public HorizontalSegment(int x1, int x2, int y) 
		{
			X1 = x1;
			X2 = x2;
			Y = y;
		}

		public int X1 { get; set; }
		public int X2 { get; set; }
		public int Y { get; set; }
	}

	public class VerticalSegment
	{
		public VerticalSegment(int x, int y1, int y2)
		{
			X = x;
			Y1 = y1;
			Y2 = y2;
		}

		public int X { get; set; }
		public int Y1 { get; set; }
		public int Y2 { get; set; }
	}

	public class Segment
	{
		public Segment(string startPoint, string endPoint)
		{
			var start = startPoint
				.Split(',')
				.Select(coor => int.Parse(coor))
				.ToArray();
			var end = endPoint
				.Split(',')
				.Select(coor => int.Parse(coor))
				.ToArray();

			X1 = start[0];
			Y1 = start[1];
			X2 = end[0];
			Y2 = end[1];
		}

		public int X1 { get; set; }
		public int Y1 { get; set; }
		public int X2 { get; set; }
		public int Y2 { get; set; }
	}
}
