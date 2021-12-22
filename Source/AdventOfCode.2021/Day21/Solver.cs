using AdventOfCode.Common;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day21Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var startingPosition1 = 
				int.Parse(rawInput
					.Single(entry => entry.Contains("Player 1"))
					.Split(' ')
					.Last());

			var startingPosition2 =
				int.Parse(rawInput
					.Single(entry => entry.Contains("Player 2"))
					.Split(' ')
					.Last());

			Part1Solution = SolvePart1(startingPosition1, startingPosition2).ToString();
		}

		private static int SolvePart1(int startingPosition1, int startingPosition2)
		{
			var totalDieRolls = Day21Helpers.PracticeDiracDice(startingPosition1, startingPosition2, out var losingScore);

			return totalDieRolls * losingScore;
		}
	}

	public static class Day21Helpers
	{
		private const int Goal = 1000;
		private const int DeterministicDieMax = 100;
		private const int BoardMax = 10;
		private const int RollsPerTurn = 3;

		public static int PracticeDiracDice(int startingPosition1, int startingPosition2, out int losingScore)
		{
			var turn = 0;
			var totalDieRolls = 0;

			var score1 = 0;
			var score2 = 0;

			var position1 = startingPosition1;
			var position2 = startingPosition2;

			while (true)
			{
				// Player 1's turn
				turn++;
				totalDieRolls += RollsPerTurn;

				position1 = GetBoardSpot(position1 + GetDeterministicDieSum(turn));
				score1 += position1;

				if (score1 >= Goal)
				{
					losingScore = score2;
					return totalDieRolls;
				}

				// Player 2's turn
				turn++;
				totalDieRolls += RollsPerTurn;

				position2 = GetBoardSpot(position2 + GetDeterministicDieSum(turn));
				score2 += position2;

				if (score2 >= Goal)
				{
					losingScore = score1;
					return totalDieRolls;
				}
			}
		}

		private static int GetDeterministicDieSum(int turn)
		{
			var baseScore = RollsPerTurn * (turn - 1);

			var dieSum =
				GetDeterministicDieValue(baseScore + 1) +
				GetDeterministicDieValue(baseScore + 2) +
				GetDeterministicDieValue(baseScore + 3);

			return dieSum;
		}

		private static int GetDeterministicDieValue(int absoluteValue)
		{
			return 1 + ((absoluteValue - 1) % DeterministicDieMax);
		}

		private static int GetBoardSpot(int score)
		{
			return 1 + ((score - 1) % BoardMax);
		}
	}
}
