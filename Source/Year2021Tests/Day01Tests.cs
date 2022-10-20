using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day01Tests : TestsBase
{
    protected override string DirectoryName => "Day01";
    protected override string Part1Solution => "1448";
    protected override string Part2Solution => "1471";
    protected override string Part1ExampleSolution => "7";
    protected override string Part2ExampleSolution => "5";

    public Day01Tests()
    {
        PuzzleSolver = new Day01Solver();
    }
    
    [TestMethod]
    public void Day01IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day01IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}