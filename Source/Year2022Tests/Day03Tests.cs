using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day03Tests : TestsBase
{
    protected override string DirectoryName => "Day03";
    protected override string Part1Solution => "7872";
    protected override string Part2Solution => "2497";
    protected override string Part1ExampleSolution => "157";
    protected override string Part2ExampleSolution => "70";

    public Day03Tests()
    {
        PuzzleSolver = new Day03Solver();
    }
    
    [TestMethod]
    public void Day03IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day03IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}