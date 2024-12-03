using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2024;

namespace Year2024Tests;

[TestClass]
public class Day03Tests : TestsBase
{
	protected override string DirectoryName => "Day03";
	protected override string Part1ExampleSolution => "161";
	protected override string Part1Solution => "187833789";
	protected override string Part2ExampleSolution => "48";
	protected override string Part2Solution => "94455185";

	public Day03Tests() => PuzzleSolver = new Day03Solver();
    
	[TestMethod]
	public void Day03Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day03Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day03Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day03Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}