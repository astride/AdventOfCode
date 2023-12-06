using System.Diagnostics;
using System.Text;
using Common.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Year2023Tests;

public abstract class TestsBase
{
    protected abstract string DirectoryName { get; }
    protected abstract string Part1Solution { get; }
    protected abstract string Part2Solution { get; }
    protected abstract string Part1ExampleSolution { get; }
    protected abstract string Part2ExampleSolution { get; }

    protected IPuzzleSolver PuzzleSolver { get; set; }

    private const string ProjectName = "Year2023";
    private const string InputFileName = "Input.txt";
    private const string ExampleInputFileName = "InputExample.txt";
    private const string ExampleInputFileNamePart1 = "InputExamplePart1.txt";
    private const string ExampleInputFileNamePart2 = "InputExamplePart2.txt";
    
    private static readonly string RootLocation =
        Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName ??
        string.Empty;

    protected void VerifyPart1()
    {
        var input = GetContentOf(InputFileName);
        
        SolvePart1AndLogExecutionTime(input, false);
        VerifyPart1SolutionAgainstExpectedOutput(Part1Solution);
    }

    protected void VerifyPart2()
    {
        var input = GetContentOf(InputFileName);
        
        SolvePart2AndLogExecutionTime(input, false);
        VerifyPart2SolutionAgainstExpectedOutput(Part2Solution);
    }

    protected void VerifyPart1WithExampleInput()
    {
        var inputFileName = PuzzleSolver.UsePartSpecificExampleInputFiles
            ? ExampleInputFileNamePart1
            : ExampleInputFileName;

        SolvePart1WithExampleInput(inputFileName);
        VerifyPart1SolutionAgainstExpectedOutput(Part1ExampleSolution);
    }

    protected void VerifyPart2WithExampleInput()
    {
        var inputFileName = PuzzleSolver.UsePartSpecificExampleInputFiles
            ? ExampleInputFileNamePart2
            : ExampleInputFileName;

        SolvePart2WithExampleInput(inputFileName);
        VerifyPart2SolutionAgainstExpectedOutput(Part2ExampleSolution);
    }

    private void SolvePart1WithExampleInput(string fileName)
    {
        var exampleInput = GetContentOf(fileName);
        
        SolvePart1AndLogExecutionTime(exampleInput, true);
    }

    private void SolvePart2WithExampleInput(string fileName)
    {
        var exampleInput = GetContentOf(fileName);
        
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
