using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day10Tests : TestsBase
{
    protected override string DirectoryName => "Day10";
    protected override string Part1Solution => "";
    protected override string Part2Solution => "";
    protected override string Part1ExampleSolution => "";
    protected override string Part2ExampleSolution => "";

    public Day10Tests()
    {
        PuzzleSolver = new Day10Solver();
    }
    
    [TestMethod]
    public void Day10IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day10IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}