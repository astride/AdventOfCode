using Common.Interfaces;

namespace Year2022;

public class Day04Solver : IPuzzleSolver
{
    public string Title => "Camp Cleanup";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        var pairs = input
            .Select(GetPair)
            .ToList();
        
        Part1Solution = SolvePart1(pairs).ToString();
        Part2Solution = SolvePart2(pairs).ToString();
    }

    private static int SolvePart1(IEnumerable<ElfPair> pairs)
    {
        var fullyOverlappingPairs = pairs
            .Where(IsFullyOverlappingPair)
            .Count();
        
        return fullyOverlappingPairs;
    }

    private static int SolvePart2(IEnumerable<ElfPair> input)
    {
        var overlappingPairs = input
            .Where(IsOverlappingPair)
            .Count();
        
        return overlappingPairs;
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