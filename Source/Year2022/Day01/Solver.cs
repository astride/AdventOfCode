using Common.Interfaces;

namespace Year2022;

public class Day01Solver : IPuzzleSolver
{
    public string Title => string.Empty;
    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        var puzzleInput = new[] { 1, 2, 3 };

        Part1Solution = SolvePart1(puzzleInput).ToString();
        Part2Solution = SolvePart2(puzzleInput).ToString();
    }

    private static int SolvePart1(IReadOnlyList<int> input)
    {
        return 0;
    }

    private static int SolvePart2(IReadOnlyList<int> input)
    {
        return 0;
    }
}
