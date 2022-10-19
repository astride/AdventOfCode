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

    protected IPuzzleSolver PuzzleSolver;

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
        Assert.AreEqual(Part1Solution, PuzzleSolver.Part1Solution);
        Assert.AreEqual(Part2Solution, PuzzleSolver.Part2Solution);
    }

    private void VerifySolutionsWithExampleInput()
    {
        Assert.AreEqual(Part1ExampleSolution, PuzzleSolver.Part1Solution);
        Assert.AreEqual(Part2ExampleSolution, PuzzleSolver.Part2Solution);
    }

    private string[] GetContentOf(string fileName)
    {
        var filePath = GetPathFor(fileName);

        return File.ReadAllLines(filePath, Encoding.Default);
    }

    private string GetPathFor(string fileName) => Path.Combine(RootLocation, ProjectName, DirectoryName, fileName);
}