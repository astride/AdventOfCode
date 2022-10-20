using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day05Tests : TestsBase
{
    protected override string DirectoryName => "Day05";
    protected override string Part1Solution => "4826";
    protected override string Part2Solution => "16793";
    protected override string Part1ExampleSolution => "5";
    protected override string Part2ExampleSolution => "12";

    public Day05Tests()
    {
        PuzzleSolver = new Day05Solver();
    }
    
    [TestMethod]
    public void Day05IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day05IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}