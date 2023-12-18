using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day18Tests : TestsBase
{
	protected override string DirectoryName => "Day18";
	protected override string Part1ExampleSolution => "62";
	protected override string Part1Solution => "47139";
	protected override string Part2ExampleSolution => string.Empty;
	protected override string Part2Solution => string.Empty;

	public Day18Tests() => PuzzleSolver = new Day18Solver();
    
	[TestMethod]
	public void Day18Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day18Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day18Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day18Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}