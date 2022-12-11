using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day18Tests : TestsBase
{
    protected override string DirectoryName => "Day18";
    protected override string Part1ExampleSolution => "";
    protected override string Part1Solution => "";
    protected override string Part2ExampleSolution => "";
    protected override string Part2Solution => "";

    public Day18Tests()
    {
        PuzzleSolver = new Day18Solver();
    }
    
    [TestMethod]
    public void Day18IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day18IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}