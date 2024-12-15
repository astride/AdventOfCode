using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2024;

namespace Year2024Tests;

[TestClass]
public class Day15Tests : TestsBase
{
	protected override string DirectoryName => "Day15";
	protected override string Part1ExampleSolution => "10092";
	protected override string Part1Solution => "1495147";
	protected override string Part2ExampleSolution => string.Empty;
	protected override string Part2Solution => string.Empty;

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