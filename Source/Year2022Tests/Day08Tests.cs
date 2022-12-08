using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day08Tests : TestsBase
{
    protected override string DirectoryName => "Day08";
    protected override string Part1Solution => "1538";
    protected override string Part2Solution => "496125";
    protected override string Part1ExampleSolution => "21";
    protected override string Part2ExampleSolution => "8";

    public Day08Tests()
    {
        PuzzleSolver = new Day08Solver();
    }
    
    [TestMethod]
    public void Day08IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day08IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}