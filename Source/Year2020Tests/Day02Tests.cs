using Year2020;

namespace Year2020Tests;

[TestClass]
public class Day02Tests : TestsBase
{
    protected override string DirectoryName => "Day02";
    protected override string Part1Solution => "517";
    protected override string Part2Solution => "284";
    protected override string Part1ExampleSolution => "2";
    protected override string Part2ExampleSolution => "1";

    public Day02Tests()
    {
        PuzzleSolver = new Day02Solver();
    }
    
    [TestMethod]
    public void Day02IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day02IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}