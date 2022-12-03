using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day02Tests : TestsBase
{
    protected override string DirectoryName => "Day02";
    protected override string Part1Solution => "10595";
    protected override string Part2Solution => "9541";
    protected override string Part1ExampleSolution => "15";
    protected override string Part2ExampleSolution => "12";

    public Day02Tests()
    {
        PuzzleSolver = new Day02Solver();
    }
    
    [TestMethod]
    public void Day02IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day02IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}