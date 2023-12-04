using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day04Tests : TestsBase
{
	protected override string DirectoryName => "Day04";
	protected override string Part1ExampleSolution => "13";
	protected override string Part1Solution => "25010";
	protected override string Part2ExampleSolution => string.Empty;
	protected override string Part2Solution => string.Empty;

	public Day04Tests() => PuzzleSolver = new Day04Solver();
    
	[TestMethod]
	public void Day04IsValid() => HasCorrectSolutions();

	[TestMethod]
	public void Day04IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}