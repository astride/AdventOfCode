using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2024;

namespace Year2024Tests;

[TestClass]
public class Day02Tests : TestsBase
{
	protected override string DirectoryName => "Day02";
	protected override string Part1ExampleSolution => "2";
	protected override string Part1Solution => "472";
	protected override string Part2ExampleSolution => "4";
	protected override string Part2Solution => "520";

	public Day02Tests() => PuzzleSolver = new Day02Solver();
    
	[TestMethod]
	public void Day02Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day02Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day02Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day02Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}