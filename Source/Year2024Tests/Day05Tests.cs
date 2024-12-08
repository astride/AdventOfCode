using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2024;

namespace Year2024Tests;

[TestClass]
public class Day05Tests : TestsBase
{
	protected override string DirectoryName => "Day05";
	protected override string Part1ExampleSolution => "143";
	protected override string Part1Solution => "4996";
	protected override string Part2ExampleSolution => "123";
	protected override string Part2Solution => "6311";

	public Day05Tests() => PuzzleSolver = new Day05Solver();
    
	[TestMethod]
	public void Day05Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day05Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day05Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day05Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}