using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day15Tests : TestsBase
{
    protected override string DirectoryName => "Day15";
    protected override string Part1ExampleSolution => "26";
    protected override string Part1Solution => "5125700";
    protected override string Part2ExampleSolution => "56000011";
    protected override string Part2Solution => "";

    public Day15Tests()
    {
        PuzzleSolver = new Day15Solver();
    }
    
    [TestMethod]
    public void Day15IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day15IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}