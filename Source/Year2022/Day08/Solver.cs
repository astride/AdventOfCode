using Common.Interfaces;

namespace Year2022;

public class Day08Solver : IPuzzleSolver
{
    public string Title => "Treetop Tree House";

    public object? Part1Solution { get; set; }
    public object? Part2Solution { get; set; }

    public object GetPart1Solution(string[] input)
    {
        var trees = GetTrees(input);
        
        var rows = trees[0].Length;
        var cols = trees.Length;

        bool IsEdgeTree(int row, int col)
        {
            var isTopEdge = row == 0;
            var isLeftEdge = col == 0;

            var isBottomEdge = row == rows - 1;
            var isRightEdge = col == cols - 1;

            return isTopEdge || isLeftEdge || isBottomEdge || isRightEdge;
        }
        
        var visibleTrees = 0;

        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < cols; col++)
            {
                if (IsEdgeTree(row, col))
                {
                    visibleTrees++;
                    continue;
                }

                var tree = trees[row][col];

                var tallestTreeAbove = Enumerable
                    .Range(0, row)
                    .Max(rowAbove => trees[rowAbove][col]);

                var tallestTreeLeftOf = Enumerable
                    .Range(0, col)
                    .Max(colLeftOf => trees[row][colLeftOf]);

                var tallestTreeBelow = Enumerable
                    .Range(row + 1, rows - (row + 1))
                    .Max(rowBelow => trees[rowBelow][col]);

                var tallestTreeRightOf = Enumerable
                    .Range(col + 1, cols - (col + 1))
                    .Max(colRightOf => trees[row][colRightOf]);

                if (tree > tallestTreeAbove ||
                    tree > tallestTreeLeftOf ||
                    tree > tallestTreeBelow ||
                    tree > tallestTreeRightOf)
                {
                    visibleTrees++;
                }
            }
        }

        return visibleTrees;
    }

    public object GetPart2Solution(string[] input)
    {
        var trees = GetTrees(input);
        
        var rows = trees[0].Length;
        var cols = trees.Length;
        
        var highestScenicScore = 0;

        for (var row = 1; row < rows - 1; row++)
        {
            for (var col = 1; col < cols - 1; col++)
            {
                var tree = trees[row][col];

                var visibleTreesAbove = 1 + Enumerable
                    .Range(0, row)
                    .Reverse()
                    .Select(rowAbove => trees[rowAbove][col])
                    .ToList()
                    .FindIndex(treeAbove => treeAbove >= tree);

                var visibleTreesLeftOf = 1 + Enumerable
                    .Range(0, col)
                    .Reverse()
                    .Select(colLeftOf => trees[row][colLeftOf])
                    .ToList()
                    .FindIndex(treeLeftOf => treeLeftOf >= tree);

                var visibleTreesBelow = 1 + Enumerable
                    .Range(row + 1, rows - (row + 1))
                    .Select(rowBelow => trees[rowBelow][col])
                    .ToList()
                    .FindIndex(treeBelow => treeBelow >= tree);

                var visibleTreesRightOf = 1 + Enumerable
                    .Range(col + 1, cols - (col + 1))
                    .Select(colRightOf => trees[row][colRightOf])
                    .ToList()
                    .FindIndex(treeRightOf => treeRightOf >= tree);

                var scenicScore =
                    (visibleTreesAbove > 0 ? visibleTreesAbove : row) *
                    (visibleTreesLeftOf > 0 ? visibleTreesLeftOf : col) *
                    (visibleTreesBelow > 0 ? visibleTreesBelow : rows - (row + 1)) *
                    (visibleTreesRightOf > 0 ? visibleTreesRightOf : cols - (col + 1));

                if (scenicScore > highestScenicScore)
                {
                    highestScenicScore = scenicScore;
                }
            }
        }

        return highestScenicScore;
    }

    private int[][] GetTrees(string[] input)
    {
        return input
            .Select(row => row
                .AsEnumerable()
                .Select(item => int.Parse(item.ToString()))
                .ToArray())
            .ToArray();
    }
}
