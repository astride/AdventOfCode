using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2024;

namespace Year2024Tests;

[TestClass]
public class Day17Tests : TestsBase
{
	protected override string DirectoryName => "Day17";
	protected override string Part1ExampleSolution => "4,6,3,5,6,3,5,2,1,0";
	protected override string Part1Solution => string.Empty;
	protected override string Part2ExampleSolution => string.Empty;
	protected override string Part2Solution => string.Empty;

	public Day17Tests() => PuzzleSolver = new Day17Solver();
    
	[TestMethod]
	public void Day17Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day17Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day17Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day17Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}