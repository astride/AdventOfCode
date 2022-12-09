namespace Common.Interfaces;

public interface IPuzzleSolver
{
    string Title { get; }
    
    string Part1Solution { get; set; }

    string Part2Solution { get; set; }
    
    void SolvePuzzle(string[] input);

    // Only applicable for puzzles where part 1 and part 2 use different example input files
    virtual bool UsePartSpecificExampleInputFiles => false;

    virtual void SolvePart1(string[] input) { }

    virtual void SolvePart2(string[] input) { }
}