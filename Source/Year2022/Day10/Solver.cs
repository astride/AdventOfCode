using Common.Interfaces;

namespace Year2022;

public class Day10Solver : IPuzzleSolver
{
    public string Title => "Cathode-Ray Tube";

    public object? Part1Solution { get; set; }
    public object? Part2Solution { get; set; }
    
    private static readonly Dictionary<string, string> InputReplacements = new()
    {
        ["addx"] = "2",
        ["noop"] = "1 0",
    };

    public object GetPart1Solution(string[] input, bool isExampleInput)
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
            .Sum(data => data.Key * data.Value);

        return signalStrength;
    }

    public object GetPart2Solution(string[] input, bool isExampleInput)
    {
        DrawRctDisplayForPart2(input);

        return "Look at the RCT display output";
    }

    private static (int CycleDiff, int XDiff) GetDecodedInstruction(string instruction)
    {
        var wordToReplace = instruction.Split(' ')[0];

        var decoded = instruction
            .Replace(wordToReplace, InputReplacements[wordToReplace])
            .Split(' ')
            .Select(int.Parse)
            .ToArray();
        
        return (decoded[0], decoded[1]);
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
            for (var i = 0; i < instruction.CycleDiff; i++)
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

        // Using ' ' rather than '.' for not-lit pixels to make it easier to read the displayed letters
        var rctDisplay = rctInput
            .Chunk(crtWidth)
            .Select(rctRow => rctRow
                .Select(isLitPixel => isLitPixel ? '#' : ' ')
                .ToArray());
        
        foreach (var rctRow in rctDisplay)
        {
            Console.WriteLine(new string(rctRow));
        }
    }
}
