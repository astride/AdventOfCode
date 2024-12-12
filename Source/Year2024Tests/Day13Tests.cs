using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2024;

namespace Year2024Tests;

[TestClass]
public class Day13Tests : TestsBase
{
	protected override string DirectoryName => "Day13";
	protected override string Part1ExampleSolution => string.Empty;
	protected override string Part1Solution => string.Empty;
	protected override string Part2ExampleSolution => string.Empty;
	protected override string Part2Solution => string.Empty;

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