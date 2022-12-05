using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day05Tests : TestsBase
{
    protected override string DirectoryName => "Day05";
    protected override string Part1Solution => "GRTSWNJHH";
    protected override string Part2Solution => "QLFQDBBHM";
    protected override string Part1ExampleSolution => "CMZ";
    protected override string Part2ExampleSolution => "MCD";

    public Day05Tests()
    {
        PuzzleSolver = new Day05Solver();
    }
    
    [TestMethod]
    public void Day05IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day05IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}