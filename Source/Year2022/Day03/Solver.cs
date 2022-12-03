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
        return item switch
        {
            'a' => 1,
            'b' => 2,
            'c' => 3,
            'd' => 4,
            'e' => 5,
            'f' => 6,
            'g' => 7,
            'h' => 8,
            'i' => 9,
            'j' => 10,
            'k' => 11,
            'l' => 12,
            'm' => 13,
            'n' => 14,
            'o' => 15,
            'p' => 16,
            'q' => 17,
            'r' => 18,
            's' => 19,
            't' => 20,
            'u' => 21,
            'v' => 22,
            'w' => 23,
            'x' => 24,
            'y' => 25,
            'z' => 26,
            'A' => 27,
            'B' => 28,
            'C' => 29,
            'D' => 30,
            'E' => 31,
            'F' => 32,
            'G' => 33,
            'H' => 34,
            'I' => 35,
            'J' => 36,
            'K' => 37,
            'L' => 38,
            'M' => 39,
            'N' => 40,
            'O' => 41,
            'P' => 42,
            'Q' => 43,
            'R' => 44,
            'S' => 45,
            'T' => 46,
            'U' => 47,
            'V' => 48,
            'W' => 49,
            'X' => 50,
            'Y' => 51,
            'Z' => 52,
            _ => 0,
        };
    }
}
