namespace Common.Interfaces;

public interface IPuzzleSolver
{
    string Title { get; }

    // Need not be set in implementation class
    object? Part1Solution { get; set; }

    // Need not be set in implementation class
    object? Part2Solution { get; set; }

    void SolvePuzzle(string[] input, bool isExampleInput)
    {
        SolvePart1(input, isExampleInput);
        SolvePart2(input, isExampleInput);
    }

    void SolvePart1(string[] input, bool isExampleInput)
    {
        Part1Solution = GetPart1Solution(input, isExampleInput);
    }

    void SolvePart2(string[] input, bool isExampleInput)
    {
        Part2Solution = GetPart2Solution(input, isExampleInput);
    }

    object GetPart1Solution(string[] input, bool isExampleInput);

    object GetPart2Solution(string[] input, bool isExampleInput);

    // Only applicable for puzzles where part 1 and part 2 use different example input files
    bool UsePartSpecificExampleInputFiles => false;
}
