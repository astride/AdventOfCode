using Common.Interfaces;

namespace Year2022;

public class Day05Solver : IPuzzleSolver
{
    public string Title => "Supply Stacks";

    public object? Part1Solution { get; set; }
    public object? Part2Solution { get; set; }

    private const int DistanceBetweenStacks = 4;
    private const int OffsetToFirstStack = 1;
    private const char EmptyStackItem = ' ';

    public object GetPart1Solution(string[] input)
    {
        return GetResultingTopItems(input, false);
    }

    public object GetPart2Solution(string[] input)
    {
        return GetResultingTopItems(input, true);
    }

    private static string GetResultingTopItems(string[] input, bool moveMultipleCrates)
    {
        var crateCount = input
            .Single(IsStackIndexLine)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .Max();

        var inputStacks = new List<Stack<char>>(crateCount);

        for (var i = 0; i < crateCount; i++)
        {
            inputStacks.Add(new Stack<char>());
        }

        foreach (var startingStackLine in input.TakeWhile(line => !IsStackIndexLine(line)))
        {
            for (var i = 0; i < crateCount; i++)
            {
                var itemIndex = i * DistanceBetweenStacks + OffsetToFirstStack;
                var item = startingStackLine[itemIndex];

                if (item != EmptyStackItem)
                {
                    inputStacks[i].Push(item);
                }
            }
        }
        
        var rearrangements = new List<int[]>();

        foreach (var rearrangement in input.SkipWhile(line => !IsRearrangementProcedure(line)))
        {
            rearrangements.Add(rearrangement
                .Split(' ')
                .Where(x => int.TryParse(x, out _))
                .Select(int.Parse)
                .ToArray());
        }

        var stacks = new List<Stack<char>>();

        for (var i = 0; i < crateCount; i++)
        {
            stacks.Add(new Stack<char>());

            foreach (var crate in inputStacks[i])
            {
                stacks[i].Push(crate);
            }
        }

        foreach (var rearrangement in rearrangements)
        {
            var crateCountToMove = rearrangement[0];
            var sourceStackIndex = rearrangement[1] - 1;
            var targetStackIndex = rearrangement[2] - 1;
            
            if (moveMultipleCrates)
            {
                var temporaryStack = new Stack<char>();

                for (var i = 0; i < crateCountToMove; i++)
                {
                    var crateToMove = stacks[sourceStackIndex].Pop();
                    
                    temporaryStack.Push(crateToMove);
                }

                foreach (var crate in temporaryStack)
                {
                    var targetStack = stacks[targetStackIndex];
                    
                    targetStack.Push(crate);
                }
            }
            else
            {
                for (var i = 0; i < crateCountToMove; i++)
                {
                    var crateToMove = stacks[sourceStackIndex].Pop();
                    var targetStack = stacks[targetStackIndex];
            
                    targetStack.Push(crateToMove);
                }
            }
        }

        var topCrates = stacks
            .Select(stack => stack.Peek())
            .ToArray();

        return new string(topCrates);
    }

    private static bool IsStackIndexLine(string line) => line.StartsWith(" 1");
    private static bool IsRearrangementProcedure(string line) => line.StartsWith("move");
}
