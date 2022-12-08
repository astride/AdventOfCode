using Common.Interfaces;

namespace Year2022;

public class Day08Solver : IPuzzleSolver
{
    public string Title => "Treetop Tree House";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        Part1Solution = SolvePart1(input).ToString();
        Part2Solution = SolvePart2(input).ToString();
    }

    private static int SolvePart1(IEnumerable<string> input)
    {
        var grid = input
            .Select(row => row
                .AsEnumerable()
                .Select(item => int.Parse(item.ToString()))
                .ToArray())
            .ToArray();

        var visibleTrees = 0;

        var rows = grid[0].Length;
        var cols = grid.Length;
        
        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < cols; col++)
            {
                if (row == 0 || col == 0 || row == rows - 1 || col == cols - 1)
                {
                    visibleTrees++;
                    continue;
                }

                var tree = grid[row][col];

                var tallestTreeAbove = Enumerable
                    .Range(0, row)
                    .Max(i => grid[i][col]);
                var tallestTreeRightOf = Enumerable
                    .Range(col + 1, cols - (col + 1))
                    .Max(i => grid[row][i]);
                var tallestTreeBelow = Enumerable
                    .Range(row + 1, rows - (row + 1))
                    .Max(i => grid[i][col]);
                var tallestTreeLeftOf = Enumerable
                    .Range(0, col)
                    .Max(i => grid[row][i]);

                if (tree > tallestTreeAbove ||
                    tree > tallestTreeRightOf ||
                    tree > tallestTreeBelow ||
                    tree > tallestTreeLeftOf)
                {
                    visibleTrees++;
                }
            }
        }

        return visibleTrees;
    }

    private static int SolvePart2(IEnumerable<string> input)
    {
        var grid = input
            .Select(row => row
                .AsEnumerable()
                .Select(item => int.Parse(item.ToString()))
                .ToArray())
            .ToArray();

        var highestScenicScore = 0;

        var rows = grid[0].Length;
        var cols = grid.Length;
        
        for (var row = 1; row < rows - 1; row++)
        {
            for (var col = 1; col < cols - 1; col++)
            {
                var tree = grid[row][col];

                var visibleTreesAbove = Enumerable
                    .Range(0, row)
                    .Reverse()
                    .Select(i => grid[i][col])
                    .ToList()
                    .FindIndex(treeAbove => treeAbove >= tree);
                var visibleTreesRightOf = Enumerable
                    .Range(col + 1, cols - (col + 1))
                    .Select(i => grid[row][i])
                    .ToList()
                    .FindIndex(treeRightOf => treeRightOf >= tree);
                var visibleTreesBelow = Enumerable
                    .Range(row + 1, rows - (row + 1))
                    .Select(i => grid[i][col])
                    .ToList()
                    .FindIndex(treeBelow => treeBelow >= tree);
                var visibleTreesLeftOf = Enumerable
                    .Range(0, col)
                    .Reverse()
                    .Select(i => grid[row][i])
                    .ToList()
                    .FindIndex(treeLeftOf => treeLeftOf >= tree);

                var scenicScore =
                    (visibleTreesAbove < 0 ? row : visibleTreesAbove + 1) *
                    (visibleTreesRightOf < 0 ? cols - (col + 1) : visibleTreesRightOf + 1) *
                    (visibleTreesBelow < 0 ? rows - (row + 1) : visibleTreesBelow + 1) *
                    (visibleTreesLeftOf < 0 ? col : visibleTreesLeftOf + 1);

                if (scenicScore > highestScenicScore)
                {
                    highestScenicScore = scenicScore;
                }
            }
        }

        return highestScenicScore;
    }
}
