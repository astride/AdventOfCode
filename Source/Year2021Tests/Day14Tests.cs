using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day14Tests : TestsBase
{
    protected override string DirectoryName => "Day14";
    protected override string Part1Solution => "2975";
    protected override string Part2Solution => "3015383850689";
    protected override string Part1ExampleSolution => "1588";
    protected override string Part2ExampleSolution => "2188189693529";

    public Day14Tests()
    {
        PuzzleSolver = new Day14Solver();
    }
    
    [TestMethod]
    public void Day14IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day14IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}