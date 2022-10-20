using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day06Tests : TestsBase
{
    protected override string DirectoryName => "Day06";
    protected override string Part1Solution => "372984";
    protected override string Part2Solution => "1681503251694";
    protected override string Part1ExampleSolution => "5934";
    protected override string Part2ExampleSolution => "26984457539";

    public Day06Tests()
    {
        PuzzleSolver = new Day06Solver();
    }
    
    [TestMethod]
    public void Day06IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day06IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}