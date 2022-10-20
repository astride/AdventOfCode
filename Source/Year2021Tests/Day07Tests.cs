using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day07Tests : TestsBase
{
    protected override string DirectoryName => "Day07";
    protected override string Part1Solution => "335330";
    protected override string Part2Solution => "92439766";
    protected override string Part1ExampleSolution => "37";
    protected override string Part2ExampleSolution => "168";

    public Day07Tests()
    {
        PuzzleSolver = new Day07Solver();
    }
    
    [TestMethod]
    public void Day07IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day07IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}