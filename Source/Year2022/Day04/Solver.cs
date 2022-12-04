using Common.Interfaces;

namespace Year2022;

public class Day04Solver : IPuzzleSolver
{
    public string Title => "Camp Cleanup";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        Part1Solution = SolvePart1(input).ToString();
        Part2Solution = SolvePart2(input).ToString();
    }

    private static int SolvePart1(IEnumerable<string> input)
    {
        var overlappingPairs = input
            .Select(IsFullyOverlappingPair)
            .Count(overlappingPair => overlappingPair);
        
        return overlappingPairs;
    }

    private static int SolvePart2(IEnumerable<string> input)
    {
        var partiallyOverlappingPairs = input
            .Select(IsPartiallyOverlappingPair)
            .Count(overlappingPair => overlappingPair);
        
        return partiallyOverlappingPairs;
    }

    private static bool IsFullyOverlappingPair(string pairStr)
    {
        var detailedPair = GetDetailedPair(pairStr);

        if (detailedPair[0][0] >= detailedPair[1][0] &&
            detailedPair[0][1] <= detailedPair[1][1])
        {
            return true;
        }

        if (detailedPair[0][0] <= detailedPair[1][0] &&
            detailedPair[0][1] >= detailedPair[1][1])
        {
            return true;
        }

        return false;
    }

    private static bool IsPartiallyOverlappingPair(string pairStr)
    {
        if (IsFullyOverlappingPair(pairStr))
        {
            return true;
        }
        
        var detailedPair = GetDetailedPair(pairStr);

        if (detailedPair[0][0] <= detailedPair[1][1] &&
            detailedPair[0][0] >= detailedPair[1][0])
        {
            return true;
        }
        
        if (detailedPair[0][1] <= detailedPair[1][1] &&
            detailedPair[0][1] >= detailedPair[1][0])
        {
            return true;
        }
        
        return false;
    }

    private static List<List<int>> GetDetailedPair(string pairStr)
    {
        var pair = pairStr.Split(',');

        var detailedPair = pair
            .Select(pairedElf => pairedElf
                .Split('-')
                .Select(int.Parse)
                .ToList())
            .ToList();

        return detailedPair;
    }
}
