using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day14Tests : TestsBase
{
    protected override string DirectoryName => "Day14";
    protected override string Part1ExampleSolution => "24";
    protected override string Part1Solution => "799";
    protected override string Part2ExampleSolution => "93";
    protected override string Part2Solution => "29076";

    public Day14Tests()
    {
        PuzzleSolver = new Day14Solver();
    }
    
    [TestMethod]
    public void Day14IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day14IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}