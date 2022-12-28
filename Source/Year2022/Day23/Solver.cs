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
    // East/West direction: Y axis (West: -) (East: +)
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

    private static readonly ElfCoor[][] Considerations =
    {
        NorthConsiderations,
        SouthConsiderations,
        WestConsiderations,
        EastConsiderations
    };

    private static readonly int ConsiderationCount = Considerations.Length;

    private static readonly ElfCoor[] AdjacentCells =
    {
        North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest
    };
    
    public object GetPart1Solution(string[] input)
    {
        const int rounds = 10;

        var elves = GetElfPositions(input);
        
        var elfCount = elves.Count;

        var considerationIndices = Enumerable.Range(0, ConsiderationCount).ToArray();

        for (var round = 0; round < rounds; round++)
        {
            var elvesThatConsiderMoving = GetElvesThatConsiderMoving(elves);

            // TODO
            var orderedConsiderations = considerationIndices
                .Select(i => (round + i) % ConsiderationCount)
                .Select(iOrdered => Considerations[iOrdered])
                .ToList();

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

            var newPosByOldPosOfMovingElves = GetNewPosByOldPosOfElvesThatCanActuallyMove(consideredNewPosByCurrentPos);

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
        
        var emptyGroundTileCount = smallestRectangleArea - elfCount;
        
        return emptyGroundTileCount;
    }

    private static HashSet<ElfCoor> GetElfPositions(string[] input)
    {
        var elfGrid = input
            .Select(line => line
                .Select(cell => cell == Elf)
                .ToList())
            .ToList();
        
        var elfPositions = elfGrid
            .Select((line, row) => line
                .Select((isElf, col) => (IsElf: isElf, Row: row, Col: col)))
            .SelectMany(_ => _)
            .Where(coordinate => coordinate.IsElf)
            .Select(coordinate => new ElfCoor(coordinate.Row, coordinate.Col))
            .ToHashSet();

        return elfPositions;
    }

    private List<ElfCoor> GetElvesThatConsiderMoving(HashSet<ElfCoor> elves)
    {
        var elvesThatConsiderMoving = elves
            .Where(elf => AdjacentCells
                .Any(cellShift => elves
                    .Contains(new ElfCoor(elf.X + cellShift.X, elf.Y + cellShift.Y))))
            .ToList();

        return elvesThatConsiderMoving;
    }

    private ElfCoor? GetConsideredMovementIfExists(List<ElfCoor[]> orderedConsiderations, HashSet<ElfCoor> elves, ElfCoor elf)
    {
        var firstFreeDirection = orderedConsiderations
            .FirstOrDefault(direction => direction
                .All(cellShift => !elves
                    .Contains(new ElfCoor(elf.X + cellShift.X, elf.Y + cellShift.Y))));

        return firstFreeDirection?.FirstOrDefault();
    }

    private static Dictionary<ElfCoor, ElfCoor> GetNewPosByOldPosOfElvesThatCanActuallyMove(Dictionary<ElfCoor, ElfCoor> consideredNewPosByCurrentPos)
    {
        var elves = consideredNewPosByCurrentPos
            .GroupBy(kvp => kvp.Value)
            .Where(gr => !gr.Skip(1).Any())
            .ToDictionary(gr => gr.Single().Key, gr => gr.Key);

        return elves;
    }

    public object GetPart2Solution(string[] input)
    {
        var elves = GetElfPositions(input);
        
        var considerationIndices = Enumerable.Range(0, ConsiderationCount).ToArray();

        var round = -1;

        while (true)
        {
            round++;

            var elvesThatConsiderMoving = GetElvesThatConsiderMoving(elves);

            if (!elvesThatConsiderMoving.Any())
            {
                return round + 1;
            }

            // TODO (also in part 1) Create list with all four order possibilities and get by index,
            // rather than generating it every round
            var orderedConsiderations = considerationIndices
                .Select(i => (round + i) % ConsiderationCount)
                .Select(iOrdered => Considerations[iOrdered])
                .ToList();

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

            var newPosByOldPosOfMovingElves = GetNewPosByOldPosOfElvesThatCanActuallyMove(consideredNewPosByCurrentPos);

            // Second half of round: Move the elves that actually can move
            foreach (var (oldPos, newPos) in newPosByOldPosOfMovingElves)
            {
                elves.Remove(oldPos);
                elves.Add(newPos);
            }
        }
    }
}

internal record ElfCoor(int X, int Y);
