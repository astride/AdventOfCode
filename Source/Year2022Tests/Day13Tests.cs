using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day13Tests : TestsBase
{
    protected override string DirectoryName => "Day13";
    protected override string Part1ExampleSolution => "";
    protected override string Part1Solution => "";
    protected override string Part2ExampleSolution => "";
    protected override string Part2Solution => "";

    public Day13Tests()
    {
        PuzzleSolver = new Day13Solver();
    }
    
    [TestMethod]
    public void Day13IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day13IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}