using Common.Interfaces;

namespace Year2020;

public class Day01Solver : IPuzzleSolver
{
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
        for (var i1 = 0; i1 < report.Count - 2; i1++)
        {
            var expense1 = report[i1];

            for (var i2 = i1 + 1; i2 < report.Count - 1; i2++)
            {
                var expense2 = report[i2];

                for (var i3 = i2 + 1; i3 < report.Count; i3++)
                {
                    var expense3 = report[i3];

                    if (expense1 + expense2 + expense3 == Sum)
                    {
                        return expense1 * expense2 * expense3;
                    }
                }
            }
        }

        return -1;
    }
}
