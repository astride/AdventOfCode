using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day03Tests : TestsBase
{
    protected override string DirectoryName => "Day03";
    protected override string Part1Solution => "";
    protected override string Part2Solution => "";
    protected override string Part1ExampleSolution => "";
    protected override string Part2ExampleSolution => "";

    public Day03Tests()
    {
        PuzzleSolver = new Day03Solver();
    }
    
    [TestMethod]
    public void Day03IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day03IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}