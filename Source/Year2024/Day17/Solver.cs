using Common.Interfaces;

namespace Year2024;

public class Day17Solver : IPuzzleSolver
{
	public string Title => "Chronospatial Computer";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		// 7,5,7,7,7,7,7,7,5 <-- Not the right answer
		var register = input
			.TakeWhile(line => !string.IsNullOrEmpty(line))
			.Select(line => line.Split(':', StringSplitOptions.TrimEntries)[1])
			.Select(long.Parse)
			.ToArray();

		var instructions = input
			.TakeLast(1).Single()
			.Split(':', StringSplitOptions.TrimEntries)[1]
			.Split(',')
			.Select(long.Parse)
			.ToList();
		
		Console.WriteLine("Register: " + string.Join(", ", register));
		Console.WriteLine("Instructions: " + string.Join(", ", instructions));

		var output = RunProgram();

		return string.Join(",", output);

		List<long> RunProgram()
		{
			var output = new List<long>();

			var instructionPointer = 0;

			while (instructionPointer < instructions.Count - 1)
			{
				var opcode = instructions[instructionPointer];
				var operand = instructions[instructionPointer + 1];
				
				Console.WriteLine();
				Console.WriteLine("----------------------------------------");
				Console.WriteLine();
				Console.WriteLine("Instruction pointer: " + instructionPointer);
				Console.WriteLine("opcode: " + opcode);
				Console.WriteLine("operand: " + operand);

				instructionPointer = opcode is 3 && register[0] is not 0
					? (int)operand
					: instructionPointer + 2;

				Action action = opcode switch
				{
					0 => () => Adv(operand),
					1 => () => Bxl(operand),
					2 => () => Bst(operand),
					3 => () => Jnz(instructionPointer),
					4 => Bxc,
					5 => () => Out(output, operand),
					6 => () => Bdv(operand),
					7 => () => Cdv(operand),
					_ => throw new ArgumentException("Invalid opcode!")
				};

				action();
				
				Console.WriteLine($"Register values: A = {register[0]} | B = {register[1]} | C = {register[2]}");
			}

			return output;
		}

		void Out(List<long> output, long operand)
		{
			output.Add(GetComboOperand(operand) % 8);
			
			Console.WriteLine("opcode is 5 --> output " + output[^1]);
		}

		void Adv(long operand) => register[0] = Divide(operand);

		void Bxl(long operand) => register[1] |= operand;

		void Jnz(int instructionPointer) => Console.WriteLine("opcode is 3 --> instructionPointer was set to " + instructionPointer);

		void Bst(long operand) => register[1] = GetComboOperand(operand) % 8;

		void Bxc() => register[1] |= register[2];

		void Bdv(long operand) => register[1] = Divide(operand);

		void Cdv(long operand) => register[2] = Divide(operand);

		long Divide(long operand)
		{
			var numerator = register[0];
			var denominator = Math.Pow(2, GetComboOperand(operand));
			
			return (long)Math.Floor(numerator / denominator);
		}

		long GetComboOperand(long operand)
		{
			return operand switch
			{
				< 4 => operand,
				< 7 => register[operand - 4],
				_ => throw new ArgumentException("Invalid combo operand!"),
			};
		}
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}
}