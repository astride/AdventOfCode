using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day06Tests : TestsBase
{
	protected override string DirectoryName => "Day06";
	protected override string Part1ExampleSolution => "288";
	protected override string Part1Solution => "2449062";
	protected override string Part2ExampleSolution => "71503";
	protected override string Part2Solution => "33149631";

	public Day06Tests() => PuzzleSolver = new Day06Solver();
    
	[TestMethod]
	public void Day06IsValid() => HasCorrectSolutions();

	[TestMethod]
	public void Day06IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}