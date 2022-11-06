using System.Globalization;
using Common.Interfaces;

namespace Year2021;

public class Day21Solver : IPuzzleSolver
{
	public string Title => "Dirac Dice";
	public string Part1Solution { get; set; } = string.Empty;
	public string Part2Solution { get; set; } = string.Empty;

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
		Part2Solution = SolvePart2(startingPosition1, startingPosition2).ToString(CultureInfo.InvariantCulture);
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

	private static double DiracDieWinCountPlayer1;
	private static double DiracDieWinCountPlayer2;

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
		foreach (var dieSum in UniverseCountForDiracDieSum.Keys)
		{
			PlayDiracDieInUniverse(dieSum, startingPosition1, startingPosition2);
		}

		return Math.Max(DiracDieWinCountPlayer1, DiracDieWinCountPlayer2);
	}

	private static void PlayDiracDieInUniverse(int dieSum, int position1, int position2, int score1 = 0, int score2 = 0)
	{
		// Player 1 always starts
		position1 = GetBoardSpot(position1 + dieSum);
		score1 += position1;

		var previousTurnRolls = new List<int> { dieSum };

		foreach (var nextDieSum in UniverseCountForDiracDieSum.Keys)
		{
			previousTurnRolls.PlayDiracDieInUniverse(nextDieSum, position1, position2, score1, score2);
		}
	}

	private static void PlayDiracDieInUniverse(this List<int> previousDieSums, int dieSum, 
		int position1, int position2, int score1, int score2)
	{
		// Player 1's turn if count of previous die sums is even; player 2's turn otherwise
		if (previousDieSums.Count % 2 == 0)
		{
			position1 = GetBoardSpot(position1 + dieSum);
			score1 += position1;

			if (score1 >= DiracDieGoal)
			{
				UpdateDiracDieWinCountForPlayer1(previousDieSums.Append(dieSum));
				return;
			}
		}
		else
		{
			position2 = GetBoardSpot(position2 + dieSum);
			score2 += position2;

			if (score2 >= DiracDieGoal)
			{
				UpdateDiracDieWinCountForPlayer2(previousDieSums.Append(dieSum));
				return;
			}
		}

		foreach (var nextDieSum in UniverseCountForDiracDieSum.Keys)
		{
			previousDieSums.Append(dieSum).ToList().PlayDiracDieInUniverse(nextDieSum, position1, position2, score1, score2);
		}
	}

	private static void UpdateDiracDieWinCountForPlayer1(IEnumerable<int> dieSums)
	{
		DiracDieWinCountPlayer1 += dieSums
			.Select(ds => UniverseCountForDiracDieSum[ds])
			.Aggregate((double)1, (total, next) => total * next);
	}

	private static void UpdateDiracDieWinCountForPlayer2(IEnumerable<int> dieSums)
	{
		DiracDieWinCountPlayer2 += dieSums
			.Select(ds => UniverseCountForDiracDieSum[ds])
			.Aggregate((double)1, (total, next) => total * next);
	}
}
