using Common.Interfaces;

namespace Year2022;

public class Day11Solver : IPuzzleSolver
{
    public string Title => "Monkey in the Middle";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    private static int TotalDivisibleByProduct;

    public void SolvePuzzle(string[] input)
    {
        Part1Solution = GetPart1Solution(input).ToString();
        Part2Solution = GetPart2Solution(input).ToString();
    }

    private static int GetPart1Solution(IEnumerable<string> input)
    {
        // TODO Refactor; almost everything is identical for Part1 and Part2
        var monkeys = input.GetMonkeyAttributes();

        SetTotalDivisibleByProduct(monkeys);

        var inspectionCounter = Enumerable.Repeat(0, monkeys.Count).ToList();

        const int totalInspections = 20;

        foreach (var inspection in Enumerable.Range(1, totalInspections))
        {
            for (var i = 0; i < monkeys.Count; i++)
            {
                var monkey = monkeys[i];
                
                foreach (var _ in Enumerable.Range(1, monkey.Items.Count))
                {
                    var worryLevel = GetNewWorryLevel(monkey) / 3;

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
            inspectionCountOfMostActiveMonkeys[0] * inspectionCountOfMostActiveMonkeys[1];
        
        return levelOfMonkeyBusiness;
    }

    private static long GetPart2Solution(IEnumerable<string> input)
    {
        var monkeys = input.GetMonkeyAttributes();

        SetTotalDivisibleByProduct(monkeys);

        var inspectionCounter = Enumerable.Repeat(0, monkeys.Count).ToList();

        const int totalInspections = 10000;

        foreach (var inspection in Enumerable.Range(1, totalInspections))
        {
            for (var i = 0; i < monkeys.Count; i++)
            {
                var monkey = monkeys[i];
                
                foreach (var _ in Enumerable.Range(1, monkey.Items.Count))
                {
                    var worryLevel = GetNewWorryLevel(monkey);

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
        var item = monkey.Items.Dequeue();
        
        long operand = monkey.InspectionInstructions.Operand ?? item;

        var newWorryLevel = monkey.InspectionInstructions.Sign switch
        {
            '*' => item * operand,
            '+' => item + operand,
            _ => item,
        };

        var remainder = (int)(newWorryLevel % TotalDivisibleByProduct);

        return remainder == 0
            ? TotalDivisibleByProduct
            : remainder;
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
            if (line.StartsWith("M"))
            {
                continue;
            }
            
            if (string.IsNullOrEmpty(line))
            {
                monkeys.Add(monkey);

                monkey = new Monkey();
                
                continue;
            }

            if (line.StartsWith("  S"))
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

            if (line.StartsWith("  O"))
            {
                var operation = line
                    .Replace("  Operation: new = old ", string.Empty)
                    .Split(' ');

                var sign = char.Parse(operation[0]);

                var operandIsGiven = int.TryParse(operation[1], out var operand);
                
                monkey.InspectionInstructions = (sign, operandIsGiven ? operand : null);
                
                continue;
            }

            if (line.StartsWith("  T"))
            {
                monkey.TestIsDivisibleBy = int.Parse(line
                    .Replace("  Test: divisible by ", string.Empty));
                
                continue;
            }

            if (line.StartsWith("    If t"))
            {
                monkey.IfTrueThrowTo = int.Parse(line
                    .Replace("    If true: throw to monkey ", string.Empty));
                
                continue;
            }

            if (line.StartsWith("    If f"))
            {
                monkey.IfFalseThrowTo = int.Parse(line
                    .Replace("    If false: throw to monkey ", string.Empty));
            }
        }
        
        monkeys.Add(monkey);

        return monkeys;
    }
}

internal class Monkey
{
    public Queue<int> Items { get; } = new();
    public (char Sign, int? Operand) InspectionInstructions { get; set; }
    public int TestIsDivisibleBy { get; set; }
    public int IfTrueThrowTo { get; set; }
    public int IfFalseThrowTo { get; set; }
}
