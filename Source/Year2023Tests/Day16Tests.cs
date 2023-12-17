using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day16Tests : TestsBase
{
	protected override string DirectoryName => "Day16";
	protected override string Part1ExampleSolution => "46";
	protected override string Part1Solution => "8125";
	protected override string Part2ExampleSolution => "51";
	protected override string Part2Solution => "8489";

	public Day16Tests() => PuzzleSolver = new Day16Solver();
    
	[TestMethod]
	public void Day16Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day16Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day16Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day16Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}