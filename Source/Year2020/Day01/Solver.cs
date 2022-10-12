using Common.Interfaces;

namespace Year2020;

public class Day01Solver : IPuzzleSolver
{
    public string Part1Solution { get; set; }
    public string Part2Solution { get; set; }

    public void SolvePuzzle(string[] input)
    {
        var report = input
            .Select(entry => int.Parse(entry))
            .ToArray();

        Part1Solution = SolvePart1(report).ToString();
        Part2Solution = SolvePart2(report).ToString();
    }

    private const int Sum = 2020;

    private int SolvePart1(int[] report)
    {
        int expense1;
        int expense2;

        for (var i1 = 0; i1 < report.Length - 1; i1++)
        {
            expense1 = report[i1];

            for (var i2 = i1 + 1; i2 < report.Length; i2++)
            {
                expense2 = report[i2];

                if (expense1 + expense2 == Sum)
                {
                    return expense1 * expense2;
                }
            }
        }

        return -1;
    }

    private int SolvePart2(int[] report)
    {
        int expense1;
        int expense2;
        int expense3;

        for (var i1 = 0; i1 < report.Length - 2; i1++)
        {
            expense1 = report[i1];

            for (var i2 = i1 + 1; i2 < report.Length - 1; i2++)
            {
                expense2 = report[i2];

                for (var i3 = i2 + 1; i3 < report.Length; i3++)
                {
                    expense3 = report[i3];

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
