using Common.Interfaces;

namespace Year2022;

public class Day04Solver : IPuzzleSolver
{
    public string Title => "Camp Cleanup";

    public object? Part1Solution { get; set; }
    public object? Part2Solution { get; set; }

    public object GetPart1Solution(string[] input, bool isExampleInput)
    {
        var pairs = GetPairs(input);
        
        var fullyOverlappingPairs = pairs
            .Where(IsFullyOverlappingPair)
            .Count();
        
        return fullyOverlappingPairs;
    }

    public object GetPart2Solution(string[] input, bool isExampleInput)
    {
        var pairs = GetPairs(input);
        
        var overlappingPairs = pairs
            .Where(IsOverlappingPair)
            .Count();
        
        return overlappingPairs;
    }

    private static IEnumerable<ElfPair> GetPairs(string[] input)
    {
        return input
            .Select(GetPair)
            .ToList();
    }

    private static bool IsFullyOverlappingPair(ElfPair pair)
    {
        if (!IsOverlappingPair(pair))
        {
            return false;
        }

        if (pair.First.FromSection < pair.Second.FromSection &&
            pair.First.ToSection < pair.Second.ToSection)
        {
            return false;
        }

        if (pair.First.FromSection > pair.Second.FromSection &&
            pair.First.ToSection > pair.Second.ToSection)
        {
            return false;
        }

        return true;
    }

    private static bool IsOverlappingPair(ElfPair pair)
    {
        if (pair.First.ToSection < pair.Second.FromSection)
        {
            return false;
        }

        if (pair.First.FromSection > pair.Second.ToSection)
        {
            return false;
        }

        return true;
    }

    private static ElfPair GetPair(string pairDescription)
    {
        var pair = pairDescription.Split(',');

        var sectionRangePair = pair
            .Select(sectionRangeForElf => sectionRangeForElf
                .Split('-')
                .Select(int.Parse)
                .ToList())
            .Select(sectionRange => new SectionRange(sectionRange[0], sectionRange[1]))
            .ToList();

        return new ElfPair(sectionRangePair[0], sectionRangePair[1]);
    }
}

internal record ElfPair(
    SectionRange First,
    SectionRange Second);

internal record SectionRange(
    int FromSection,
    int ToSection);