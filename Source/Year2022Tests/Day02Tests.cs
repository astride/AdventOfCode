using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day02Tests : TestsBase
{
    protected override string DirectoryName => "Day02";
    protected override string Part1Solution => "";
    protected override string Part2Solution => "";
    protected override string Part1ExampleSolution => "";
    protected override string Part2ExampleSolution => "";

    public Day02Tests()
    {
        PuzzleSolver = new Day01Solver();
    }
    
    [TestMethod]
    public void Day01IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day01IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}