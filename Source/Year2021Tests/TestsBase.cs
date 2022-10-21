using System.Text;
using Common.Interfaces;

namespace Year2021Tests;

public abstract class TestsBase
{
    protected abstract string DirectoryName { get; }
    protected abstract string Part1Solution { get; }
    protected abstract string Part2Solution { get; }
    protected abstract string Part1ExampleSolution { get; }
    protected abstract string Part2ExampleSolution { get; }

    protected virtual bool SkipVerificationOfPart2 => false;

    protected IPuzzleSolver PuzzleSolver { get; init; }

    private const string ProjectName = "Year2021";
    private const string InputFileName = "Input.txt";
    private const string ExampleInputFileName = "InputExample.txt";
    
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
        SolvePuzzleWithExampleInput();
        VerifySolutionsWithExampleInput();
    }

    private void SolvePuzzle()
    {
        var input = GetContentOf(InputFileName);
        
        PuzzleSolver.SolvePuzzle(input);
    }

    private void SolvePuzzleWithExampleInput()
    {
        var exampleInput = GetContentOf(ExampleInputFileName);
        
        PuzzleSolver.SolvePuzzle(exampleInput);
    }

    private void VerifySolutions()
    {
        VerifySolutionsAgainstExpectedOutput(Part1Solution, Part2Solution);
    }

    private void VerifySolutionsWithExampleInput()
    {
        VerifySolutionsAgainstExpectedOutput(Part1ExampleSolution, Part2ExampleSolution);
    }

    private void VerifySolutionsAgainstExpectedOutput(string expectedOutputPart1, string expectedOutputPart2)
    {
        Assert.AreEqual(expectedOutputPart1, PuzzleSolver.Part1Solution);

        if (SkipVerificationOfPart2)
        {
            InformUserThatVerificationOfPart2WasSkipped();
            return;
        }

        Assert.AreEqual(expectedOutputPart2, PuzzleSolver.Part2Solution);
    }

    private static void InformUserThatVerificationOfPart2WasSkipped()
    {
        Console.WriteLine("Verification of Part 2 was skipped. Has it not been solved yet?");
    }

    private string[] GetContentOf(string fileName)
    {
        var filePath = GetPathFor(fileName);

        return File.ReadAllLines(filePath, Encoding.Default);
    }

    private string GetPathFor(string fileName) => Path.Combine(RootLocation, ProjectName, DirectoryName, fileName);
}