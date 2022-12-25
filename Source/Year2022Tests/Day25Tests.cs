using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day25Tests : TestsBase
{
    protected override string DirectoryName => "Day25";
    protected override string Part1ExampleSolution => "2=-1=0"; // SNAFU number for decimal number 4890
    protected override string Part1Solution => "";
    protected override string Part2ExampleSolution => "";
    protected override string Part2Solution => "";

    public Day25Tests()
    {
        PuzzleSolver = new Day25Solver();
    }
    
    [TestMethod]
    public void Day25IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day25IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}