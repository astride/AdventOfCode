using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day19Tests : TestsBase
{
	protected override string DirectoryName => "Day19";
	protected override string Part1ExampleSolution => "19114";
	protected override string Part1Solution => "420739";
	protected override string Part2ExampleSolution => "167409079868000";
	protected override string Part2Solution => "130251901420382";

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