using Year2021;

namespace Year2021Tests;

[TestClass]
public class Day21Tests : TestsBase
{
    protected override string DirectoryName => "Day21";
    protected override string Part1Solution => "503478";
    protected override string Part2Solution => "716241959649754";
    protected override string Part1ExampleSolution => "739785";
    protected override string Part2ExampleSolution => "444356092776315";

    public Day21Tests()
    {
        PuzzleSolver = new Day21Solver();
    }
    
    [TestMethod]
    public void Day21IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day21IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}