using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day00Tests : TestsBase
{
    protected override string DirectoryName => "Day00";
    protected override string Part1Solution => "";
    protected override string Part2Solution => "";
    protected override string Part1ExampleSolution => "";
    protected override string Part2ExampleSolution => "";

    public Day00Tests()
    {
        PuzzleSolver = new Day00Solver();
    }
    
    [TestMethod]
    public void Day00IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day00IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}