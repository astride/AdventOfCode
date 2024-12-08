using Common.Interfaces;

namespace Year2024;

public class Day05Solver : IPuzzleSolver
{
	public string Title => "Print Queue";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private static string[] PageOrderingRules;
	
	private static Dictionary<string, List<string>> RequiredPreviousPageNumbersByPageNumber;

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		ExtractPageOrderingRules(input);
		ExtractRequiredPreviousPageNumbersByPageNumber();
		
		var updates = input.Skip(PageOrderingRules.Length + 1);

		var validUpdates = updates
			.Select(update => update.Split(',').ToList())
			.Where(IsValidUpdate);
		
		var middlePageNumbers = validUpdates
			.SelectMany(update => update.Skip(update.Count/2).Take(1))
			.Select(int.Parse);

		return middlePageNumbers.Sum();
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		ExtractPageOrderingRules(input);
		ExtractRequiredPreviousPageNumbersByPageNumber();
		
		var updates = input.Skip(PageOrderingRules.Length + 1);
		
		var invalidUpdates = updates
			.Select(update => update.Split(',').ToList())
			.Where(update => !IsValidUpdate(update));

		var correctedUpdates = invalidUpdates.Select(AmendUpdate);

		var middlePageNumbers = correctedUpdates
			.SelectMany(update => update.Skip(update.Count/2).Take(1))
			.Select(int.Parse);

		return middlePageNumbers.Sum();

		List<string> AmendUpdate(List<string> update)
		{
			var pageNumbersToCheck = update.SkipLast(1).ToArray();

			foreach (var pageNumber in pageNumbersToCheck)
			{
				if (!RequiredPreviousPageNumbersByPageNumber.TryGetValue(pageNumber, out var requiredPreviousPageNumbers))
				{
					continue;
				}

				var sourceIndex = update.IndexOf(pageNumber);
				var targetIndex = requiredPreviousPageNumbers.Max(update.IndexOf);

				if (sourceIndex < targetIndex)
				{
					update.RemoveAt(sourceIndex);
					update.Insert(targetIndex, pageNumber);
				}
			}

			return update;
		}
	}

	private static void ExtractPageOrderingRules(IEnumerable<string> sleighLaunchSafetyManual)
	{
		PageOrderingRules = sleighLaunchSafetyManual
			.TakeWhile(line => !string.IsNullOrEmpty(line))
			.ToArray();
	}

	private static void ExtractRequiredPreviousPageNumbersByPageNumber()
	{
		RequiredPreviousPageNumbersByPageNumber = new Dictionary<string, List<string>>();

		foreach (var rule in PageOrderingRules)
		{
			var pageNumbers = rule.Split('|').ToArray();

			if (!RequiredPreviousPageNumbersByPageNumber.TryAdd(pageNumbers[1], new List<string> { pageNumbers[0] }))
			{
				RequiredPreviousPageNumbersByPageNumber[pageNumbers[1]].Add(pageNumbers[0]);
			}
		}
	}
	
	private static bool IsValidUpdate(List<string> pagesToPrint)
	{
		for (var i = 0; i < pagesToPrint.Count; i++)
		{
			var pageNumberToPrint = pagesToPrint[i];

			if (!RequiredPreviousPageNumbersByPageNumber.TryGetValue(pageNumberToPrint, out var requiredPageNumbers))
			{
				continue;
			}

			// Assuming that if a required page number is scheduled to be printed _after_ the current page number,
			// it has not been printed _before_ the current page number
			if (requiredPageNumbers.Intersect(pagesToPrint.Skip(i)).Any())
			{
				return false;
			}
		}

		return true;
	}
}