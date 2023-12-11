using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day11Tests : TestsBase
{
	protected override string DirectoryName => "Day11";
	protected override string Part1ExampleSolution => "374";
	protected override string Part1Solution => "9795148";
	protected override string Part2ExampleSolution => "82000210";
	protected override string Part2Solution => "650672493820";

	public Day11Tests() => PuzzleSolver = new Day11Solver();
    
	[TestMethod]
	public void Day11Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day11Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day11Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day11Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}