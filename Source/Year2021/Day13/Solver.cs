using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Year2021;

public class Day13Solver : IPuzzleSolver
{
	public string Part1Solution { get; set; }
	public string Part2Solution { get; set; }

	public void SolvePuzzle(string[] rawInput)
	{
		IEnumerable<(int X, int Y)> markedPoints = rawInput
			.TakeWhile(entry => !string.IsNullOrWhiteSpace(entry))
			.Select(entry => entry.Split(','))
			.Select(coor => (int.Parse(coor[0]), int.Parse(coor[1])));

		List<(char AlongPlane, int Coor)> foldingInstructions = rawInput
			.SkipWhile(entry => !entry.Contains("fold along"))
			.Where(entry => entry != null)
			.Select(entry => entry.Split(' '))
			.Select(instruction => instruction.Last().Split('='))
			.Select(foldingInstruction => (foldingInstruction[0].Single(), int.Parse(foldingInstruction[1])))
			.ToList();

		Part1Solution = SolvePart1(markedPoints, foldingInstructions).ToString();
		Part2Solution = SolvePart2(markedPoints, foldingInstructions);
	}

	private static int SolvePart1(
		IEnumerable<(int X, int Y)> markedPoints,
		List<(char Plane, int Coor)> foldingInstructions)
	{
		var markedPaper = markedPoints.CreateAndMarkPaper();
		markedPaper.Fold(foldingInstructions.First());

		return markedPaper.CountMarks();
	}

	private static string SolvePart2(
		IEnumerable<(int X, int Y)> markedPoints,
		List<(char Plane, int Coor)> foldingInstructions)
	{
		var markedPaper = markedPoints.CreateAndMarkPaper();
		
		foreach (var instruction in foldingInstructions)
		{
			markedPaper.Fold(instruction);
		}

		var maxX = foldingInstructions
			.Where(instruction => instruction.Plane == 'x')
			.Select(instruction => instruction.Coor)
			.Min();

		var maxY = foldingInstructions
			.Where(instruction => instruction.Plane == 'y')
			.Select(instruction => instruction.Coor)
			.Min();

		foreach (var y in Enumerable.Range(0, maxY))
		{
			foreach (var x in Enumerable.Range(0, maxX))
			{
				Console.Write(markedPaper[x, y] ? "# " : "  ");
			}

			Console.WriteLine();
		}

		return "Read the note yourself --^";
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

	public static int CountMarks(this bool[,] paper)
	{
		var count = 0;

		foreach (var point in paper)
		{
			if (point) // == true when point is marked
			{
				count++;
			}
		}

		return count;
	}
	
	private static void FoldAlongX(this bool[,] paper, int coordinate)
	{
		int mirroredX;

		foreach (var x in Enumerable.Range(0, coordinate + 1))
		{
			mirroredX = coordinate + (coordinate - x);

			foreach (var y in Enumerable.Range(0, SizeY))
			{
				if (mirroredX < SizeX)
				{
					paper[x, y] = paper[x, y] || paper[mirroredX, y];
					paper[mirroredX, y] = false;
				}
			}
		}
	}

	private static void FoldAlongY(this bool[,] paper, int coordinate)
	{
		int mirroredY;

		foreach (var y in Enumerable.Range(0, coordinate + 1))
		{
			mirroredY = coordinate + (coordinate - y);

			foreach (var x in Enumerable.Range(0, SizeX))
			{
				if (mirroredY < SizeY)
				{
					paper[x, y] = paper[x, y] || paper[x, mirroredY];
					paper[x, mirroredY] = false;
				}
			}
		}
	}
}
