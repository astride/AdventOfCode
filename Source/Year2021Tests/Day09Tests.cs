using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day09Tests : TestsBase
{
    protected override string DirectoryName => "Day09";
    protected override string Part1Solution => "489";
    protected override string Part2Solution => "1056330";
    protected override string Part1ExampleSolution => "15";
    protected override string Part2ExampleSolution => "1134";

    public Day09Tests()
    {
        PuzzleSolver = new Day09Solver();
    }
    
    [TestMethod]
    public void Day09IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day09IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}