using System.Diagnostics;
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
        if (PuzzleSolver.UsePartSpecificExampleInputFiles)
        {
            SolvePart1WithExampleInput();
            SolvePart2WithExampleInput();
        }
        else
        {
            SolvePuzzleWithExampleInput();
        }
        
        VerifySolutionsWithExampleInput();
    }

    private void SolvePuzzle()
    {
        var input = GetContentOf(InputFileName);
        
        SolvePart1AndLogExecutionTime(input, false);
        SolvePart2AndLogExecutionTime(input, false);
    }

    private void SolvePuzzleWithExampleInput()
    {
        var exampleInput = GetContentOf(ExampleInputFileName);
        
        SolvePart1AndLogExecutionTime(exampleInput, true);
        SolvePart2AndLogExecutionTime(exampleInput, true);
    }

    private void SolvePart1WithExampleInput()
    {
        var exampleInput = GetContentOf(ExampleInputFileNamePart1);
        
        SolvePart1AndLogExecutionTime(exampleInput, true);
    }

    private void SolvePart2WithExampleInput()
    {
        var exampleInput = GetContentOf(ExampleInputFileNamePart2);
        
        SolvePart2AndLogExecutionTime(exampleInput, true);
    }

    private void SolvePart1AndLogExecutionTime(string[] input, bool isExampleInput)
    {
        ExecuteAndLogExecutionTime("Part 1", () => PuzzleSolver.SolvePart1(input, isExampleInput));
    }

    private void SolvePart2AndLogExecutionTime(string[] input, bool isExampleInput)
    {
        ExecuteAndLogExecutionTime("Part 2", () => PuzzleSolver.SolvePart2(input, isExampleInput));
    }

    private static void ExecuteAndLogExecutionTime(string actionName, Action action)
    {
        var stopwatch = Stopwatch.StartNew();

        action();
        stopwatch.Stop();

        Console.WriteLine($"{actionName} needed {stopwatch.ElapsedMilliseconds} ms to execute.");
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
        VerifyPart1SolutionAgainstExpectedOutput(expectedOutputPart1);
        VerifyPart2SolutionAgainstExpectedOutput(expectedOutputPart2);
    }

    private void VerifyPart1SolutionAgainstExpectedOutput(string expected)
    {
        Assert.AreEqual(expected, PuzzleSolver.Part1Solution?.ToString(), "(Part 1)");
    }

    private void VerifyPart2SolutionAgainstExpectedOutput(string expected)
    {
        Assert.AreEqual(expected, PuzzleSolver.Part2Solution?.ToString(), "(Part 2)");
    }

    private string[] GetContentOf(string fileName)
    {
        var filePath = GetPathFor(fileName);

        return File.ReadAllLines(filePath, Encoding.Default);
    }

    private string GetPathFor(string fileName) => Path.Combine(RootLocation, ProjectName, DirectoryName, fileName);
}