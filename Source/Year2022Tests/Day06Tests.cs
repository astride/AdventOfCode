using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day06Tests : TestsBase
{
    protected override string DirectoryName => "Day06";
    protected override string Part1Solution => "1760";
    protected override string Part2Solution => "2974";
    protected override string Part1ExampleSolution => "7";
    protected override string Part2ExampleSolution => "19";

    public Day06Tests()
    {
        PuzzleSolver = new Day06Solver();
    }
    
    [TestMethod]
    public void Day06IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day06IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}