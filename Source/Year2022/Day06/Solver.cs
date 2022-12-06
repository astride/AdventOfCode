using Common.Interfaces;

namespace Year2022;

public class Day06Solver : IPuzzleSolver
{
    public string Title => "Tuning Trouble";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        var datastreamBuffer = input.Single();
        
        Part1Solution = SolvePart1(datastreamBuffer).ToString();
        Part2Solution = SolvePart2(datastreamBuffer).ToString();
    }

    private static int SolvePart1(string datastreamBuffer)
    {
        return DetectStartOfPacketMarkerEndIndex(datastreamBuffer);
    }

    private static int SolvePart2(string datastreamBuffer)
    {
        return DetectStartOfMessageMarkerEndIndex(datastreamBuffer);
    }

    private static int DetectStartOfPacketMarkerEndIndex(string datastreamBuffer)
    {
        const int sequenceSize = 4;

        return DetectFirstEndIndexOfDistinctCharacterSequence(datastreamBuffer, sequenceSize);
    }

    private static int DetectStartOfMessageMarkerEndIndex(string datastreamBuffer)
    {
        const int sequenceSize = 14;

        return DetectFirstEndIndexOfDistinctCharacterSequence(datastreamBuffer, sequenceSize);
    }

    private static int DetectFirstEndIndexOfDistinctCharacterSequence(string datastreamBuffer, int sequenceSize)
    {
        for (var i = 0; i < datastreamBuffer.Length - sequenceSize; i++)
        {
            var iEnd = i + sequenceSize;
            
            if (datastreamBuffer[i..iEnd].Distinct().Count() == sequenceSize)
            {
                return iEnd;
            }
        }
        
        return -1;
    }
}
