using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day20Tests : TestsBase
{
    protected override string DirectoryName => "Day20";
    protected override string Part1Solution => "5884";
    protected override string Part2Solution => "19043";
    protected override string Part1ExampleSolution => "35";
    protected override string Part2ExampleSolution => "3351";

    public Day20Tests()
    {
        PuzzleSolver = new Day20Solver();
    }
    
    [TestMethod]
    public void Day20IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day20IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}