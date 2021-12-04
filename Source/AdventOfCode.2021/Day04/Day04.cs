using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day04 : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var drawStack = rawInput
				.First()
				.Split(',')
				.Select(entry => int.Parse(entry))
				.ToArray();

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

			var boardsInPlay = boards
				.Select(board => board
					.Select(entry => (int?)entry)
					.ToList())
				.ToList();

			Part1Solution = SolvePart1(boardsInPlay, drawStack).ToString();
			Part2Solution = SolvePart2(boardsInPlay, drawStack).ToString();
		}

		private static int SolvePart1(List<List<int?>> boards, int[] drawStack)
		{
			var winnerBoard = boards.GetWinner(drawStack, out var lastDrawnNumber);

			var finalScore = winnerBoard.CalculateScore(lastDrawnNumber);

			//TODO Handle when null

			return finalScore.Value;
		}

		private static int SolvePart2(List<List<int?>> boards, int[] drawStack)
		{
			while (boards.Count() > 1)
			{
				boards.Remove(boards.GetWinner(drawStack, out _));
			}

			var lastWinningBoard = boards.GetWinner(drawStack, out var lastDrawnNumber);

			var finalScore = lastWinningBoard.CalculateScore(lastDrawnNumber);

			//TODO Handle when null

			return finalScore.Value;
		}
	}

	static class Day04Helpers
	{
		private const int RowSize = 5;
		private const int RowCount = RowSize;

		public static List<int?> GetWinner(this List<List<int?>> boards, int[] drawStack, out int lastDrawnNumber)
		{
			foreach (var number in drawStack)
			{
				foreach (var board in boards)
				{
					var bingo = board.Play(number);

					if (bingo)
					{
						lastDrawnNumber = number;
						return board;
					}
				}
			}

			//TODO Inform about no winning board
			lastDrawnNumber = 0;
			return new List<int?>();
		}

		public static bool Play(this List<int?> board, int number)
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
			foreach (var i in Enumerable.Range(0, RowCount))
			{
				if (board.GetRow(i).IsBingo() || // check board rows
					board.Transpose().GetRow(i).IsBingo()) // check board cols
				{
					return true;
				}
			}

			return false;
		}

		public static List<int?> GetRow(this List<int?> board, int rowIndex) => board
			.Skip(RowSize * rowIndex)
			.Take(RowSize)
			.ToList();

		public static bool IsBingo(this List<int?> row) => row.All(number => !number.HasValue);

		public static List<int?> Transpose(this List<int?> board)
		{
			var rows = Enumerable.Range(0, RowCount)
				.Select(iRow => board.GetRow(iRow))
				.ToList();

			return Enumerable.Range(0, RowSize)
				.SelectMany(iCol => rows
					.Select(row => row[iCol]))
				.ToList();
		}

		public static int? CalculateScore(this List<int?> board, int lastDrawnNumber)
		{
			return lastDrawnNumber * board.Where(entry => entry.HasValue).Sum();
		}
	}
}
