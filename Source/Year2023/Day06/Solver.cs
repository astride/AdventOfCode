using System.Text.RegularExpressions;
using Common.Interfaces;

namespace Year2023;

public class Day06Solver : IPuzzleSolver
{
	public string Title => "Wait For It";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private static readonly Regex NumberRegex = new(@"\d+");

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var raceTimes = NumberRegex.Matches(input[0]).Select(match => int.Parse(match.ToString())).ToArray();
		var currentDistanceRecords = NumberRegex.Matches(input[1]).Select(match => int.Parse(match.ToString())).ToArray();

		var waysToWinMultiplied = 1;

		for (var i = 0; i < raceTimes.Length; i++)
		{
			var raceTime = raceTimes[i];

			var waysToWin = 0;

			var raceTimeIsEvenNumber = raceTime % 2 == 0;
			var raceHalfTime = raceTime / 2;

			for (var buttonHoldTime = 1; buttonHoldTime <= raceHalfTime; buttonHoldTime++)
			{
				var traveledDistance = buttonHoldTime * (raceTime - buttonHoldTime);

				if (traveledDistance > currentDistanceRecords[i])
				{
					waysToWin += 2 * (1 + raceHalfTime - buttonHoldTime);
					
					if (raceTimeIsEvenNumber)
					{
						waysToWin--;
					}

					waysToWinMultiplied *= waysToWin;

					break;
				}
			}
		}

		return waysToWinMultiplied;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var raceTime = double.Parse(string.Join(string.Empty, NumberRegex.Matches(input[0]).Select(match => match.ToString())));
		var currentDistanceRecord = double.Parse(string.Join(string.Empty, NumberRegex.Matches(input[1]).Select(match => match.ToString())));
		
		var raceTimeIsEvenNumber = raceTime % 2 == 0;
		var raceHalfTime = raceTime / 2;

		for (var buttonHoldTime = 1; buttonHoldTime <= raceHalfTime; buttonHoldTime++)
		{
			var traveledDistance = buttonHoldTime * (raceTime - buttonHoldTime);

			if (traveledDistance > currentDistanceRecord)
			{
				var waysToWin = 2 * (1 + raceHalfTime - buttonHoldTime);
				
				if (raceTimeIsEvenNumber)
				{
					waysToWin--;
				}

				return waysToWin;
			}
		}

		return 0;
	}
}