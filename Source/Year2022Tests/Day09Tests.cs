using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day09Tests : TestsBase
{
    protected override string DirectoryName => "Day09";
    protected override string Part1Solution => "";
    protected override string Part2Solution => "";
    protected override string Part1ExampleSolution => "";
    protected override string Part2ExampleSolution => "";

    public Day09Tests()
    {
        PuzzleSolver = new Day09Solver();
    }
    
    [TestMethod]
    public void Day09IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day09IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}