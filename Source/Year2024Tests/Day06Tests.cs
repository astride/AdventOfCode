using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2024;

namespace Year2024Tests;

[TestClass]
public class Day06Tests : TestsBase
{
	protected override string DirectoryName => "Day06";
	protected override string Part1ExampleSolution => "41";
	protected override string Part1Solution => "5067";
	protected override string Part2ExampleSolution => "6";
	protected override string Part2Solution => "";

	public Day06Tests() => PuzzleSolver = new Day06Solver();
    
	[TestMethod]
	public void Day06Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day06Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day06Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day06Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}