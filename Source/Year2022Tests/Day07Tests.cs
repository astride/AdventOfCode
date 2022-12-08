using Year2022;

namespace Year2022Tests;

[TestClass]
public class Day07Tests : TestsBase
{
    protected override string DirectoryName => "Day07";
    protected override string Part1Solution => "1118405";
    protected override string Part2Solution => "12545514";
    protected override string Part1ExampleSolution => "95437";
    protected override string Part2ExampleSolution => "24933642";

    public Day07Tests()
    {
        PuzzleSolver = new Day07Solver();
    }
    
    [TestMethod]
    public void Day07IsValid() => HasCorrectSolutions();

    [TestMethod]
    public void Day07IsValid_WithExampleInput() => HasCorrectSolutionsWithExampleInput();
}