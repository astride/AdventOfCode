using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2024;

namespace Year2024Tests;

[TestClass]
public class Day13Tests : TestsBase
{
	protected override string DirectoryName => "Day13";
	protected override string Part1ExampleSolution => "480";
	protected override string Part1Solution => "35082";
	protected override string Part2ExampleSolution => "875318608908";
	protected override string Part2Solution => "82570698600470";

	public Day13Tests() => PuzzleSolver = new Day13Solver();
    
	[TestMethod]
	public void Day13Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day13Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day13Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day13Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}