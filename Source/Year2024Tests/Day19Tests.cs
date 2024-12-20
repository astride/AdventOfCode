using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2024;

namespace Year2024Tests;

[TestClass]
public class Day19Tests : TestsBase
{
	protected override string DirectoryName => "Day19";
	protected override string Part1ExampleSolution => "6";
	protected override string Part1Solution => "283";
	protected override string Part2ExampleSolution => string.Empty;
	protected override string Part2Solution => string.Empty;

	public Day19Tests() => PuzzleSolver = new Day19Solver();
    
	[TestMethod]
	public void Day19Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day19Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day19Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day19Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}