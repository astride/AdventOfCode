using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day02Tests : TestsBase
{
    protected override string DirectoryName => "Day02";
    protected override string Part1Solution => "1746616";
    protected override string Part2Solution => "1741971043";
    protected override string Part1ExampleSolution => "150";
    protected override string Part2ExampleSolution => "900";

    public Day02Tests()
    {
        PuzzleSolver = new Day02Solver();
    }
    
    [TestMethod]
    public void Day02IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day02IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}