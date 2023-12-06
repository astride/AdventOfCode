using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day05Tests : TestsBase
{
	protected override string DirectoryName => "Day05";
	protected override string Part1ExampleSolution => "35";
	protected override string Part1Solution => "525792406";
	protected override string Part2ExampleSolution => string.Empty;
	protected override string Part2Solution => string.Empty;

	public Day05Tests() => PuzzleSolver = new Day05Solver();
    
	[TestMethod]
	public void Day05IsValid() => HasCorrectSolutions();

	[TestMethod]
	public void Day05IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}
