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

    private static int SolvePart1(IEnumerable<string> input)
    {
        var contentInCompartments = input.
            Select(rucksackContent => rucksackContent.Chunk(rucksackContent.Length / 2).ToList());

        var doppelItemPerRucksack = contentInCompartments
            .Select(rucksackCompartmentContents =>
                rucksackCompartmentContents[0]
                    .Intersect(rucksackCompartmentContents[1]))
            .Select(commonItems => commonItems.Single());
        
        return doppelItemPerRucksack
            .Sum(doppelItem => doppelItem.GetPriority());
    }

    private static int SolvePart2(IEnumerable<string> input)
    {
        var groups = input.Chunk(3).ToList();

        var commonItemsPerGroup = groups
            .Select(group => group[0]
                .Intersect(group[1])
                .Intersect(group[2]));

        var commonItemPerGroup = commonItemsPerGroup
            .Select(cipg => cipg.Single())
            .ToList();
        
        return commonItemPerGroup
            .Sum(cipg => cipg.GetPriority());
    }
}

internal static class Day03Extensions
{
    public static int GetPriority(this char item)
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
