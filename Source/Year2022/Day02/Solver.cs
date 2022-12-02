using Common.Interfaces;

namespace Year2022;

public class Day02Solver : IPuzzleSolver
{
    public string Title => "Rock Paper Scissors";
    
    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] encryptedStrategyGuide)
    {
        Part1Solution = SolvePart1(encryptedStrategyGuide).ToString();
        Part2Solution = SolvePart2(encryptedStrategyGuide).ToString();
    }

    private static int SolvePart1(IEnumerable<string> encryptedStrategyGuide)
    {
        var rounds = encryptedStrategyGuide.GetRoundsForPart1();
        
        return rounds.GetScore();
    }

    private static int SolvePart2(IEnumerable<string> encryptedStrategyGuide)
    {
        var rounds = encryptedStrategyGuide.GetRoundsForPart2();

        return rounds.GetScore();
    }
}

internal static class Day02Extensions
{
    public static IEnumerable<Round> GetRoundsForPart1(this IEnumerable<string> guide)
    {
        var roundPerGuideline = new Dictionary<string, Round>
        {
            ["A X"] = new(Shape.Rock, Outcome.Draw),
            ["A Y"] = new(Shape.Paper, Outcome.Win),
            ["A Z"] = new(Shape.Scissor, Outcome.Loss),
            ["B X"] = new(Shape.Rock, Outcome.Loss),
            ["B Y"] = new(Shape.Paper, Outcome.Draw),
            ["B Z"] = new(Shape.Scissor, Outcome.Win),
            ["C X"] = new(Shape.Rock, Outcome.Win),
            ["C Y"] = new(Shape.Paper, Outcome.Loss),
            ["C Z"] = new(Shape.Scissor, Outcome.Draw),
        };

        return guide
            .Select(guideline => roundPerGuideline[guideline])
            .ToList();
    }
    
    public static IEnumerable<Round> GetRoundsForPart2(this IEnumerable<string> guide)
    {
        var roundPerGuideline = new Dictionary<string, Round>
        {
            ["A X"] = new(Shape.Scissor, Outcome.Loss),
            ["A Y"] = new(Shape.Rock, Outcome.Draw),
            ["A Z"] = new(Shape.Paper, Outcome.Win),
            ["B X"] = new(Shape.Rock, Outcome.Loss),
            ["B Y"] = new(Shape.Paper, Outcome.Draw),
            ["B Z"] = new(Shape.Scissor, Outcome.Win),
            ["C X"] = new(Shape.Paper, Outcome.Loss),
            ["C Y"] = new(Shape.Scissor, Outcome.Draw),
            ["C Z"] = new(Shape.Rock, Outcome.Win),
        };

        return guide
            .Select(guideline => roundPerGuideline[guideline])
            .ToList();
    }

    public static int GetScore(this IEnumerable<Round> rounds)
    {
        var scoreByShape = new Dictionary<Shape, int>
        {
            [Shape.Rock] = 1,
            [Shape.Paper] = 2,
            [Shape.Scissor] = 3,
        };

        var scoreByOutcome = new Dictionary<Outcome, int>
        {
            [Outcome.Loss] = 0,
            [Outcome.Draw] = 3,
            [Outcome.Win] = 6,
        };

        return rounds
            .Sum(round => scoreByShape[round.Shape] + scoreByOutcome[round.Outcome]);
    }
}

internal enum Outcome
{
    Loss,
    Draw,
    Win,
}

internal enum Shape
{
    Rock,
    Scissor,
    Paper,
}

internal record Round(
    Shape Shape,
    Outcome Outcome);
