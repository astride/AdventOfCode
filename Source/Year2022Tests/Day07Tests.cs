using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day07Tests : TestsBase
{
    protected override string DirectoryName => "Day07";
    protected override string Part1Solution => "";
    protected override string Part2Solution => "";
    protected override string Part1ExampleSolution => "95437";
    protected override string Part2ExampleSolution => "";

    public Day07Tests()
    {
        PuzzleSolver = new Day07Solver();
    }
    
    [TestMethod]
    public void Day07IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day07IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}