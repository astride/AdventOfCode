using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day03Tests : TestsBase
{
    protected override string DirectoryName => "Day03";
    protected override string Part1Solution => "2954600";
    protected override string Part2Solution => "1662846";
    protected override string Part1ExampleSolution => "198";
    protected override string Part2ExampleSolution => "230";

    public Day03Tests()
    {
        PuzzleSolver = new Day03Solver();
    }
    
    [TestMethod]
    public void Day03IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day03IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}