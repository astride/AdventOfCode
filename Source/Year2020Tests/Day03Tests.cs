using Year2020;

namespace Year2020Tests;

[TestClass]
public class Day03Tests : TestsBase
{
    protected override string DirectoryName => "Day03";
    protected override string Part1Solution => "214";
    protected override string Part2Solution => "336";
    protected override string Part1ExampleSolution => "7";
    protected override string Part2ExampleSolution => "8336352024";

    public Day03Tests()
    {
        PuzzleSolver = new Day03Solver();
    }
    
    [TestMethod]
    public void Day03IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day03IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}