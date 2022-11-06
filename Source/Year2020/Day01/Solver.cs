using Common.Interfaces;

namespace Year2020;

public class Day01Solver : IPuzzleSolver
{
    public string Title => "Report Repair";
    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        var report = input
            .Select(int.Parse)
            .ToArray();

        Part1Solution = SolvePart1(report).ToString();
        Part2Solution = SolvePart2(report).ToString();
    }

    private const int Sum = 2020;

    private static int SolvePart1(IReadOnlyList<int> report)
    {
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

    private static int SolvePart2(IReadOnlyList<int> report)
    {
        for (var i = 0; i < report.Count - 2; i++)
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
}
