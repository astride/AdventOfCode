using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day01Tests : TestsBase
{
	protected override string DirectoryName => "Day01";
	protected override string Part1Solution => "56397";
	protected override string Part2Solution => "55701";
	protected override string Part1ExampleSolution => "142";
	protected override string Part2ExampleSolution => "281";

	public Day01Tests() => PuzzleSolver = new Day01Solver();

	[TestMethod]
	public void Day01IsValid() => HasCorrectSolutions();

	[TestMethod]
	public void Day01IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}
