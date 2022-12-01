using Common.Interfaces;

namespace Year2022;

public class Day01Solver : IPuzzleSolver
{
    public string Title => "Calorie Counting";
    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        var puzzleInput = input.GetCaloriesPerElf().ToArray();

        Part1Solution = SolvePart1(puzzleInput).ToString();
        Part2Solution = SolvePart2(puzzleInput).ToString();
    }

    private static int SolvePart1(IEnumerable<int> caloriesPerElf)
    {
        return caloriesPerElf.Max();
    }

    private static int SolvePart2(IEnumerable<int> caloriesPerElf)
    {
        var topThreeWithMaxCalories = caloriesPerElf
            .OrderByDescending(cal => cal)
            .Take(3);
        
        return topThreeWithMaxCalories.Sum();
    }
}

internal static class Extensions
{
    public static IEnumerable<int> GetCaloriesPerElf(this IEnumerable<string> input)
    {
        var calories = 0;

        foreach (var item in input)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                if (calories > 0)
                {
                    yield return calories;
                    calories = 0;
                }

                continue;
            }

            calories += int.Parse(item);
        }

        if (calories > 0)
        {
            yield return calories;
        }
    }
}
