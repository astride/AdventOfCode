using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day09Tests : TestsBase
{
	protected override string DirectoryName => "Day09";
	protected override string Part1ExampleSolution => "114";
	protected override string Part1Solution => "1702218515";
	protected override string Part2ExampleSolution => string.Empty;
	protected override string Part2Solution => string.Empty;

	public Day09Tests() => PuzzleSolver = new Day09Solver();
    
	[TestMethod]
	public void Day09Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day09Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day09Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day09Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}