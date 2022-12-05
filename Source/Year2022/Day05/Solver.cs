using Common.Interfaces;

namespace Year2022;

public class Day05Solver : IPuzzleSolver
{
    public string Title => "Supply Stacks";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        Part1Solution = SolvePart1(input);
        Part2Solution = SolvePart2(input);
    }

    private static string SolvePart1(IReadOnlyList<string> input)
    {
        return GetResultingTopItems(input, false);
    }

    private static string SolvePart2(IEnumerable<string> input)
    {
        return GetResultingTopItems(input, true);
    }

    private static string GetResultingTopItems(IEnumerable<string> input, bool moveMultipleCrates)
    {
        var crateCount = input
            .Single(line => line.StartsWith(" 1"))
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .Max();

        var inputStacks = new List<Stack<char>>(crateCount);

        for (var i = 0; i < crateCount; i++)
        {
            inputStacks.Add(new Stack<char>());
        }

        foreach (var startingStackLine in input.TakeWhile(line => !line.StartsWith(" 1")))
        {
            for (var i = 0; i < crateCount; i++)
            {
                var charIndex = 1 + i * 4;
                var ch = startingStackLine[charIndex];

                if (ch != ' ')
                {
                    inputStacks[i].Push(ch);
                }
            }
        }
        
        var rearrangements = new List<int[]>();

        foreach (var rearrangement in input.SkipWhile(line => !line.StartsWith("move")))
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

            foreach (var item in inputStacks[i])
            {
                stacks[i].Push(item);
            }
        }

        foreach (var rearrangement in rearrangements)
        {
            if (moveMultipleCrates)
            {
                var temp = new Stack<char>();

                for (var i = 0; i < rearrangement[0]; i++)
                {
                    temp.Push(stacks[rearrangement[1] - 1].Pop());
                }

                foreach (var tempItem in temp)
                {
                    stacks[rearrangement[2] - 1].Push(tempItem);
                }
            }
            else
            {
                for (var i = 0; i < rearrangement[0]; i++)
                {
                    var crateToMove = stacks[rearrangement[1] - 1].Pop();
                    var targetStack = stacks[rearrangement[2] - 1];
            
                    targetStack.Push(crateToMove);
                }
            }
        }

        var topItems = new char[crateCount];

        for (var i = 0; i < crateCount; i++)
        {
            topItems[i] = stacks[i].Peek();
        }
        
        return new string(topItems);
    }
}
