using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day12Tests : TestsBase
{
    protected override string DirectoryName => "Day12";
    protected override string Part1Solution => "5254";
    protected override string Part2Solution => string.Empty; // Not yet solved
    protected override string Part1ExampleSolution => "10"; // Medium sized example: 19; Part 2 example: 226
    protected override string Part2ExampleSolution => "3509"; // Medium sized example: 103; Part 1 example: 36

    protected override bool SkipVerificationOfPart2 => true;

    public Day12Tests()
    {
        PuzzleSolver = new Day12Solver();
    }
    
    [TestMethod]
    public void Day12IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day12IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}