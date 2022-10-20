using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day04Tests : TestsBase
{
    protected override string DirectoryName => "Day04";
    protected override string Part1Solution => "35711";
    protected override string Part2Solution => "5586";
    protected override string Part1ExampleSolution => "4512";
    protected override string Part2ExampleSolution => "1924";

    public Day04Tests()
    {
        PuzzleSolver = new Day04Solver();
    }
    
    [TestMethod]
    public void Day04IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day04IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}