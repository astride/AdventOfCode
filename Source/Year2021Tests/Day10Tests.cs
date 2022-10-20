using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day10Tests : TestsBase
{
    protected override string DirectoryName => "Day10";
    protected override string Part1Solution => "343863";
    protected override string Part2Solution => "2924734236";
    protected override string Part1ExampleSolution => "26397";
    protected override string Part2ExampleSolution => "288957";

    public Day10Tests()
    {
        PuzzleSolver = new Day10Solver();
    }
    
    [TestMethod]
    public void Day10IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day10IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}