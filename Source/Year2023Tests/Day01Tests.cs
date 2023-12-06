using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day01Tests : TestsBase
{
	protected override string DirectoryName => "Day01";
	protected override string Part1Solution => "56397";
	protected override string Part2Solution => "55701";
	protected override string Part1ExampleSolution => "142";
	protected override string Part2ExampleSolution => "281";

	public Day01Tests() => PuzzleSolver = new Day01Solver();

	[TestMethod]
	public void Day01Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day01Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day01Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day01Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}
