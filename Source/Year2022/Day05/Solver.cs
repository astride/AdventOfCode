using Common.Interfaces;

namespace Year2022;

public class Day05Solver : IPuzzleSolver
{
    public string Title => "";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        Part1Solution = SolvePart1(input);
        Part2Solution = SolvePart2(input);
    }

    private static string SolvePart1(IReadOnlyList<string> input)
    {
        var crateCount = input
            .Single(line => line.StartsWith(" 1"))
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .Max();

        bool isInRearrangement = false;

        var inputStacks = new List<Stack<char>>(crateCount);
        var rearrangements = new List<int[]>();

        foreach (var line in input)
        {
            if (line.StartsWith(" 1"))
            {
                isInRearrangement = true;
                continue;
            }

            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            if (isInRearrangement)
            {
                rearrangements.Add(line
                    .Split(' ')
                    .Where(x => int.TryParse(x, out _))
                    .Select(int.Parse)
                    .ToArray());
                
                continue;
            }

            if (!inputStacks.Any())
            {
                for (var i = 0; i < crateCount; i++)
                {
                    inputStacks.Add(new Stack<char>());
                }
            }

            for (var i = 0; i < crateCount; i++)
            {
                var charIndex = 1 + i * 4;
                var ch = line[charIndex];

                if (ch != ' ')
                {
                    inputStacks[i].Push(ch);
                }
            }
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
            for (var i = 0; i < rearrangement[0]; i++)
            {
                stacks[rearrangement[2] - 1].Push(stacks[rearrangement[1] - 1].Pop());
            }
        }

        var topItems = new List<char>();

        foreach (var stack in stacks)
        {
            topItems.Add(stack.Peek());
        }
        
        return new string(topItems.ToArray());
    }

    private static string SolvePart2(IEnumerable<string> input)
    {
        var crateCount = input
            .Single(line => line.StartsWith(" 1"))
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .Max();

        bool isInRearrangement = false;

        var inputStacks = new List<Stack<char>>(crateCount);
        var rearrangements = new List<int[]>();

        foreach (var line in input)
        {
            if (line.StartsWith(" 1"))
            {
                isInRearrangement = true;
                continue;
            }

            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            if (isInRearrangement)
            {
                rearrangements.Add(line
                    .Split(' ')
                    .Where(x => int.TryParse(x, out _))
                    .Select(int.Parse)
                    .ToArray());
                
                continue;
            }

            if (!inputStacks.Any())
            {
                for (var i = 0; i < crateCount; i++)
                {
                    inputStacks.Add(new Stack<char>());
                }
            }

            for (var i = 0; i < crateCount; i++)
            {
                var charIndex = 1 + i * 4;
                var ch = line[charIndex];

                if (ch != ' ')
                {
                    inputStacks[i].Push(ch);
                }
            }
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

        var topItems = new List<char>();

        foreach (var stack in stacks)
        {
            topItems.Add(stack.Peek());
        }
        
        return new string(topItems.ToArray());
    }
}
