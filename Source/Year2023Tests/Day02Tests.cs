using Microsoft.VisualStudio.TestTools.UnitTesting;
using Year2023;

namespace Year2023Tests;

[TestClass]
public class Day02Tests : TestsBase
{
	protected override string DirectoryName => "Day02";
	protected override string Part1ExampleSolution => "8";
	protected override string Part1Solution => "2528";
	protected override string Part2ExampleSolution => "2286";
	protected override string Part2Solution => "67363";

	public Day02Tests() => PuzzleSolver = new Day02Solver();
    
	[TestMethod]
	public void Day02IsValid() => HasCorrectSolutions();

	[TestMethod]
	public void Day02IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}