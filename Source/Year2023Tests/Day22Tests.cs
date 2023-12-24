using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day22Tests : TestsBase
{
	protected override string DirectoryName => "Day22";
	protected override string Part1ExampleSolution => "5";
	protected override string Part1Solution => "401";
	protected override string Part2ExampleSolution => "7";
	protected override string Part2Solution => "63491";

	public Day22Tests() => PuzzleSolver = new Day22Solver();
    
	[TestMethod]
	public void Day22Part1_IsValid() => VerifyPart1();
	
	[TestMethod]
	public void Day22Part2_IsValid() => VerifyPart2();

	[TestMethod]
	public void Day22Part1_IsValid_WithExampleInput() => VerifyPart1WithExampleInput();
	
	[TestMethod]
	public void Day22Part2_IsValid_WithExampleInput() => VerifyPart2WithExampleInput();
}