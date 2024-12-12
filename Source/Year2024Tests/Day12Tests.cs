using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2024;

namespace Year2024Tests;

[TestClass]
public class Day12Tests : TestsBase
{
	protected override string DirectoryName => "Day12";
	protected override string Part1ExampleSolution => "1930";
	protected override string Part1Solution => "1377008";
	protected override string Part2ExampleSolution => "1206";
	protected override string Part2Solution => string.Empty;

	public Day12Tests() => PuzzleSolver = new Day12Solver();
    
	[TestMethod]
	public void Day12Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day12Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day12Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day12Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}