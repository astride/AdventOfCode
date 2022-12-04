using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day06Tests : TestsBase
{
    protected override string DirectoryName => "Day06";
    protected override string Part1Solution => "";
    protected override string Part2Solution => "";
    protected override string Part1ExampleSolution => "";
    protected override string Part2ExampleSolution => "";

    public Day06Tests()
    {
        PuzzleSolver = new Day06Solver();
    }
    
    [TestMethod]
    public void Day06IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day06IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}