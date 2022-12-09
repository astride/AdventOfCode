using System.Text;
using Common.Interfaces;

namespace Year2022Tests;

public abstract class TestsBase
{
    protected abstract string DirectoryName { get; }
    protected abstract string Part1Solution { get; }
    protected abstract string Part2Solution { get; }
    protected abstract string Part1ExampleSolution { get; }
    protected abstract string Part2ExampleSolution { get; }

    protected IPuzzleSolver PuzzleSolver { get; set; }

    private const string ProjectName = "Year2022";
    private const string InputFileName = "Input.txt";
    private const string ExampleInputFileName = "InputExample.txt";
    private const string ExampleInputFileNamePart1 = "InputExamplePart1.txt";
    private const string ExampleInputFileNamePart2 = "InputExamplePart2.txt";
    
    private static readonly string RootLocation =
        Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName ??
        string.Empty;

    protected void HasCorrectSolutions()
    {
        SolvePuzzle();
        VerifySolutions();
    }

    protected void HasCorrectSolutionsWithExampleInput()
    {
        if (PuzzleSolver.UsePartSpecificInputFiles)
        {
            // TODO Need to solve this situation better, so that each part doesn't need to run with the wrong example input when solving the other part...
            
            SolvePuzzleWithExampleInput(ExampleInputFileNamePart1);
            VerifyPart1SolutionAgainstExpectedOutput(Part1ExampleSolution);

            SolvePuzzleWithExampleInput(ExampleInputFileNamePart2);
            VerifyPart2SolutionAgainstExpectedOutput(Part2ExampleSolution);
            
            return;
        }
        
        SolvePuzzleWithExampleInput(ExampleInputFileName);
        VerifySolutionsAgainstExpectedOutput(Part1ExampleSolution, Part2ExampleSolution);
    }

    private void SolvePuzzle()
    {
        var input = GetContentOf(InputFileName);
        
        PuzzleSolver.SolvePuzzle(input);
    }

    private void SolvePuzzleWithExampleInput(string inputFileName)
    {
        var exampleInput = GetContentOf(inputFileName);
        
        PuzzleSolver.SolvePuzzle(exampleInput);
    }

    private void VerifySolutions()
    {
        VerifySolutionsAgainstExpectedOutput(Part1Solution, Part2Solution);
    }

    private void VerifySolutionsAgainstExpectedOutput(string expectedOutputPart1, string expectedOutputPart2)
    {
        VerifyPart1SolutionAgainstExpectedOutput(expectedOutputPart1);
        VerifyPart2SolutionAgainstExpectedOutput(expectedOutputPart2);
    }

    private void VerifyPart1SolutionAgainstExpectedOutput(string expected)
    {
        Assert.AreEqual(expected, PuzzleSolver.Part1Solution, "(Part 1)");
    }

    private void VerifyPart2SolutionAgainstExpectedOutput(string expected)
    {
        Assert.AreEqual(expected, PuzzleSolver.Part2Solution, "(Part 2)");
    }

    private string[] GetContentOf(string fileName)
    {
        var filePath = GetPathFor(fileName);

        return File.ReadAllLines(filePath, Encoding.Default);
    }

    private string GetPathFor(string fileName) => Path.Combine(RootLocation, ProjectName, DirectoryName, fileName);
}