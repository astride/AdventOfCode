using Common.Interfaces;

namespace Year2024;

public class Day13Solver : IPuzzleSolver
{
	public string Title => "Claw Contraption";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var machineConfigs = input
			.Where(line => !string.IsNullOrEmpty(line))
			.Chunk(3)
			.Select(config => new MachineConfig(config[0], config[1], config[2]))
			.ToList();

		foreach (var config in machineConfigs)
		{
			config.CalculateClawMovement();
		}
		
		var spentTokens = machineConfigs.Select(CalculateTokenUsage).Sum();

		return spentTokens;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}

	private decimal CalculateTokenUsage(MachineConfig machineConfig)
	{
		return machineConfig.PrizeCanBeWon
			? 3 * machineConfig.A + machineConfig.B
			: 0;
	}
}

public class MachineConfig
{
	private readonly long _x;
	private readonly long _y;
	
	private readonly long _a;
	private readonly long _b;
	private readonly long _c;
	private readonly long _d;

	public long A { get; set; }
	public long B { get; set; }

	public bool PrizeCanBeWon { get; set; }
	
	public MachineConfig(string buttonA, string buttonB, string prizeLocation)
	{
		// Aa + Bb = X
		// Ca + Db = Y
	
		var buttonAConfig = buttonA.Split(',');
		var buttonBConfig = buttonB.Split(',');

		_a = long.Parse(buttonAConfig[0].Split('+')[1]);
		_b = long.Parse(buttonAConfig[1].Split('+')[1]);
		_c = long.Parse(buttonBConfig[0].Split('+')[1]);
		_d = long.Parse(buttonBConfig[1].Split('+')[1]);

		var prizeLocationConfig = prizeLocation.Split(',');

		_x = long.Parse(prizeLocationConfig[0].Split('=')[1]);
		_y = long.Parse(prizeLocationConfig[1].Split('=')[1]);
	}

	public void CalculateClawMovement()
	{
		// b = (AY - BX) / (AD - BC)
		// a = (X - Cb) / A

		B = Math.DivRem(_a * _y - _b * _x, _a * _d - _b * _c, out var bRemainder);

		PrizeCanBeWon = bRemainder is 0;

		if (PrizeCanBeWon)
		{
			A = Math.DivRem(_x - _c * B, _a, out var aRemainder);

			PrizeCanBeWon = aRemainder is 0;
		}
	}
}
