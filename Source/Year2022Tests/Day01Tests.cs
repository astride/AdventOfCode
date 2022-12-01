using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day01Tests : TestsBase
{
    protected override string DirectoryName => "Day01";
    protected override string Part1Solution => "66186";
    protected override string Part2Solution => "196804";
    protected override string Part1ExampleSolution => "24000";
    protected override string Part2ExampleSolution => "45000";

    public Day01Tests()
    {
        PuzzleSolver = new Day01Solver();
    }
    
    [TestMethod]
    public void Day01IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day01IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}