using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day15Tests : TestsBase
{
    protected override string DirectoryName => "Day15";
    protected override string Part1Solution => "553";
    protected override string Part2Solution => string.Empty; // Not yet solved
    protected override string Part1ExampleSolution => "40";
    protected override string Part2ExampleSolution => "315";

    protected override bool SkipVerificationOfPart2 => true;

    public Day15Tests()
    {
        PuzzleSolver = new Day15Solver();
    }
    
    [TestMethod]
    public void Day15IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day15IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}