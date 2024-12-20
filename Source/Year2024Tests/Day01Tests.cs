using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2024;

namespace Year2024Tests;

[TestClass]
public class Day01Tests : TestsBase
{
	protected override string DirectoryName => "Day01";
	protected override string Part1ExampleSolution => "11";
	protected override string Part1Solution => "765748";
	protected override string Part2ExampleSolution => "31";
	protected override string Part2Solution => "27732508";

	public Day01Tests() => PuzzleSolver = new Day01Solver();
    
	[TestMethod]
	public void Day01Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day01Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day01Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day01Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}