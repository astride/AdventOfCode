using Common.Interfaces;

namespace Year2022;

public class Day11Solver : IPuzzleSolver
{
    public string Title => "Monkey in the Middle";

    public object? Part1Solution { get; set; }
    public object? Part2Solution { get; set; }

    private static int TotalDivisibleByProduct;

    public object GetPart1Solution(string[] input, bool isExampleInput)
    {
        const int totalInspections = 20;
        const int worryLevelReductionFactor = 3;

        var levelOfMonkeyBusiness = GetLevelOfMonkeyBusiness(input, totalInspections, worryLevelReductionFactor);

        return levelOfMonkeyBusiness;
    }

    public object GetPart2Solution(string[] input, bool isExampleInput)
    {
        const int totalInspections = 10000;

        var levelOfMonkeyBusiness = GetLevelOfMonkeyBusiness(input, totalInspections);

        return levelOfMonkeyBusiness;
    }

    private static long GetLevelOfMonkeyBusiness(
        IEnumerable<string> input,
        int totalInspections,
        int worryLevelReductionFactor = 1)
    {
        var monkeys = input.GetMonkeyAttributes();

        SetTotalDivisibleByProduct(monkeys);

        var inspectionCounter = Enumerable.Repeat(0, monkeys.Count).ToList();

        foreach (var inspection in Enumerable.Range(1, totalInspections))
        {
            for (var i = 0; i < monkeys.Count; i++)
            {
                var monkey = monkeys[i];
                
                foreach (var item in Enumerable.Range(1, monkey.Items.Count))
                {
                    var worryLevel = GetNewWorryLevel(monkey) / worryLevelReductionFactor;

                    var throwToMonkeyIndex = worryLevel % monkey.TestIsDivisibleBy == 0
                        ? monkey.IfTrueThrowTo
                        : monkey.IfFalseThrowTo;
                    
                    monkeys[throwToMonkeyIndex].Items.Enqueue(worryLevel);

                    inspectionCounter[i]++;
                }
            }
        }

        var inspectionCountOfMostActiveMonkeys = inspectionCounter
            .OrderByDescending(inspectionCount => inspectionCount)
            .Take(2)
            .ToList();

        var levelOfMonkeyBusiness =
            (long)inspectionCountOfMostActiveMonkeys[0] * inspectionCountOfMostActiveMonkeys[1];
        
        return levelOfMonkeyBusiness;
    }

    private static int GetNewWorryLevel(Monkey monkey)
    {
        var itemWorryLevel = monkey.Items.Dequeue();
        
        long operand = monkey.InspectionInstructions.Operand ?? itemWorryLevel;

        var newWorryLevel = monkey.InspectionInstructions.Sign switch
        {
            '*' => itemWorryLevel * operand,
            '+' => itemWorryLevel + operand,
            _ => itemWorryLevel,
        };

        var remainder = (int)(newWorryLevel % TotalDivisibleByProduct);

        if (remainder == 0)
        {
            return TotalDivisibleByProduct;
        }

        return remainder;
    }

    private static void SetTotalDivisibleByProduct(IEnumerable<Monkey> monkeys)
    {
        TotalDivisibleByProduct = monkeys
            .Select(monkey => monkey.TestIsDivisibleBy)
            .Aggregate(1, (acc, next) => acc * next);
    }
}

internal static class Day11Helpers
{
    public static List<Monkey> GetMonkeyAttributes(this IEnumerable<string> input)
    {
        var monkeys = new List<Monkey>();

        var monkey = new Monkey();

        foreach (var line in input)
        {
            if (IsMonkeyIntroductionLine(line))
            {
                continue;
            }
            
            if (IsMonkeySeparationLine(line))
            {
                monkeys.Add(monkey);

                monkey = new Monkey();
                
                continue;
            }

            if (IsStartingItemsLine(line))
            {
                var items = line
                    .Replace("  Starting items: ", string.Empty)
                    .Split(',', StringSplitOptions.TrimEntries)
                    .Select(int.Parse);

                foreach (var item in items)
                {
                    monkey.Items.Enqueue(item);
                }
                
                continue;
            }

            if (IsOperationLine(line))
            {
                var operation = line
                    .Replace("  Operation: new = old ", string.Empty)
                    .Split(' ');

                var sign = char.Parse(operation[0]);

                var operandIsGiven = int.TryParse(operation[1], out var operand);
                
                monkey.InspectionInstructions = (sign, operandIsGiven ? operand : null);
                
                continue;
            }

            if (IsTestLine(line))
            {
                monkey.TestIsDivisibleBy = int.Parse(line
                    .Replace("  Test: divisible by ", string.Empty));
                
                continue;
            }

            if (IsPassingTestActionLine(line))
            {
                monkey.IfTrueThrowTo = int.Parse(line
                    .Replace("    If true: throw to monkey ", string.Empty));
                
                continue;
            }

            if (IsFailingTestActionLine(line))
            {
                monkey.IfFalseThrowTo = int.Parse(line
                    .Replace("    If false: throw to monkey ", string.Empty));
            }
        }
        
        monkeys.Add(monkey);

        return monkeys;
    }

    private static bool IsMonkeyIntroductionLine(string line) => line.StartsWith("M");
    private static bool IsMonkeySeparationLine(string line) => string.IsNullOrEmpty(line);
    private static bool IsStartingItemsLine(string line) => line.StartsWith("  S");
    private static bool IsOperationLine(string line) => line.StartsWith("  O");
    private static bool IsTestLine(string line) => line.StartsWith("  T");
    private static bool IsPassingTestActionLine(string line) => line.StartsWith("    If t");
    private static bool IsFailingTestActionLine(string line) => line.StartsWith("    If f");
}

internal class Monkey
{
    public Queue<int> Items { get; } = new();
    public (char Sign, int? Operand) InspectionInstructions { get; set; }
    public int TestIsDivisibleBy { get; set; }
    public int IfTrueThrowTo { get; set; }
    public int IfFalseThrowTo { get; set; }
}
