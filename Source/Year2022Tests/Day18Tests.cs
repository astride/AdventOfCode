using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day18Tests : TestsBase
{
	protected override string DirectoryName => "Day18";
	protected override string Part1ExampleSolution => "64";
	protected override string Part1Solution => "4332";
	protected override string Part2ExampleSolution => "58";
	protected override string Part2Solution => "";

	public Day18Tests()
	{
		PuzzleSolver = new Day18Solver();
	}
    
	[TestMethod]
	public void Day18IsValid() => HasCorrectSolutions();

	[TestMethod]
	public void Day18IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}
