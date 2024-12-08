using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2024;

namespace Year2024Tests;

[TestClass]
public class Day07Tests : TestsBase
{
	protected override string DirectoryName => "Day07";
	protected override string Part1ExampleSolution => "3749";
	protected override string Part1Solution => string.Empty;
	protected override string Part2ExampleSolution => string.Empty;
	protected override string Part2Solution => string.Empty;

	public Day07Tests() => PuzzleSolver = new Day07Solver();
    
	[TestMethod]
	public void Day07Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day07Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day07Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day07Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}