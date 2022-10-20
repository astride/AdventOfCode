using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day13Tests : TestsBase
{
    protected override string DirectoryName => "Day13";
    protected override string Part1Solution => "687";
    protected override string Part2Solution => "FGKCKBZG"; // Displayed as image in console
    protected override string Part1ExampleSolution => "17";
    protected override string Part2ExampleSolution => "[]"; // Displayed as image in console: Squared shape

    protected override bool SkipVerificationOfPart2 => true;
    protected override bool SkipVerificationOfPart2Example => true;

    public Day13Tests()
    {
        PuzzleSolver = new Day13Solver();
    }
    
    [TestMethod]
    public void Day13IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day13IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}