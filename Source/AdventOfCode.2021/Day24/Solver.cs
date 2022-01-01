using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day24Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var instructions = new List<MonadStep>();
			List<string> monadStepInput;

			while (rawInput.Any())
			{
				monadStepInput = rawInput
					.Skip(1)
					.TakeWhile(entry => !entry.Contains("inp"))
					.ToList();

				monadStepInput.Add(rawInput.First());

				instructions.Add(new MonadStep(monadStepInput.ToArray()));

				rawInput = rawInput
					.Skip(1 + instructions.Last().Operations.Count())
					.ToArray();
			}

			Part1Solution = SolvePart1(instructions).ToString();
		}

		private static string SolvePart1(List<MonadStep> instructions)
		{
			return instructions.GetLargestAcceptedModelNumber();
		}
	}

	public static class Day24Helpers
	{
		private const int InputValueMin = 1;
		private const int InputValueMax = 9;
		private const int InputPossibilities = InputValueMax - InputValueMin + 1;
		private const int ModelNumberLength = 14;

		private static IDictionary<int, List<int>> ResultingZValuePerDigitIndexForInputValue =
			Enumerable.Range(InputValueMin, InputPossibilities)
				.ToDictionary(
					value => value,
					value => new List<int>());

		public static string GetLargestAcceptedModelNumber(this List<MonadStep> instructions)
		{
			int z = 0;

			// Run all possible variations once and store in dictionary
			foreach (var modelNumberDigitIndex in Enumerable.Range(0, ModelNumberLength - 1))
			{
				foreach (var possibleInputDigit in Enumerable.Range(InputValueMin, InputPossibilities))
				{
					z = instructions[modelNumberDigitIndex].CalculateZValueFor(possibleInputDigit);

					ResultingZValuePerDigitIndexForInputValue[possibleInputDigit].Add(z);
				}
			}

			// Work from there

			// Observations (part 1 - real input):
			// w is independent between digit places (input value / digit of model number to be tested)
			// x is independent between digit places (always set to zero before usage)
			// x is independent between digit places (always set to zero before usage)
			// z is DEPENDENT between digit places

			//TODO
			return null;
		}
	}

	public class MonadStep
	{
		private const string Input = "inp";
		private const string Addition = "add";
		private const string Multiplication = "mul";
		private const string Division = "div";
		private const string Modulo = "mod";
		private const string EqualTo = "eql";

		public MonadStep(string[] input)
		{
			InputVariable = input
				.Single(entry => entry.Contains(Input))
				.Last();

			Operations = input
				.Where(entry => !entry.Contains(Input))
				.Select(entry => GetOperation(entry))
				.ToList();
		}

		private Operation GetOperation(string operationString)
		{
			var parts = operationString.Split(' ');

			return parts[0] switch
			{
				Addition => new Addition(parts[1], parts[2]),
				Multiplication => new Multiplication(parts[1], parts[2]),
				Division => new Division(parts[1], parts[2]),
				Modulo => new Modulo(parts[1], parts[2]),
				EqualTo => new EqualTo(parts[1], parts[2]),
				_ => null
			};
		}

		public int CalculateZValueFor(int digit)
		{
			//TODO Switch on InputVariable to know where to store calculation
			return -1;
		}

		public char InputVariable { get; set; }

		public List<Operation> Operations { get; set; }
	}

	public class Operation
	{
		private const string W = "w";
		private const string X = "x";
		private const string Y = "y";
		private const string Z = "z";

		public Operation(string var1, string var2)
		{
			Variable1 = var1;
			Variable2 = var2;
		}

		public string Variable1 { get; set; }
		protected string Variable2 { get; set; }
		protected int Operator1 { get; set; }
		protected int Operator2 { get; set; }

		protected void PrepareOperators(int w, int x, int y, int z)
		{
			Operator1 = Variable1 switch
			{
				W => w,
				X => x,
				Y => y,
				Z => z,
				_ => 0
			};

			Operator2 = Variable2 switch
			{
				W => w,
				X => x,
				Y => y,
				Z => z,
				_ => int.Parse(Variable2)
			};
		}

		public virtual int Calculate(int w, int x, int y, int z) => 0; // ?
	}

	public class Addition : Operation
	{
		public Addition(string var1, string var2) : base(var1, var2) { }

		public override int Calculate(int w, int x, int y, int z)
		{
			PrepareOperators(w, x, y, z);

			return Operator1 + Operator2;
		}
	}

	public class Multiplication : Operation
	{
		public Multiplication(string var1, string var2) : base(var1, var2) { }

		public override int Calculate(int w, int x, int y, int z)
		{
			PrepareOperators(w, x, y, z);

			return Operator1 * Operator2;
		}
	}

	public class Division : Operation
	{
		public Division(string var1, string var2) : base(var1, var2) { }

		public override int Calculate(int w, int x, int y, int z)
		{
			PrepareOperators(w, x, y, z);

			return (int)Math.Floor((double)Operator1 / Operator2);
		}
	}

	public class Modulo : Operation
	{
		public Modulo(string var1, string var2) : base(var1, var2) { }

		public override int Calculate(int w, int x, int y, int z)
		{
			PrepareOperators(w, x, y, z);

			return Operator1 % Operator2;
		}
	}

	public class EqualTo : Operation
	{
		public EqualTo(string var1, string var2) : base(var1, var2) { }

		public override int Calculate(int w, int x, int y, int z)
		{
			PrepareOperators(w, x, y, z);

			return Operator1 == Operator2 ? 1 : 0;
		}
	}
}
