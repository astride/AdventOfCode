using Common.Interfaces;

namespace Year2022;

public class Day01Solver : IPuzzleSolver
{
    public string Title => "Calorie Counting";
    public object? Part1Solution { get; set; }
    public object? Part2Solution { get; set; }

    public object GetPart1Solution(string[] input, bool isExampleInput)
    {
        var caloriesPerElf = GetCaloriesPerElf(input);
        
        return caloriesPerElf.Max();
    }

    public object GetPart2Solution(string[] input, bool isExampleInput)
    {
        var caloriesPerElf = GetCaloriesPerElf(input);
        
        var topThreeWithMaxCalories = caloriesPerElf
            .OrderByDescending(cal => cal)
            .Take(3);
        
        return topThreeWithMaxCalories.Sum();
    }

    private static IEnumerable<int> GetCaloriesPerElf(string[] input)
    {
        return input.GetCaloriesPerElf().ToArray();
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
