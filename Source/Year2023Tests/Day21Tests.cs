using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day21Tests : TestsBase
{
	protected override string DirectoryName => "Day21";
	protected override string Part1ExampleSolution => "16"; // When using 6 steps
	protected override string Part1Solution => "3677";
	protected override string Part2ExampleSolution => string.Empty;
	protected override string Part2Solution => string.Empty;

	public Day21Tests() => PuzzleSolver = new Day21Solver();
    
	[TestMethod]
	public void Day21Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day21Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day21Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day21Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}