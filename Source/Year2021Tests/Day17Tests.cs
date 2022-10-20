using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day17Tests : TestsBase
{
    protected override string DirectoryName => "Day17";
    protected override string Part1Solution => "5886";
    protected override string Part2Solution => "1806";
    protected override string Part1ExampleSolution => "45";
    protected override string Part2ExampleSolution => "112";

    public Day17Tests()
    {
        PuzzleSolver = new Day17Solver();
    }
    
    [TestMethod]
    public void Day17IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day17IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}