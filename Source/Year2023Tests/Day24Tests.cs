using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day24Tests : TestsBase
{
	protected override string DirectoryName => "Day24";
	protected override string Part1ExampleSolution => "2";
	protected override string Part1Solution => string.Empty;
	protected override string Part2ExampleSolution => string.Empty;
	protected override string Part2Solution => string.Empty;

	public Day24Tests() => PuzzleSolver = new Day24Solver();
    
	[TestMethod]
	public void Day24Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day24Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day24Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day24Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}