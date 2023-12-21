using Common.Extensions;
using Common.Helpers;
using Common.Interfaces;

namespace Year2023;

public class Day19Solver : IPuzzleSolver
{
	public string Title => "Aplenty";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var workflowDescriptions = input.TakeWhile(line => !string.IsNullOrEmpty(line));

		var workflows = new Dictionary<string, Workflow>();

		foreach (var workflowDescription in workflowDescriptions)
		{
			var workflow = new Workflow(workflowDescription);

			workflows[workflow.Name] = workflow;
		}
		
		var totalRatingNumberSum = 0L;

		var ratingDescriptions = input.SkipWhile(line => !string.IsNullOrEmpty(line)).Skip(1);

		foreach (var partRatingDescription in ratingDescriptions)
		{
			var partRating = PartRating.FromDescription(partRatingDescription);

			var workflowTarget = "in";

			while (workflowTarget != "A" && workflowTarget != "R")
			{
				var workflow = workflows[workflowTarget];

				workflowTarget = workflow.GetTargetForPart(partRating);
			}

			if (workflowTarget == "A")
			{
				totalRatingNumberSum += partRating.Sum;
			}
		}

		return totalRatingNumberSum;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}

	private class Workflow
	{
		public Workflow(string description)
		{
			Name = GetName(description);

			GenerateRules(description);
		}

		private readonly List<Func<PartRating, string?>> _rules = new();

		public string Name { get; }

		public string GetTargetForPart(PartRating part)
		{
			foreach (var rule in _rules)
			{
				var target = rule(part);

				if (!string.IsNullOrEmpty(target))
				{
					return target;
				}
			}

			throw new AggregateException("No unconditional rule found");
		}

		private static string GetName(string description) => description.Split('{')[0];

		private void GenerateRules(string description)
		{
			var ruleDescriptions = description.Split('{')[1].Without("}").Split(',');

			foreach (var ruleDescription in ruleDescriptions)
			{
				if (!ruleDescription.Contains(':'))
				{
					// Unconditional rule; always comes last
					_rules.Add(_ => ruleDescription);
					return;
				}

				var ratingLimit = RegexHelper.GetFirstNumberAsInt(ruleDescription);
				var category = RegexHelper.GetFirstWord(ruleDescription);
				var lessThanLimit = ruleDescription.Contains('<');

				var target = ruleDescription.Split(':')[^1];

				Func<PartRating, string?> rule = category switch
				{
					"x" when lessThanLimit => rating => rating.X < ratingLimit ? target : null,
					"x" => rating => rating.X > ratingLimit ? target : null,
					"m" when lessThanLimit => rating => rating.M < ratingLimit ? target : null,
					"m" => rating => rating.M > ratingLimit ? target : null,
					"a" when lessThanLimit => rating => rating.A < ratingLimit ? target : null,
					"a" => rating => rating.A > ratingLimit ? target : null,
					"s" when lessThanLimit => rating => rating.S < ratingLimit ? target : null,
					"s" => rating => rating.S > ratingLimit ? target : null,
					_ => throw new AggregateException("Could not create rule from rule description")
				};
				
				_rules.Add(rule);
			}
		}
	}

	private class PartRating
	{
		private PartRating(params int[] ratings)
		{
			X = ratings[0];
			M = ratings[1];
			A = ratings[2];
			S = ratings[3];
		}

		public int X { get; }
		public int M { get; }
		public int A { get; }
		public int S { get; }

		public int Sum => X + M + A + S;

		public static PartRating FromDescription(string description)
		{
			var ratings = description.Without("{").Without("}");

			return new PartRating(RegexHelper.GetAllNumbersAsInt(ratings).ToArray());
		}
	}
}