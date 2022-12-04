using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day11Tests : TestsBase
{
    protected override string DirectoryName => "Day11";
    protected override string Part1Solution => "";
    protected override string Part2Solution => "";
    protected override string Part1ExampleSolution => "";
    protected override string Part2ExampleSolution => "";

    public Day11Tests()
    {
        PuzzleSolver = new Day11Solver();
    }
    
    [TestMethod]
    public void Day11IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day11IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}