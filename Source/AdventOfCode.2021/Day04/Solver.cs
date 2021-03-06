using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day04Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var drawStack = rawInput
				.First()
				.Split(',')
				.Select(entry => int.Parse(entry))
				.ToList();

			var input = rawInput
				.Skip(1)
				.SkipWhile(line => string.IsNullOrWhiteSpace(line));

			var boards = new List<List<int>>();

			while (input != null && input.Any())
			{
				var board = input
					.TakeWhile(line => !string.IsNullOrWhiteSpace(line))
					.SelectMany(row => row
						.Split(' ')
						.Where(entry => !string.IsNullOrWhiteSpace(entry))
						.Select(entry => int.Parse(entry)))
					.ToList();

				boards.Add(board);

				input = input
					.SkipWhile(line => !string.IsNullOrWhiteSpace(line))
					.SkipWhile(line => string.IsNullOrWhiteSpace(line));
			}

			Part1Solution = SolvePart1(boards, drawStack).ToString();
			Part2Solution = SolvePart2(boards, drawStack).ToString();
		}

		private static int SolvePart1(List<List<int>> boards, List<int> drawStack)
		{
			var playingBoards = boards
				.Select(board => board
					.Select(entry => (int?)entry)
					.ToList())
				.ToList();

			var winnerBoard = playingBoards.GetWinner(drawStack.ToList(), out var lastDrawnNumber);

			var finalScore = winnerBoard.CalculateScore(lastDrawnNumber);

			//TODO Handle when null

			return finalScore.Value;
		}

		private static int SolvePart2(List<List<int>> boards, List<int> drawStack)
		{
			var remainingBoards = boards
				.Select(board => board
					.Select(entry => (int?)entry)
					.ToList())
				.ToList();

			var remainingDrawStack = drawStack.ToList();

			while (remainingBoards.Count() > 1)
			{
				remainingBoards.RemoveNextWinner(remainingDrawStack);
			}

			var lastBoard = remainingBoards.Single();

			foreach (var number in remainingDrawStack)
			{
				if (lastBoard.HasBingoWith(number))
				{
					var finalScore = lastBoard.CalculateScore(number);

					return finalScore.Value;
				}
			}

			return -1;
		}
	}

	static class Day04Helpers
	{
		private const int RowSize = 5;
		private const int RowCount = RowSize;

		public static List<int?> GetWinner(this List<List<int?>> boards, List<int> drawStack, out int lastDrawnNumber)
		{
			foreach (var number in drawStack)
			{
				foreach (var board in boards)
				{
					if (board.HasBingoWith(number))
					{
						drawStack.RemoveRange(0, drawStack.IndexOf(number)); // remove numbers that have been played on all boards from stack

						lastDrawnNumber = number;
						return board;
					}
				}
			}

			//TODO Inform about no winning board
			lastDrawnNumber = -1;
			return new List<int?>();
		}

		public static void RemoveNextWinner(this List<List<int?>> boards, List<int> drawStack)
		{
			boards.Remove(boards.GetWinner(drawStack, out _));
		}

		public static bool HasBingoWith(this List<int?> board, int number)
		{
			if (board.Contains(number))
			{
				board[board.IndexOf(number)] = null; // mark number

				return board.HasBingo();
			}

			return false;
		}

		public static bool HasBingo(this List<int?> board)
		{
			foreach (var rowNumber in Enumerable.Range(1, RowCount))
			{
				if (board.GetRow(rowNumber).IsBingo() || // check board rows
					board.Transpose().GetRow(rowNumber).IsBingo()) // check board cols
				{
					return true;
				}
			}

			return false;
		}

		public static List<int?> GetRow(this List<int?> board, int rowNumber)
		{
			return board
				.Skip(RowSize * (rowNumber - 1))
				.Take(RowSize)
				.ToList();
		}

		public static bool IsBingo(this List<int?> row)
		{
			return row.All(number => !number.HasValue);
		}

		public static List<int?> Transpose(this List<int?> board)
		{
			var rows = Enumerable.Range(1, RowCount)
				.Select(rowNumber => board.GetRow(rowNumber))
				.ToList();

			return Enumerable.Range(0, RowSize)
				.SelectMany(colIndex => rows
					.Select(row => row[colIndex]))
				.ToList();
		}

		public static int? CalculateScore(this List<int?> board, int lastDrawnNumber)
		{
			return lastDrawnNumber * board.Where(entry => entry.HasValue).Sum();
		}
	}
}
