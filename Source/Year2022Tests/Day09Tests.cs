using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day09Tests : TestsBase
{
    protected override string DirectoryName => "Day09";
    protected override string Part1Solution => "5513";
    protected override string Part2Solution => "2427";
    protected override string Part1ExampleSolution => "13";
    protected override string Part2ExampleSolution => "36";

    public Day09Tests()
    {
        PuzzleSolver = new Day09Solver();
    }
    
    [TestMethod]
    public void Day09IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day09IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}
