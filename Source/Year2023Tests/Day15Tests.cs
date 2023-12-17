using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day15Tests : TestsBase
{
	protected override string DirectoryName => "Day15";
	protected override string Part1ExampleSolution => "1320";
	protected override string Part1Solution => "514639";
	protected override string Part2ExampleSolution => "145";
	protected override string Part2Solution => "279470";

	public Day15Tests() => PuzzleSolver = new Day15Solver();
    
	[TestMethod]
	public void Day15Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day15Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day15Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day15Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}