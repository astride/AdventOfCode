using Common.Interfaces;

namespace Year2022;

public class Day03Solver : IPuzzleSolver
{
    public string Title => "Rucksack Reorganization";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        var puzzleInput = input.ToList();

        Part1Solution = SolvePart1(puzzleInput).ToString();
        Part2Solution = SolvePart2(puzzleInput).ToString();
    }

    private static int SolvePart1(IEnumerable<string> rucksackContents)
    {
        var contentByCompartments = rucksackContents
            .Select(rucksackContent => rucksackContent
                .Chunk(rucksackContent.Length / 2)
                .Select(contentInCompartment => new string(contentInCompartment))
                .ToList());

        var misplacedItemTypePerRucksack = contentByCompartments
            .Select(GetCommonItem);
        
        return misplacedItemTypePerRucksack.Sum(GetPriority);
    }

    private static int SolvePart2(IEnumerable<string> rucksackContents)
    {
        var elfGroups = rucksackContents.Chunk(3).ToList();

        var badgePerElfGroup = elfGroups.Select(GetCommonItem);
        
        return badgePerElfGroup.Sum(GetPriority);
    }
    
    private static char GetCommonItem(IReadOnlyCollection<IEnumerable<char>> collections)
    {
        var commonItems = collections.First();

        foreach (var collection in collections.Skip(1))
        {
            commonItems = commonItems.Intersect(collection);
        }

        return commonItems.Single();
    }
    
    private static int GetPriority(char item)
    {
        const int alphabetLength = 26;
        const int offsetLowercase = 'a' - 1;
        const int offsetUppercase = 'A' - 1 - alphabetLength;

        if (char.IsLower(item))
        {
            return item - offsetLowercase;
        }

        if (char.IsUpper(item))
        {
            return item - offsetUppercase;
        }

        return 0;
    }
}
