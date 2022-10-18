using Common.Interfaces;

namespace Year2021;

public class Day13Solver : IPuzzleSolver
{
	public string Part1Solution { get; set; } = string.Empty;
	public string Part2Solution { get; set; } = string.Empty;

	public void SolvePuzzle(string[] rawInput)
	{
		List<(int X, int Y)> markedPoints = rawInput
			.TakeWhile(entry => !string.IsNullOrWhiteSpace(entry))
			.Select(entry => entry.Split(','))
			.Select(coordinate => (int.Parse(coordinate[0]), int.Parse(coordinate[1])))
			.ToList();

		List<(char AlongPlane, int Coor)> foldingInstructions = rawInput
			.SkipWhile(entry => !entry.Contains("fold along"))
			.Select(entry => entry.Split(' '))
			.Select(instruction => instruction.Last().Split('='))
			.Select(foldingInstruction => (foldingInstruction[0].Single(), int.Parse(foldingInstruction[1])))
			.ToList();

		Part1Solution = SolvePart1(markedPoints, foldingInstructions).ToString();
		Part2Solution = SolvePart2(markedPoints, foldingInstructions);
	}

	private static int SolvePart1(
		IReadOnlyCollection<(int X, int Y)> markedPoints,
		IEnumerable<(char Plane, int Coor)> foldingInstructions)
	{
		var markedPaper = markedPoints.CreateAndMarkPaper();
		markedPaper.Fold(foldingInstructions.First());

		return markedPaper.CountMarks();
	}

	private static string SolvePart2(
		IReadOnlyCollection<(int X, int Y)> markedPoints,
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

	public static bool[,] CreateAndMarkPaper(this IReadOnlyCollection<(int X, int Y)> pointsToMark)
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
		switch (instruction.AlongPlane)
		{
			case 'x':
				paper.FoldAlongX(instruction.Coor);
				return;
			case 'y':
				paper.FoldAlongY(instruction.Coor);
				return;
		}
	}

	public static int CountMarks(this bool[,] paper)
	{
		return paper
			.Cast<bool>()
			.Count(point => point); // == true when point is marked
	}
	
	private static void FoldAlongX(this bool[,] paper, int coordinate)
	{
		foreach (var x in Enumerable.Range(0, coordinate + 1))
		{
			var mirroredX = coordinate + (coordinate - x);

			if (mirroredX >= SizeX)
			{
				continue;
			}

			foreach (var y in Enumerable.Range(0, SizeY))
			{
				paper[x, y] = paper[x, y] || paper[mirroredX, y];
				paper[mirroredX, y] = false;
			}
		}
	}

	private static void FoldAlongY(this bool[,] paper, int coordinate)
	{
		foreach (var y in Enumerable.Range(0, coordinate + 1))
		{
			var mirroredY = coordinate + (coordinate - y);

			if (mirroredY >= SizeY)
			{
				continue;
			}

			foreach (var x in Enumerable.Range(0, SizeX))
			{
				paper[x, y] = paper[x, y] || paper[x, mirroredY];
				paper[x, mirroredY] = false;
			}
		}
	}
}
