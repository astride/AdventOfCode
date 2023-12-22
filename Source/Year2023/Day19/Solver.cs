using System.Collections;
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
		const int ratingMin = 1;
		const int ratingMax = 4000;

		var workflowDescriptions = input
			.TakeWhile(line => !string.IsNullOrEmpty(line))
			.Select(wd => new WorkflowDescription(wd))
			.ToList();

		#region Removing redundant workflows

		var approvedWorkflows = new Queue();
		var rejectedWorkflows = new Queue();

		foreach (var description in workflowDescriptions)
		{
			var ruleTargets = description.Rules
				.Select(rule => rule.Target)
				.ToList();
			
			if (ruleTargets.All(target => target == "A"))
			{
				approvedWorkflows.Enqueue(description);
			}
			else if (ruleTargets.All(target => target == "R"))
			{
				rejectedWorkflows.Enqueue(description);
			}
		}
		
		while (approvedWorkflows.Count > 0)
		{
			var approvedWorkflow = (approvedWorkflows.Dequeue() as WorkflowDescription)!;

			workflowDescriptions.Remove(approvedWorkflow);

			foreach (var workflowDescription in workflowDescriptions)
			{
				foreach (var approvedCondition in workflowDescription.Rules.Where(rule => rule.Target == approvedWorkflow.Name))
				{
					approvedCondition.Target = "A";
				}

				if (workflowDescription.Rules.All(rule => rule.Target == "A"))
				{
					approvedWorkflows.Enqueue(workflowDescription);
				}
			}
		}

		while (rejectedWorkflows.Count > 0)
		{
			var rejectedWorkflow = (rejectedWorkflows.Dequeue() as WorkflowDescription)!;

			workflowDescriptions.Remove(rejectedWorkflow);

			foreach (var workflowDescription in workflowDescriptions)
			{
				foreach (var rejectedCondition in workflowDescription.Rules.Where(rule => rule.Target == rejectedWorkflow.Name))
				{
					rejectedCondition.Target = "R";
				}

				if (workflowDescription.Rules.All(rule => rule.Target == "R"))
				{
					rejectedWorkflows.Enqueue(workflowDescription);
				}
			}
		}

		#endregion 

		#region Collecting batch starting numbers for X, M, A and S

		var batchStartingNumbersX = new HashSet<int> { ratingMin };
		var batchStartingNumbersM = new HashSet<int> { ratingMin };
		var batchStartingNumbersA = new HashSet<int> { ratingMin };
		var batchStartingNumbersS = new HashSet<int> { ratingMin };

		foreach (var workflowDescription in workflowDescriptions)
		{
			foreach (var condition in workflowDescription.Rules.Select(rule => rule.Condition))
			{
				if (string.IsNullOrEmpty(condition))
				{
					continue;
				}

				var targetValue = int.Parse(condition[2..]);

				switch (condition[..2])
				{
					case "x<":
						batchStartingNumbersX.Add(targetValue);
						continue;
					case "x>":
						batchStartingNumbersX.Add(targetValue + 1);
						continue;
					case "m<":
						batchStartingNumbersM.Add(targetValue);
						continue;
					case "m>":
						batchStartingNumbersM.Add(targetValue + 1);
						continue;
					case "a<":
						batchStartingNumbersA.Add(targetValue);
						continue;
					case "a>":
						batchStartingNumbersA.Add(targetValue + 1);
						continue;
					case "s<":
						batchStartingNumbersS.Add(targetValue);
						continue;
					case "s>":
						batchStartingNumbersS.Add(targetValue + 1);
						continue;
				}
			}
		}
		
		#endregion
		
		#region Creating batches for X, M, A and S

		var batchesX = new List<Batch>();
		var batchesM = new List<Batch>();
		var batchesA = new List<Batch>();
		var batchesS = new List<Batch>();

		var orderedBatchStartingNumbersX = batchStartingNumbersX.OrderBy(number => number).ToList();
		var orderedBatchStartingNumbersM = batchStartingNumbersM.OrderBy(number => number).ToList();
		var orderedBatchStartingNumbersA = batchStartingNumbersA.OrderBy(number => number).ToList();
		var orderedBatchStartingNumbersS = batchStartingNumbersS.OrderBy(number => number).ToList();

		for (var i = 0; i < orderedBatchStartingNumbersX.Count; i++)
		{
			var startingNumber = orderedBatchStartingNumbersX[i];

			var startingNumberNextBatch = i < orderedBatchStartingNumbersX.Count - 1
				? orderedBatchStartingNumbersX[i + 1]
				: 1 + ratingMax;

			batchesX.Add(new Batch(startingNumber, startingNumberNextBatch - startingNumber));
		}

		for (var i = 0; i < orderedBatchStartingNumbersM.Count; i++)
		{
			var startingNumber = orderedBatchStartingNumbersM[i];

			var startingNumberNextBatch = i < orderedBatchStartingNumbersM.Count - 1
				? orderedBatchStartingNumbersM[i + 1]
				: 1 + ratingMax;

			batchesM.Add(new Batch(startingNumber, startingNumberNextBatch - startingNumber));
		}

		for (var i = 0; i < orderedBatchStartingNumbersA.Count; i++)
		{
			var startingNumber = orderedBatchStartingNumbersA[i];

			var startingNumberNextBatch = i < orderedBatchStartingNumbersA.Count - 1
				? orderedBatchStartingNumbersA[i + 1]
				: 1 + ratingMax;

			batchesA.Add(new Batch(startingNumber, startingNumberNextBatch - startingNumber));
		}

		for (var i = 0; i < orderedBatchStartingNumbersS.Count; i++)
		{
			var startingNumber = orderedBatchStartingNumbersS[i];

			var startingNumberNextBatch = i < orderedBatchStartingNumbersS.Count - 1
				? orderedBatchStartingNumbersS[i + 1]
				: 1 + ratingMax;

			batchesS.Add(new Batch(startingNumber, startingNumberNextBatch - startingNumber));
		}

		#endregion
		
		#region Try all different batch combinations
		
		var workflows = new Dictionary<string, Workflow>();

		foreach (var workflowDescription in workflowDescriptions)
		{
			var workflow = new Workflow(workflowDescription.Description);

			workflows[workflow.Name] = workflow;
		}

		var distinctAcceptedRatingCombinations = 0L;

		foreach (var batchX in batchesX)
		{
			foreach (var batchM in batchesM)
			{
				foreach (var batchA in batchesA)
				{
					foreach (var batchS in batchesS)
					{
						var batchStartingNumbers = new[] { batchX, batchM, batchA, batchS }
							.Select(batch => batch.StartingNumber)
							.ToArray();

						var workflowTarget = "in";

						while (workflowTarget != "A" && workflowTarget != "R")
						{
							var workflow = workflows[workflowTarget];

							workflowTarget = workflow.GetTargetForPart(new PartRating(batchStartingNumbers));
						}

						if (workflowTarget == "A")
						{
							distinctAcceptedRatingCombinations += (long)batchX.Count * batchM.Count * batchA.Count * batchS.Count;
						}
					}
				}
			}
		}
		
		#endregion
		
		return distinctAcceptedRatingCombinations;
	}

	private object GetPart2SolutionForExampleInput(string[] input)
	{
		var A = 1415L;
		var B = 1025L;
		var C = 222L;
		var D = 1338L;
		var E = 838L;
		var F = 962L;
		var G = 290L;
		var H = 1910L;
		var I = 1716L;
		var J = 289L;
		var K = 1995L;
		var L = 536L;
		var M = 814L;
		var N = 1420L;
		var O = 1230L;

		var sets = new[]
		{
			new[] { A, H, K, L },
			new[] { A, H, K, M },
			new[] { B, H, K, L },
			new[] { B, H, K, M },
			new[] { C, H, K, L },
			new[] { C, H, K, M },
			new[] { D, H, K, L },
			new[] { D, H, K, M },
			new[] { A, E, I, L },
			new[] { A, E, I, M },
			new[] { A, E, J, L },
			new[] { A, E, J, M },
			new[] { A, F, I, L },
			new[] { A, F, I, M },
			new[] { A, F, J, L },
			new[] { A, F, J, M },
			new[] { A, G, I, L },
			new[] { A, G, I, M },
			new[] { A, G, J, L },
			new[] { A, G, J, M },
			new[] { A, H, I, L },
			new[] { A, H, I, M },
			new[] { A, H, J, L },
			new[] { A, H, J, M },
			new[] { A, E, K, M },
			new[] { A, F, K, M },
			new[] { A, G, K, M },
			new[] { B, E, K, M },
			new[] { B, F, K, M },
			new[] { B, G, K, M },
			new[] { 4000L, 4000L, 4000L, O },
			new[] { A, F, I, N },
			new[] { A, F, J, N },
			new[] { A, F, K, N },
			new[] { B, F, I, N },
			new[] { B, F, J, N },
			new[] { B, F, K, N },
			new[] { C, F, I, N },
			new[] { C, F, J, N },
			new[] { C, F, K, N },
			new[] { D, F, I, N },
			new[] { D, F, J, N },
			new[] { D, F, K, N },
			new[] { D, E, I, L },
			new[] { D, E, I, M },
			new[] { D, E, J, L },
			new[] { D, E, J, M },
			new[] { D, F, I, L },
			new[] { D, F, I, M },
			new[] { D, F, J, L },
			new[] { D, F, J, M },
			new[] { D, G, I, L },
			new[] { D, G, I, M },
			new[] { D, G, J, L },
			new[] { D, G, J, M },
			new[] { D, H, I, L },
			new[] { D, H, I, M },
			new[] { D, H, J, L },
			new[] { D, H, J, M },
			new[] { A, E, I, N },
			new[] { B, E, I, N },
			new[] { C, E, I, N },
			new[] { D, E, I, N },
		};

		var sum = sets.Select(set => set[0] * set[1] * set[2] * set[3]).Sum();

		return sum;
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

	private class WorkflowDescription
	{
		public WorkflowDescription(string description)
		{
			Name = RegexHelper.GetFirstWord(description);

			var rules = description.Split('{')[^1].Without("}").Split(',');

			Rules = rules
				.Select(rule => rule.Split(':'))
				.Select(splitRule => splitRule.Length == 1
					? new WorkflowDescriptionRule(string.Empty, splitRule[0])
					: new WorkflowDescriptionRule(splitRule[0], splitRule[1]))
				.ToList();
		}
		
		public string Name { get; }
		public List<WorkflowDescriptionRule> Rules { get; }

		public string Description => Name + "{" + string.Join(",", Rules) + "}";
	}

	private class WorkflowDescriptionRule
	{
		public WorkflowDescriptionRule(string condition, string target)
		{
			Condition = condition;
			Target = target;
		}

		public string Condition { get; }
		public string Target { get; set; }

		public override string ToString() => string.IsNullOrEmpty(Condition) ? Target : Condition + ":" + Target;
	}

	private class PartRating
	{
		public PartRating(params int[] ratings)
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

	private class Batch
	{
		public Batch(int startingNumber, int count)
		{
			StartingNumber = startingNumber;
			Count = count;
		}

		public int StartingNumber { get; }
		public int Count { get; }
	}
}