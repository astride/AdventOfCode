using Common.Interfaces;

namespace Year2024;

public class Day03Solver : IPuzzleSolver
{
	public string Title => "Mull It Over";
	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	private const int MaxDigits = 3;

	private const string EnableCommand = "do()";
	private const string DisableCommand = "don't()";

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		return GetProductOfValidMultiplications(input).Sum();
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return GetProductOfValidMultiplications(input, skipDisabledInstructions: true).Sum();
	}

	private static IEnumerable<int> GetProductOfValidMultiplications(string[] memory, bool skipDisabledInstructions = false)
	{
		bool gotM;
		bool gotU;
		bool gotL;
		bool gotParenthesisOpen;
		bool gotFirstNumber;
		bool gotComma;
		bool gotSecondNumber;

		string firstNumber;
		string secondNumber;
		
		EmptyValues();

		var enabled = true;

		var instructions = string.Join(string.Empty, memory);

		for (var i = 0; i < instructions.Length; i++)
		{
			if (skipDisabledInstructions)
			{
				if (!enabled)
				{
					if (ReachedEnableCommand(i))
					{
						enabled = true;
						i += EnableCommand.Length;
					}
					
					continue;
				}

				if (ReachedDisableCommand(i))
				{
					enabled = false;
					i += DisableCommand.Length;

					EmptyValues();
					continue;
				}
			}
			
			var ch = instructions[i];

			if (gotSecondNumber && ch == ')')
			{
				var product = int.Parse(firstNumber) * int.Parse(secondNumber);
				Console.WriteLine($"FOUND mul({firstNumber},{secondNumber}) = {product}");

				yield return product;
				
				EmptyValues();
				continue;
			}

			var isDigit = int.TryParse(new[] { ch }, out _);

			if (gotComma)
			{
				if (!isDigit || secondNumber.Length == MaxDigits)
				{
					EmptyValues();
					continue;
				}

				secondNumber += ch;
				gotSecondNumber = true;
				continue;
			}

			if (gotFirstNumber && ch == ',')
			{
				gotComma = true;
				continue;
			}

			if (gotParenthesisOpen)
			{
				if (!isDigit || firstNumber.Length == MaxDigits)
				{
					EmptyValues();
					continue;
				}

				firstNumber += ch;
				gotFirstNumber = true;
				continue;
			}

			if (gotL && ch == '(')
			{
				gotParenthesisOpen = true;
				continue;
			}
			
			if (gotU && ch == 'l')
			{
				gotL = true;
				continue;
			}

			if (gotM && ch == 'u')
			{
				gotU = true;
				continue;
			}

			if (ch == 'm')
			{
				gotM = true;
				continue;
			}
			
			EmptyValues();
		}

		yield break;

		void EmptyValues()
		{
			gotM = false;
			gotU = false;
			gotL = false;
			gotParenthesisOpen = false;
			gotFirstNumber = false;
			gotComma = false;
			gotSecondNumber = false;

			firstNumber = string.Empty;
			secondNumber = string.Empty;
		}

		bool ReachedDisableCommand(int i) => ReachedCommand(i, DisableCommand);
		bool ReachedEnableCommand(int i) => ReachedCommand(i, EnableCommand);

		bool ReachedCommand(int i, string command)
		{
			return i + command.Length <= instructions.Length &&
			       instructions[i..(i + command.Length)] == command;
		}
	}
}