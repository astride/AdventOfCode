using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day03Tests : TestsBase
{
	protected override string DirectoryName => "Day03";
	protected override string Part1ExampleSolution => string.Empty;
	protected override string Part1Solution => string.Empty;
	protected override string Part2ExampleSolution => string.Empty;
	protected override string Part2Solution => string.Empty;

	public Day03Tests() => PuzzleSolver = new Day03Solver();
    
	[TestMethod]
	public void Day03IsValid() => HasCorrectSolutions();

	[TestMethod]
	public void Day03IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}