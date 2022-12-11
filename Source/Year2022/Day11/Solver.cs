using Common.Interfaces;

namespace Year2022;

public class Day11Solver : IPuzzleSolver
{
    public string Title => "Monkey in the Middle";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        Part1Solution = SolvePart1(input).ToString();
        //Part2Solution = SolvePart2(input).ToString();
    }

    private static long SolvePart1(IEnumerable<string> input)
    {
        var monkeys = input.GetMonkeyAttributes<int>();

        var inspectionCounter = Enumerable.Repeat(0, monkeys.Count).ToList();

        const int totalInspections = 20;

        foreach (var inspection in Enumerable.Range(1, totalInspections))
        {
            for (var i = 0; i < monkeys.Count; i++)
            {
                var monkey = monkeys[i];
                
                foreach (var _ in Enumerable.Range(1, monkey.Items.Count))
                {
                    var item = monkey.Items.Dequeue();
                    
                    var worryLevel = GetNewWorryLevel(item, monkey.InspectionInstructions);

                    worryLevel /= 3; // Divided by three and rounded down to the nearest integer

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

    private static int SolvePart2(IEnumerable<string> input)
    {
        return 0;
    }

    private static int GetNewWorryLevel(int item, (char Sign, string Operand) inspectionInstructions) // Send whole monkey?
    {
        var operand = inspectionInstructions.Operand == "old"
            ? item
            : int.Parse(inspectionInstructions.Operand);

        return inspectionInstructions.Sign switch
        {
            '*' => item * operand,
            '+' => item + operand,
            _ => item,
        };
    }
}

internal static class Day11Helpers
{
    public static List<Monkey<T>> GetMonkeyAttributes<T>(this IEnumerable<string> input)
    {
        var monkeys = new List<Monkey<T>>();

        var monkey = new Monkey<T>();

        foreach (var line in input)
        {
            if (line.StartsWith("M"))
            {
                continue;
            }
            
            if (string.IsNullOrEmpty(line))
            {
                monkeys.Add(monkey);

                monkey = new Monkey<T>();
                
                continue;
            }

            if (line.StartsWith("  S"))
            {
                var items = line
                    .Replace("  Starting items: ", string.Empty)
                    .Split(',', StringSplitOptions.TrimEntries)
                    .Select(item => (T)Convert.ChangeType(item, typeof(T)));

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

                monkey.InspectionInstructions = (char.Parse(operation[0]), operation[1]);
                
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

internal class Monkey<T>
{
    public Queue<T> Items { get; } = new();
    public (char Sign, string Operand) InspectionInstructions { get; set; } // Operand can be "old"!
    public int TestIsDivisibleBy { get; set; }
    public int IfTrueThrowTo { get; set; }
    public int IfFalseThrowTo { get; set; }
}
