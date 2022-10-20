using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day11Tests : TestsBase
{
    protected override string DirectoryName => "Day11";
    protected override string Part1Solution => "1675";
    protected override string Part2Solution => "515";
    protected override string Part1ExampleSolution => "1656";
    protected override string Part2ExampleSolution => "195";

    public Day11Tests()
    {
        PuzzleSolver = new Day11Solver();
    }
    
    [TestMethod]
    public void Day11IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day11IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}