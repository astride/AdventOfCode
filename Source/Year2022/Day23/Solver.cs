using Common.Interfaces;

namespace Year2022;

public class Day23Solver : IPuzzleSolver
{
    public string Title => "Unstable Diffusion";

    public object? Part1Solution { get; set; }
    public object? Part2Solution { get; set; }

    private const char Elf = '#';
    
    // Due to input parsing:
    // North/South direction: X axis (North: -) (South: +)
    // West/East direction: Y axis (West: -) (East: +)
    private static readonly ElfCoor North = new(-1, 0);
    private static readonly ElfCoor South = new(1, 0);
    private static readonly ElfCoor West = new(0, -1);
    private static readonly ElfCoor East = new(0, 1);

    private static readonly ElfCoor NorthWest = new(-1, -1);
    private static readonly ElfCoor NorthEast = new(-1, 1);
    private static readonly ElfCoor SouthWest = new(1, -1);
    private static readonly ElfCoor SouthEast = new(1, 1);

    private static readonly ElfCoor[] NorthConsiderations = { North, NorthWest, NorthEast };
    private static readonly ElfCoor[] SouthConsiderations = { South, SouthWest, SouthEast };
    private static readonly ElfCoor[] WestConsiderations = { West, NorthWest, SouthWest };
    private static readonly ElfCoor[] EastConsiderations = { East, NorthEast, SouthEast };

    private static readonly ElfCoor[][] ConsiderationsStartingWithNorth =
    {
        NorthConsiderations, SouthConsiderations, WestConsiderations, EastConsiderations
    };

    private static readonly ElfCoor[][] ConsiderationsStartingWithSouth =
    {
        SouthConsiderations, WestConsiderations, EastConsiderations, NorthConsiderations
    };

    private static readonly ElfCoor[][] ConsiderationsStartingWithWest =
    {
        WestConsiderations, EastConsiderations, NorthConsiderations, SouthConsiderations
    };

    private static readonly ElfCoor[][] ConsiderationsStartingWithEast =
    {
        EastConsiderations, NorthConsiderations, SouthConsiderations, WestConsiderations
    };

    private static readonly ElfCoor[][][] OrderedConsiderationsInOrder =
    {
        ConsiderationsStartingWithNorth,
        ConsiderationsStartingWithSouth,
        ConsiderationsStartingWithWest,
        ConsiderationsStartingWithEast,
    };

    private static readonly int OrderedConsiderationsCount = OrderedConsiderationsInOrder.Length;

    private static readonly ElfCoor[] AdjacentCells =
    {
        North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest
    };
    
    public object GetPart1Solution(string[] input, bool isExampleInput)
    {
        const int rounds = 10;

        var elves = GetElfPositions(input);
        
        // for (var round = 0; round < rounds; round++)
        foreach (var round in Enumerable.Range(1, rounds))
        {
            var elvesThatConsiderMoving = GetElvesThatConsiderMoving(elves);

            var newPosByOldPosOfMovingElves = GetNewPosByCurrentPosOfMovingElves(elves, elvesThatConsiderMoving, round);

            // Second half of round: Move the elves that actually can move
            foreach (var (oldPos, newPos) in newPosByOldPosOfMovingElves)
            {
                elves.Remove(oldPos);
                elves.Add(newPos);
            }
        }
        
        var xPositions = elves.Select(pos => pos.X).Distinct().ToArray();
        var yPositions = elves.Select(pos => pos.Y).Distinct().ToArray();

        var width = 1 + xPositions.Max() - xPositions.Min();
        var height = 1 + yPositions.Max() - yPositions.Min();

        var smallestRectangleArea = width * height;
        
        var emptyGroundTileCount = smallestRectangleArea - elves.Count;
        
        return emptyGroundTileCount;
    }

    public object GetPart2Solution(string[] input, bool isExampleInput)
    {
        var elves = GetElfPositions(input);
        
        var round = 0;

        while (true)
        {
            round++;

            var elvesThatConsiderMoving = GetElvesThatConsiderMoving(elves);

            if (!elvesThatConsiderMoving.Any())
            {
                return round;
            }

            var newPosByOldPosOfMovingElves = GetNewPosByCurrentPosOfMovingElves(elves, elvesThatConsiderMoving, round);

            // Second half of round: Move the elves that actually can move
            foreach (var (oldPos, newPos) in newPosByOldPosOfMovingElves)
            {
                elves.Remove(oldPos);
                elves.Add(newPos);
            }
        }
    }

    private static HashSet<ElfCoor> GetElfPositions(string[] input)
    {
        var elfGrid = input
            .Select(line => line
                .Select(cell => cell == Elf)
                .ToList())
            .ToList();
        
        var elfPositions = elfGrid
            .SelectMany((line, row) => line
                .Select((isElf, col) => (IsElf: isElf, Row: row, Col: col)))
            .Where(coordinate => coordinate.IsElf)
            .Select(coordinate => new ElfCoor(coordinate.Row, coordinate.Col))
            .ToHashSet();

        return elfPositions;
    }

    private static List<ElfCoor> GetElvesThatConsiderMoving(HashSet<ElfCoor> elves)
    {
        var elvesThatConsiderMoving = elves
            .Where(elf => AdjacentCells
                .Any(cellShift => elves
                    .Contains(new ElfCoor(elf.X + cellShift.X, elf.Y + cellShift.Y))))
            .ToList();

        return elvesThatConsiderMoving;
    }

    private static ElfCoor? GetConsideredMovementIfExists(ElfCoor[][] orderedConsiderations, HashSet<ElfCoor> elves, ElfCoor elf)
    {
        var firstFreeDirection = orderedConsiderations
            .FirstOrDefault(direction => direction
                .All(cellShift => !elves
                    .Contains(new ElfCoor(elf.X + cellShift.X, elf.Y + cellShift.Y))));

        return firstFreeDirection?.FirstOrDefault();
    }

    private static Dictionary<ElfCoor, ElfCoor> GetNewPosByCurrentPosOfElvesThatCanActuallyMove(Dictionary<ElfCoor, ElfCoor> consideredNewPosByCurrentPos)
    {
        var elfShifts = consideredNewPosByCurrentPos
            .GroupBy(kvp => kvp.Value)
            .Where(gr => !gr.Skip(1).Any())
            .ToDictionary(gr => gr.Single().Key, gr => gr.Key);

        return elfShifts;
    }

    private static Dictionary<ElfCoor, ElfCoor> GetNewPosByCurrentPosOfMovingElves(HashSet<ElfCoor> elves, List<ElfCoor> elvesThatConsiderMoving, int round)
    {
        var orderedConsiderations = OrderedConsiderationsInOrder[(round - 1) % OrderedConsiderationsCount];

        var consideredNewPosByCurrentPos = new Dictionary<ElfCoor, ElfCoor>();

        // First half of round: Figure out where elves consider moving to
        foreach (var elf in elvesThatConsiderMoving)
        {
            var consideredMovement = GetConsideredMovementIfExists(orderedConsiderations, elves, elf);

            if (consideredMovement == null)
            {
                // Elf cannot move
                continue;
            }

            consideredNewPosByCurrentPos[elf] = new ElfCoor(elf.X + consideredMovement.X, elf.Y + consideredMovement.Y);
        }

        var newPosByCurrentPosOfMovingElves = GetNewPosByCurrentPosOfElvesThatCanActuallyMove(consideredNewPosByCurrentPos);

        return newPosByCurrentPosOfMovingElves;
    }
}

internal record ElfCoor(int X, int Y);
