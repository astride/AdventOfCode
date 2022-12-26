namespace Common.Interfaces;

public interface IPuzzleSolver
{
    string Title { get; }

    // Need not be set in implementation class
    object? Part1Solution { get; set; }

    // Need not be set in implementation class
    object? Part2Solution { get; set; }

    void SolvePuzzle(string[] input)
    {
        SolvePart1(input);
        SolvePart2(input);
    }

    void SolvePart1(string[] input)
    {
        Part1Solution = GetPart1Solution(input);
    }

    void SolvePart2(string[] input)
    {
        Part2Solution = GetPart2Solution(input);
    }

    object GetPart1Solution(string[] input);

    object GetPart2Solution(string[] input);

    // Only applicable for puzzles where part 1 and part 2 use different example input files
    bool UsePartSpecificExampleInputFiles => false;
}
