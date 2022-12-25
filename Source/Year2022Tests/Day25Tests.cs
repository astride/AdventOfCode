using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day25Tests : TestsBase
{
    protected override string DirectoryName => "Day25";
    protected override string Part1ExampleSolution => "2=-1=0"; // Equals decimal number 4890
    protected override string Part1Solution => "20=212=1-12=200=00-1"; // Equals decimal number 36966761092496
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