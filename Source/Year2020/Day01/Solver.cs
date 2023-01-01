using Common.Interfaces;

namespace Year2020;

public class Day01Solver : IPuzzleSolver
{
    public string Title => "Report Repair";
    public object? Part1Solution { get; set; }
    public object? Part2Solution { get; set; }

    private const int Sum = 2020;

    public object GetPart1Solution(string[] input, bool isExampleInput)
    {
        var report = GetReport(input);

        var differences = new HashSet<int>();

        foreach (var expense in report)
        {
            var difference = Sum - expense;

            if (differences.Contains(expense))
            {
                return expense * difference;
            }

            differences.Add(difference);
        }
        
        return -1;
    }

    public object GetPart2Solution(string[] input, bool isExampleInput)
    {
        var report = GetReport(input);
        
        for (var i = 0; i < report.Length - 2; i++)
        {
            var expense1 = report[i];
            var missingExpenses = Sum - expense1;

            foreach (var expense2 in report.Skip(i + 1).SkipLast(1))
            {
                var expense3 = missingExpenses - expense2;

                if (report.Skip(i + 2).Contains(expense3))
                {
                    return expense1 * expense2 * expense3;
                }
            }
        }

        return -1;
    }

    private static int[] GetReport(IEnumerable<string> input)
    {
        return input
            .Select(int.Parse)
            .ToArray();
    }
}
