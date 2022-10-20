using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day18Tests : TestsBase
{
    protected override string DirectoryName => "Day18";
    protected override string Part1Solution => "3647";
    protected override string Part2Solution => "4600";
    protected override string Part1ExampleSolution => "4140";
    protected override string Part2ExampleSolution => "3993";

    public Day18Tests()
    {
        PuzzleSolver = new Day18Solver();
    }
    
    [TestMethod]
    public void Day18IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day18IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}