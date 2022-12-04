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
        var overlappingPairs = pairs
            .Select(IsFullyOverlappingPair)
            .Count(overlappingPair => overlappingPair);
        
        return overlappingPairs;
    }

    private static int SolvePart2(IEnumerable<ElfPair> input)
    {
        var partiallyOverlappingPairs = input
            .Select(IsPartiallyOverlappingPair)
            .Count(overlappingPair => overlappingPair);
        
        return partiallyOverlappingPairs;
    }

    private static bool IsFullyOverlappingPair(ElfPair pair)
    {
        if (pair.First.FromSection >= pair.Second.FromSection &&
            pair.First.ToSection <= pair.Second.ToSection)
        {
            return true;
        }

        if (pair.First.FromSection <= pair.Second.FromSection &&
            pair.First.ToSection >= pair.Second.ToSection)
        {
            return true;
        }

        return false;
    }

    private static bool IsPartiallyOverlappingPair(ElfPair pair)
    {
        if (IsFullyOverlappingPair(pair))
        {
            return true;
        }

        if (pair.First.FromSection <= pair.Second.ToSection &&
            pair.First.FromSection >= pair.Second.FromSection)
        {
            return true;
        }
        
        if (pair.First.ToSection <= pair.Second.ToSection &&
            pair.First.ToSection >= pair.Second.FromSection)
        {
            return true;
        }
        
        return false;
    }

    private static ElfPair GetPair(string pairDescription)
    {
        var pair = pairDescription.Split(',');

        var sectionRange = pair
            .Select(pairedElf => pairedElf
                .Split('-')
                .Select(int.Parse)
                .ToList())
            .Select(sections => new SectionRange(sections[0], sections[1]))
            .ToList();

        return new ElfPair(sectionRange[0], sectionRange[1]);
    }
}

internal record ElfPair(
    SectionRange First,
    SectionRange Second);

internal record SectionRange(
    int FromSection,
    int ToSection);