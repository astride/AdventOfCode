using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day11Tests : TestsBase
{
    protected override string DirectoryName => "Day11";
    protected override string Part1Solution => "54752";
    protected override string Part2Solution => "13606755504";
    protected override string Part1ExampleSolution => "10605";
    protected override string Part2ExampleSolution => "2713310158";

    public Day11Tests()
    {
        PuzzleSolver = new Day11Solver();
    }
    
    [TestMethod]
    public void Day11IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day11IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}