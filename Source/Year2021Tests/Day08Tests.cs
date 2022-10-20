using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day08Tests : TestsBase
{
    protected override string DirectoryName => "Day08";
    protected override string Part1Solution => "247";
    protected override string Part2Solution => "933305";
    protected override string Part1ExampleSolution => "26";
    protected override string Part2ExampleSolution => "61229";

    public Day08Tests()
    {
        PuzzleSolver = new Day08Solver();
    }
    
    [TestMethod]
    public void Day08IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day08IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}