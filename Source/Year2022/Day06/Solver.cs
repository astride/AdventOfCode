using Common.Interfaces;

namespace Year2022;

public class Day06Solver : IPuzzleSolver
{
    public string Title => "Tuning Trouble";

    public object? Part1Solution { get; set; }
    public object? Part2Solution { get; set; }

    public object GetPart1Solution(string[] input, bool isExampleInput)
    {
        var datastreamBuffer = GetDatastreamBuffer(input);
        
        return DetectStartOfPacketMarkerEndIndex(datastreamBuffer);
    }

    public object GetPart2Solution(string[] input, bool isExampleInput)
    {
        var datastreamBuffer = GetDatastreamBuffer(input);
        
        return DetectStartOfMessageMarkerEndIndex(datastreamBuffer);
    }
    
    private static string GetDatastreamBuffer(string[] input) => input.Single();

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
