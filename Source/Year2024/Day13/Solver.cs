using Common.Interfaces;

namespace Year2024;

public class Day13Solver : IPuzzleSolver
{
	public string Title => "Claw Contraption";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput) => GetSolution(input);

	public object GetPart2Solution(string[] input, bool isExampleInput) => GetSolution(input, shiftedTarget: true);

	private object GetSolution(string[] input, bool shiftedTarget = false)
	{
		var machineConfigs = input
			.Where(line => !string.IsNullOrEmpty(line))
			.Chunk(3)
			.Select(config => new MachineConfig(config[0], config[1], config[2]))
			.ToList();

		foreach (var config in machineConfigs)
		{
			config.CalculateClawMovement(shiftedTarget);
		}
		
		var spentTokens = machineConfigs.Select(CalculateTokenUsage).Sum();

		return spentTokens;
	}

	private static long CalculateTokenUsage(MachineConfig machineConfig)
	{
		return machineConfig.PrizeCanBeWon
			? 3 * machineConfig.A + machineConfig.B
			: 0;
	}
}

public class MachineConfig
{
	private const long Shift = 10000000000000;

	private readonly long _x;
	private readonly long _y;
	
	private readonly long _a;
	private readonly long _b;
	private readonly long _c;
	private readonly long _d;
	
	private long ShiftedDividendA => Shift + _x - _c * B;
	private long DividendA => _x - _c * B;
	private long DivisorA => _a;

	private long ShiftedDividendB => (_a - _b) * Shift + _a * _y - _b * _x;
	private long DividendB => _a * _y - _b * _x;
	private long DivisorB => _a * _d - _b * _c;

	public long A { get; set; }
	public long B { get; set; }

	public bool PrizeCanBeWon { get; set; }
	
	public MachineConfig(string buttonA, string buttonB, string prizeLocation)
	{
		// Aa + Bb = X
		// Ca + Db = Y
	
		var buttonAConfig = GetConfig(buttonA);
		var buttonBConfig = GetConfig(buttonB);

		_a = GetButtonValue(buttonAConfig[0]);
		_b = GetButtonValue(buttonAConfig[1]);
		_c = GetButtonValue(buttonBConfig[0]);
		_d = GetButtonValue(buttonBConfig[1]);

		var prizeConfig = GetConfig(prizeLocation);

		_x = GetPrizeLocation(prizeConfig[0]);
		_y = GetPrizeLocation(prizeConfig[1]);
	}

	private static string[] GetConfig(string line) => line.Split(',');
	private static long GetButtonValue(string buttonConfig) => long.Parse(buttonConfig.Split('+')[1]);
	private static long GetPrizeLocation(string prizeConfig) => long.Parse(prizeConfig.Split('=')[1]);

	public void CalculateClawMovement(bool shifted = false)
	{
		if (shifted)
		{
			CalculateClawMovement(() => ShiftedDividendA, () => ShiftedDividendB);
		}
		else
		{
			CalculateClawMovement(() => DividendA, () => DividendB);
		}
	}

	private void CalculateClawMovement(Func<long> dividendA, Func<long> dividendB)
	{
		B = Math.DivRem(dividendB(), DivisorB, out var remainderB);

		PrizeCanBeWon = remainderB is 0;

		if (PrizeCanBeWon)
		{
			A = Math.DivRem(dividendA(), DivisorA, out var remainderA);

			PrizeCanBeWon = remainderA is 0;
		}
	}
}
