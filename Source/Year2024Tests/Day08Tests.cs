using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2024;

namespace Year2024Tests;

[TestClass]
public class Day08Tests : TestsBase
{
	protected override string DirectoryName => "Day08";
	protected override string Part1ExampleSolution => "14";
	protected override string Part1Solution => "222";
	protected override string Part2ExampleSolution => "34";
	protected override string Part2Solution => "884";

	public Day08Tests() => PuzzleSolver = new Day08Solver();
    
	[TestMethod]
	public void Day08Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day08Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day08Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day08Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}