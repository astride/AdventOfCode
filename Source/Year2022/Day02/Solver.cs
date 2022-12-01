using Common.Interfaces;

namespace Year2022;

public class Day02Solver : IPuzzleSolver
{
    public string Title => ""; // TODO
    
    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        var puzzleInput = input
            .Where(item => !string.IsNullOrEmpty(item))
            .Select(int.Parse)
            .ToArray();

        Part1Solution = SolvePart1(puzzleInput).ToString();
        //Part2Solution = SolvePart2(puzzleInput).ToString();
    }

    private static int SolvePart1(IEnumerable<int> input)
    {
        return 0;
    }

    private static int SolvePart2(IEnumerable<int> input)
    {
        return 0;
    }
}
