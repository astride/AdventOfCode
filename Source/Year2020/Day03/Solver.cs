using Common.Interfaces;
using Common.Models;

namespace Year2020;

public class Day03Solver : IPuzzleSolver
{
    public string Title => "Toboggan Trajectory";
    public object? Part1Solution { get; set; }
    public object? Part2Solution { get; set; }

    private const char TreeChar = '#';

    public object GetPart1Solution(string[] input, bool isExampleInput)
    {
        var treeMap = GetTreeMap(input);
        var slope = new Coordinate(3, 1);

        var treeCount = treeMap.GetTreeCountFor(slope);

        return treeCount;
    }

    public object GetPart2Solution(string[] input, bool isExampleInput)
    {
        var treeMap = GetTreeMap(input);
        
        var slopes = new Coordinate[]
        {
            new(1, 1),
            new(3, 1),
            new(5, 1),
            new(7, 1),
            new(1, 2),
        };
        
        var treeCountsMultiplied = slopes.Aggregate(
            (long)1,
            (acc, slope) => acc * treeMap.GetTreeCountFor(slope));

        return treeCountsMultiplied;
    }

    private static IReadOnlyList<IReadOnlyList<bool>> GetTreeMap(string[] input)
    {
        return input
            .Select(line => line
                .Select(ch => ch == TreeChar)
                .ToList())
            .ToList();
    }
}

internal static class Day03Helpers
{
    public static int GetTreeCountFor(this IReadOnlyList<IReadOnlyList<bool>> treeMap, Coordinate slope)
    {
        var mapHeight = treeMap.Count;
        var mapWidth = treeMap[0].Count;

        var slopeRepetitions = mapHeight / slope.Y;

        var startingPoint = Coordinate.Origin;

        var treeCount = 0;

        for (var i = 1; i < slopeRepetitions; i++)
        {
            var row = startingPoint.Y + i * slope.Y;
            var column = (startingPoint.X + i * slope.X) % mapWidth;
            
            if (treeMap[row][column])
            {
                treeCount++;
            }
        }

        return treeCount;
    }
}
