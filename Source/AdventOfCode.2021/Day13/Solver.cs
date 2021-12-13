using AdventOfCode.Common;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day13Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			IEnumerable<(int X, int Y)> markedPoints = rawInput
				.TakeWhile(entry => !string.IsNullOrWhiteSpace(entry))
				.Select(instruction => instruction.Split(','))
				.Select(pointToMark => (int.Parse(pointToMark[0]), int.Parse(pointToMark[1])));

			List<(char AlongPlane, int Coor)> foldingInstructions = rawInput
				.SkipWhile(entry => !entry.Contains("fold along"))
				.Where(instruction => instruction != null)
				.Select(instruction => instruction.Split(' '))
				.Select(instruction => instruction.Last().Split('='))
				.Select(foldingInstruction => (foldingInstruction[0].Single(), int.Parse(foldingInstruction[1])))
				.ToList();

			Part1Solution = SolvePart1(markedPoints, foldingInstructions).ToString();
		}

		private static int SolvePart1(
			IEnumerable<(int X, int Y)> markedPoints,
			List<(char Plane, int Coor)> foldingInstructions)
		{
			var markedPaper = markedPoints.CreateAndMarkPaper();
			markedPaper.Fold(foldingInstructions.First());

			var marks = 0;

			foreach (var point in markedPaper)
			{
				if (point) // == true when point is marked
				{
					marks++;
				}
			}

			return marks;
		}
	}

	public static class Day13Helpers
	{
		private static int SizeX;
		private static int SizeY;

		public static bool[,] CreateAndMarkPaper(this IEnumerable<(int X, int Y)> pointsToMark)
		{
			SizeX = pointsToMark
				.Select(point => point.X)
				.Max() + 1;

			SizeY = pointsToMark
				.Select(point => point.Y)
				.Max() + 1;

			var paper = new bool[SizeX, SizeY];

			foreach (var point in pointsToMark)
			{
				paper[point.X, point.Y] = true;
			}

			return paper;
		}

		public static void Fold(this bool[,] paper, (char AlongPlane, int Coor) instruction)
		{
			if (instruction.AlongPlane == 'x') paper.FoldAlongX(instruction.Coor);
			else if (instruction.AlongPlane == 'y') paper.FoldAlongY(instruction.Coor);
		}

		private static void FoldAlongX(this bool[,] paper, int coordinate)
		{
			int foldedX;

			foreach (var x in Enumerable.Range(0, coordinate + 1))
			{
				foldedX = coordinate + (coordinate - x);

				foreach (var y in Enumerable.Range(0, SizeY))
				{
					if (foldedX < SizeX)
					{
						paper[x, y] = paper[x, y] || paper[foldedX, y];
						paper[foldedX, y] = false;
					}
				}
			}
		}

		private static void FoldAlongY(this bool[,] paper, int coordinate)
		{
			int foldedY;

			foreach (var y in Enumerable.Range(0, coordinate + 1))
			{
				foldedY = coordinate + (coordinate - y);

				foreach (var x in Enumerable.Range(0, SizeX))
				{
					if (foldedY < SizeY)
					{
						paper[x, y] = paper[x, y] || paper[x, foldedY];
						paper[x, foldedY] = false;
					}
				}
			}
		}
	}
}
