using Common.Interfaces;
using Common.Models;

namespace Year2020;

public class Day03Solver : IPuzzleSolver
{
    public string Title => "Toboggan Trajectory";
    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    private const char TreeChar = '#';

    public void SolvePuzzle(string[] input)
    {
        var treeMap = input
            .Select(line => line
                .Select(ch => ch == TreeChar)
                .ToList())
            .ToList();
        
        Part1Solution = SolvePart1(treeMap).ToString();
        Part2Solution = SolvePart2(treeMap).ToString();
    }

    private int SolvePart1(List<List<bool>> treeMap)
    {
        var mapHeight = treeMap.Count;
        var sourceMapWidth = treeMap[0].Count;

        var startingPoint = Coordinate.Origin;

        var slope = new Coordinate(3, 1);
        var slopeRepetitions = mapHeight / slope.Y;

        var treeCount = 0;

        for (var i = 1; i < slopeRepetitions; i++)
        {
            var row = startingPoint.Y + i * slope.Y;
            var column = (startingPoint.X + i * slope.X) % sourceMapWidth;
            
            if (treeMap[row][column])
            {
                treeCount++;
            }
        }
        
        return treeCount;
    }

    private int SolvePart2(List<List<bool>> treeMap)
    {
        return 0;
    }
}