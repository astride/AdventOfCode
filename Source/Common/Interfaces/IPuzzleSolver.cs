namespace Common.Interfaces;

public interface IPuzzleSolver
{
    string Title { get; }
    
    string Part1Solution { get; set; }

    string Part2Solution { get; set; }
    
    void SolvePuzzle(string[] input);
}