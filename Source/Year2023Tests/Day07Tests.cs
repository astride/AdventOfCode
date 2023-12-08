using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day07Tests : TestsBase
{
	protected override string DirectoryName => "Day07";
	protected override string Part1ExampleSolution => "6440";
	protected override string Part1Solution => "251058093";
	protected override string Part2ExampleSolution => "5905";
	protected override string Part2Solution => "249781879";

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