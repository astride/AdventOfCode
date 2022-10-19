using Year2020;

namespace Year2020Tests;

[TestClass]
public class Day01Tests : TestsBase
{
    protected override string DirectoryName => "Day01";
    protected override string Part1Solution => "538464";
    protected override string Part2Solution => "278783190";
    protected override string Part1ExampleSolution => "514579";
    protected override string Part2ExampleSolution => "241861950";

    public Day01Tests()
    {
        PuzzleSolver = new Day01Solver();
    }
    
    [TestMethod]
    public void Day01IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day01IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}
