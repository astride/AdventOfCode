using Common.Interfaces;

namespace Year2022;

public class Day10Solver : IPuzzleSolver
{
    public string Title => "";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;
    
    private static readonly Dictionary<string, string> InputReplacements = new()
    {
        ["addx"] = "2",
        ["noop"] = "1 0",
    };

    public void SolvePuzzle(string[] input)
    {
        Part1Solution = GetPart1Solution(input).ToString();

        DrawRctDisplayForPart2(input);
    }

    private static int GetPart1Solution(IEnumerable<string> input)
    {
        var interestingCycles = new[] { 20, 60, 100, 140, 180, 220 };

        var interestingCycleIndexMax = interestingCycles.Length - 1;

        var interestingCycleIndex = 0;
        var interestingCycle = interestingCycles[interestingCycleIndex];

        var cycle = 0;
        var x = 1;

        var interestingData = new Dictionary<int, int>();

        foreach (var instruction in input.Select(GetDecodedInstruction))
        {
            cycle += instruction.CycleDiff;

            if (cycle >= interestingCycle)
            {
                interestingData.Add(interestingCycle, x);

                interestingCycleIndex++;

                if (interestingCycleIndex > interestingCycleIndexMax)
                {
                    break;
                }

                interestingCycle = interestingCycles[interestingCycleIndex];
            }

            x += instruction.XDiff;
        }

        var signalStrength = interestingData
            .Select(data => data.Key * data.Value)
            .Sum();

        return signalStrength;
    }

    private static (int CycleDiff, int XDiff) GetDecodedInstruction(string instruction)
    {
        var wordToReplace = instruction.Split(' ')[0];

        var decoded = instruction
            .Replace(wordToReplace, InputReplacements[wordToReplace])
            .Split(' ');
        
        return (int.Parse(decoded[0]), int.Parse(decoded[1]));
    }

    private static void DrawRctDisplayForPart2(IEnumerable<string> input)
    {
        const int crtWidth = 40;

        var rctIndex = 0;
        var x = 1;
       
        int GetRelativeRctIndex() => rctIndex % crtWidth;
        int GetSpritePositionStart() => x - 1;
        int GetSpritePositionEnd() => x + 1;
        
        var rctInput = new List<bool>();

        foreach (var instruction in input.Select(GetDecodedInstruction))
        {
            foreach (var _ in Enumerable.Range(1, instruction.CycleDiff))
            {
                var relativeRctIndex = GetRelativeRctIndex();

                var spotIsEmpty =
                    relativeRctIndex < GetSpritePositionStart() ||
                    relativeRctIndex > GetSpritePositionEnd();

                rctInput.Add(!spotIsEmpty);
                rctIndex++;
            }

            x += instruction.XDiff;
        }

        // Using ' ' rather than '.' to make it easier to read the displayed letters
        var rctDisplay = rctInput
            .Chunk(crtWidth)
            .Select(chunk => chunk.Select(lit => lit ? '#' : ' '));

        foreach (var row in rctDisplay)
        {
            Console.WriteLine(string.Join(string.Empty, row));
        }
    }
}
