using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day14Tests : TestsBase
{
	protected override string DirectoryName => "Day14";
	protected override string Part1ExampleSolution => "136";
	protected override string Part1Solution => "105003";
	protected override string Part2ExampleSolution => string.Empty;
	protected override string Part2Solution => string.Empty;

	public Day14Tests() => PuzzleSolver = new Day14Solver();
    
	[TestMethod]
	public void Day14Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day14Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day14Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day14Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}