using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day04Tests : TestsBase
{
	protected override string DirectoryName => "Day04";
	protected override string Part1ExampleSolution => "13";
	protected override string Part1Solution => "25010";
	protected override string Part2ExampleSolution => "30";
	protected override string Part2Solution => "9924412";

	public Day04Tests() => PuzzleSolver = new Day04Solver();
    
	[TestMethod]
	public void Day04Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day04Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day04Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day04Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}