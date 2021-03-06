using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day05Solver : IPuzzleSolver
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
			Part2Solution = SolvePart2(input, xMax, yMax).ToString();
		}

		private static int SolvePart1(IEnumerable<Segment> linesOfVents, int maxRowIndex, int maxColIndex)
		{
			return Solve(linesOfVents, maxRowIndex, maxColIndex);
		}

		private static int SolvePart2(IEnumerable<Segment> linesOfVents, int maxRowIndex, int maxColIndex)
		{
			return Solve(linesOfVents, maxRowIndex, maxColIndex, true);
		}

		private static int Solve(IEnumerable<Segment> linesOfVents, int maxRowIndex, int maxColIndex, bool includeDiagonalLines = false)
		{
			var overlapMap = new int[maxRowIndex + 1, maxColIndex + 1];

			foreach (var line in linesOfVents.HorizontalSegments())
			{
				overlapMap.UpdateWith(line);
			}

			foreach (var line in linesOfVents.VerticalSegments())
			{
				overlapMap.UpdateWith(line);
			}

			if (includeDiagonalLines)
			{
				var diagonalSegments = linesOfVents.UpwardsDiagonalSegments().Concat(linesOfVents.DownWardsDiagonalSegments());

				foreach (var line in diagonalSegments)
				{
					overlapMap.UpdateWith(line);
				}
			}

			var dangerousOverlapCount = overlapMap.Cast<int>()
				.Count(overlapCount => overlapCount >= 2);

			return dangerousOverlapCount;
		}
	}

	static class Day05Helpers
	{
		public static IEnumerable<HorizontalSegment> HorizontalSegments(this IEnumerable<Segment> segments)
		{
			return segments
				.Where(s => s.Y1 == s.Y2)
				.Select(s => new HorizontalSegment(s.X1, s.X2, s.Y1));
		}

		public static IEnumerable<VerticalSegment> VerticalSegments(this IEnumerable<Segment> segments)
		{
			return segments
				.Where(s => s.X1 == s.X2)
				.Select(s => new VerticalSegment(s.X1, s.Y1, s.Y2));
		}

		public static IEnumerable<DiagonalSegment> UpwardsDiagonalSegments(this IEnumerable<Segment> segments)
		{
			return segments
				.Where(s => s.Is45Degrees() && s.IsPointingUpwards())
				.Select(s => new DiagonalSegment(
					Math.Min(s.X1, s.X2),
					Math.Min(s.Y1, s.Y2),
					Math.Abs(s.X2 - s.X1) + 1,
					true));
		}

		public static IEnumerable<DiagonalSegment> DownWardsDiagonalSegments(this IEnumerable<Segment> segments)
		{
			return segments
				.Where(s => s.Is45Degrees() && !s.IsPointingUpwards())
				.Select(s => new DiagonalSegment(
					Math.Min(s.X1, s.X2),
					Math.Max(s.Y1, s.Y2),
					Math.Abs(s.X2 - s.X1) + 1,
					false));
		}

		public static bool Is45Degrees(this Segment segment)
		{
			return Math.Abs(segment.Y2 - segment.Y1) == Math.Abs(segment.X2 - segment.X1);
		}

		public static bool IsPointingUpwards(this Segment segment)
		{
			return Math.Sign(segment.Y2 - segment.Y1) == Math.Sign(segment.X2 - segment.X1);
		}

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

		public static void UpdateWith(this int[,] map, DiagonalSegment segment)
		{
			var x = segment.XStart;
			var y = segment.YStart;

			if (segment.IsPointingUpwards)
			{
				for (var i = 0; i < segment.Length; i++)
				{
					map[x + i, y + i]++;
				}

				return;
			}

			for (var i = 0; i < segment.Length; i++)
			{
				map[x + i, y - i]++;
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

	public class DiagonalSegment
	{
		public DiagonalSegment(int xStart, int yStart, int length, bool isPointingUpwards)
		{
			XStart = xStart;
			YStart = yStart;
			Length = length;
			IsPointingUpwards = isPointingUpwards;
		}

		public int XStart { get; set; }
		public int YStart { get; set; }
		public int Length { get; set; }
		public bool IsPointingUpwards { get; set; }
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
