using Common.Interfaces;

namespace Year2022;

public class Day03Solver : IPuzzleSolver
{
    public string Title => "";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        var puzzleInput = input.ToList();

        Part1Solution = SolvePart1(puzzleInput).ToString();
        Part2Solution = SolvePart2(puzzleInput).ToString();
    }

    private static int SolvePart1(IEnumerable<string> input)
    {
        return 0;
    }

    private static int SolvePart2(IEnumerable<string> input)
    {
        return 0;
    }
}
