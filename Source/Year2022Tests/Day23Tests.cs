using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day23Tests : TestsBase
{
    protected override string DirectoryName => "Day23";
    protected override string Part1ExampleSolution => "110";
    protected override string Part1Solution => "3766";
    protected override string Part2ExampleSolution => "";
    protected override string Part2Solution => "";

    public Day23Tests()
    {
        PuzzleSolver = new Day23Solver();
    }
    
    [TestMethod]
    public void Day23IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day23IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}