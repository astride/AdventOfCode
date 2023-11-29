using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day00Tests : TestsBase
{
	protected override string DirectoryName => "Day00";
	protected override string Part1ExampleSolution => string.Empty;
	protected override string Part1Solution => string.Empty;
	protected override string Part2ExampleSolution => string.Empty;
	protected override string Part2Solution => string.Empty;

	public Day00Tests() => PuzzleSolver = new Day00Solver();
    
	// [TestMethod]
	public void Day00IsValid() => HasCorrectSolutions();

	// [TestMethod]
	public void Day00IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}