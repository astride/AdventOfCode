using AdventOfCode.Common;
using System;
using System.Collections.Generic;
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
			Part2Solution = SolvePart2(startingPosition1, startingPosition2).ToString();
		}

		private static int SolvePart1(int startingPosition1, int startingPosition2)
		{
			var totalDieRolls = Day21Helpers.PlayDeterministicDice(startingPosition1, startingPosition2, out var losingScore);

			return totalDieRolls * losingScore;
		}

		private static double SolvePart2(int startingPosition1, int startingPosition2)
		{
			return Day21Helpers.PlayDiracDie(startingPosition1, startingPosition2);
		}
	}

	public static class Day21Helpers
	{
		private const int DeterministicDieGoal = 1000;
		private const int DeterministicDieMax = 100;
		private const int BoardMax = 10;
		private const int RollsPerTurn = 3;

		private const int DiracDieGoal = 21;

		private static double DiracDieWinCountPlayer1 = 0;
		private static double DiracDieWinCountPlayer2 = 0;

		private static readonly IDictionary<int, int> DiracDieSum = new Dictionary<int, int> 
		{
			[111] = 3,
			[112] = 4,
			[113] = 5,
			[121] = 4,
			[122] = 5,
			[123] = 6,
			[131] = 5,
			[132] = 6,
			[133] = 7,

			[211] = 4,
			[212] = 5,
			[213] = 6,
			[221] = 5,
			[222] = 6,
			[223] = 7,
			[231] = 6,
			[232] = 7,
			[233] = 8,

			[311] = 5,
			[312] = 6,
			[313] = 7,
			[321] = 6,
			[322] = 7,
			[323] = 8,
			[331] = 7,
			[332] = 8,
			[333] = 9
		};

		private static readonly IDictionary<int, int> UniverseCountForDiracDieSum = DiracDieSum
			.GroupBy(kvp => kvp.Value)
			.ToDictionary(gr => gr.Key, gr => gr.Count());

		public static int PlayDeterministicDice(int startingPosition1, int startingPosition2, out int losingScore)
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

				if (score1 >= DeterministicDieGoal)
				{
					losingScore = score2;
					return totalDieRolls;
				}

				// Player 2's turn
				turn++;
				totalDieRolls += RollsPerTurn;

				position2 = GetBoardSpot(position2 + GetDeterministicDieSum(turn));
				score2 += position2;

				if (score2 >= DeterministicDieGoal)
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

		public static double PlayDiracDie(int startingPosition1, int startingPosition2)
		{
			PlayDiracDieRoundPlayer1(startingPosition1, startingPosition2, 0, 0);

			return Math.Max(DiracDieWinCountPlayer1, DiracDieWinCountPlayer2);
		}

		private static void PlayDiracDieRoundPlayer1(int position1, int position2, int score1, int score2)
		{
			PlayTurnPlayer1(DiracDieSum[111], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[112], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[113], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[121], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[122], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[123], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[131], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[132], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[133], position1, position2, score1, score2);
			
			PlayTurnPlayer1(DiracDieSum[211], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[212], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[213], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[221], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[222], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[223], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[231], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[232], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[233], position1, position2, score1, score2);
			
			PlayTurnPlayer1(DiracDieSum[311], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[312], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[313], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[321], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[322], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[323], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[331], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[332], position1, position2, score1, score2);
			PlayTurnPlayer1(DiracDieSum[333], position1, position2, score1, score2);
		}

		private static void PlayDiracDieRoundPlayer2(int position1, int position2, int score1, int score2)
		{
			PlayTurnPlayer2(DiracDieSum[111], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[112], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[113], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[121], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[122], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[123], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[131], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[132], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[133], position1, position2, score1, score2);
			
			PlayTurnPlayer2(DiracDieSum[211], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[212], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[213], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[221], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[222], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[223], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[231], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[232], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[233], position1, position2, score1, score2);
			
			PlayTurnPlayer2(DiracDieSum[311], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[312], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[313], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[321], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[322], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[323], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[331], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[332], position1, position2, score1, score2);
			PlayTurnPlayer2(DiracDieSum[333], position1, position2, score1, score2);
		}

		private static void PlayTurnPlayer1(int stepCount, int position1, int position2, int score1, int score2)
		{
			position1 = GetBoardSpot(position1 + stepCount);
			score1 += position1;

			if (score1 >= DiracDieGoal)
			{
				DiracDieWinCountPlayer1++;
				return;
			}

			PlayDiracDieRoundPlayer2(position1, position2, score1, score2);
		}

		private static void PlayTurnPlayer2(int stepCount, int position1, int position2, int score1, int score2)
		{
			position2 = GetBoardSpot(position2 + stepCount);
			score2 += position2;

			if (score2 >= DiracDieGoal)
			{
				DiracDieWinCountPlayer2++;
				return;
			}

			PlayDiracDieRoundPlayer1(position1, position2, score1, score2);
		}

		/*
		public static double PlayDiracDie(int startingPosition1, int startingPosition2)
		{
			PlayDiracDieTurnForPlayer1(startingPosition1, startingPosition2, 0, 0);

			return Math.Max(DiracDieWinCountPlayer1, DiracDieWinCountPlayer2);
		}

		private static void PlayDiracDieTurnForPlayer1(int position1, int position2, int score1, int score2)
		{
			foreach (var universeCountForDieSum in UniverseCountForDiracDieSum)
			{
				foreach (var _ in Enumerable.Range(1, universeCountForDieSum.Value))
				{
					CreateUniverseForPlayer1(universeCountForDieSum.Key, position1, position2, score1, score2);
				}
			}
		}

		private static void PlayDiracDieTurnForPlayer2(int position1, int position2, int score1, int score2)
		{
			foreach (var universeCountForDieSum in UniverseCountForDiracDieSum)
			{
				foreach (var _ in Enumerable.Range(1, universeCountForDieSum.Value))
				{
					CreateUniverseForPlayer2(universeCountForDieSum.Key, position1, position2, score1, score2);
				}
			}
		}

		private static void CreateUniverseForPlayer1(int diracDieSum, int position1, int position2, int score1, int score2)
		{
			position1 = GetBoardSpot(position1 + diracDieSum);
			score1 += position1;

			if (score1 >= DiracDieGoal)
			{
				DiracDieWinCountPlayer1++;
				return;
			}

			PlayDiracDieTurnForPlayer2(position1, position2, score1, score2);
		}

		private static void CreateUniverseForPlayer2(int diracDieSum, int position1, int position2, int score1, int score2)
		{
			position2 = GetBoardSpot(position2 + diracDieSum);
			score2 += position2;

			if (score2 >= DiracDieGoal)
			{
				DiracDieWinCountPlayer2++;
				return;
			}

			PlayDiracDieTurnForPlayer1(position1, position2, score1, score2);
		}
		*/
	}
}
