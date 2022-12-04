using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day04Tests : TestsBase
{
    protected override string DirectoryName => "Day04";
    protected override string Part1Solution => "567";
    protected override string Part2Solution => "907";
    protected override string Part1ExampleSolution => "2";
    protected override string Part2ExampleSolution => "4";

    public Day04Tests()
    {
        PuzzleSolver = new Day04Solver();
    }
    
    [TestMethod]
    public void Day04IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day04IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}