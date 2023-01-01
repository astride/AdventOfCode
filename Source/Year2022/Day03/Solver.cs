using Common.Interfaces;

namespace Year2022;

public class Day03Solver : IPuzzleSolver
{
    public string Title => "Rucksack Reorganization";

    public object? Part1Solution { get; set; }
    public object? Part2Solution { get; set; }

    public object GetPart1Solution(string[] input)
    {
        var contentByCompartments = input
            .Select(content =>
                new[]
                {
                    content[..(content.Length / 2)],
                    content[(content.Length / 2)..],
                });

        var misplacedItemTypePerRucksack = contentByCompartments
            .Select(GetCommonItem);
        
        return misplacedItemTypePerRucksack.Sum(GetPriority);
    }

    public object GetPart2Solution(string[] input)
    {
        var elfGroups = input.Chunk(3).ToList();

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
