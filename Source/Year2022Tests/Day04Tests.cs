using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day04Tests : TestsBase
{
    protected override string DirectoryName => "Day04";
    protected override string Part1Solution => "";
    protected override string Part2Solution => "";
    protected override string Part1ExampleSolution => "";
    protected override string Part2ExampleSolution => "";

    public Day04Tests()
    {
        PuzzleSolver = new Day04Solver();
    }
    
    [TestMethod]
    public void Day04IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day04IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}