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
        
        var elfCount = elfPositions.Count;

        var considerationIndices = Enumerable.Range(0, ConsiderationCount).ToArray();

        for (var round = 0; round < rounds; round++)
        {
            var elvesThatConsiderMoving = elfPositions
                .Where(elf => AdjacentCells
                    .Any(cellShift => elfPositions
                        .Contains(new ElfCoor(elf.X + cellShift.X, elf.Y + cellShift.Y))))
                .ToList();

            var orderedConsiderations = considerationIndices
                .Select(i => (round + i) % ConsiderationCount)
                .Select(iOrdered => Considerations[iOrdered])
                .ToList();

            var consideredNewPosByCurrentPos = new Dictionary<ElfCoor, ElfCoor>();

            // First half of round: Figure out where elves consider moving to
            foreach (var elf in elvesThatConsiderMoving)
            {
                var firstFreeDirection = orderedConsiderations
                    .FirstOrDefault(direction => direction
                        .All(cellShift => !elfPositions
                            .Contains(new ElfCoor(elf.X + cellShift.X, elf.Y + cellShift.Y))));

                if (firstFreeDirection == default)
                {
                    // Elf cannot move
                    continue;
                }

                var consideredMovement = firstFreeDirection.First();

                consideredNewPosByCurrentPos[elf] = new ElfCoor(elf.X + consideredMovement.X, elf.Y + consideredMovement.Y);
            }

            var elvesThatConsiderMovingThatActuallyCanMove = consideredNewPosByCurrentPos
                .GroupBy(kvp => kvp.Value)
                .Where(gr => !gr.Skip(1).Any())
                .ToDictionary(gr => gr.Single().Key, gr => gr.Key);

            // Second half of round: Move the elves that actually can move
            foreach (var movingElf in elvesThatConsiderMovingThatActuallyCanMove)
            {
                elfPositions.Remove(movingElf.Key);
                elfPositions.Add(movingElf.Value);
            }
        }
        
        var xPositions = elfPositions.Select(pos => pos.X).Distinct().ToArray();
        var yPositions = elfPositions.Select(pos => pos.Y).Distinct().ToArray();

        var width = 1 + xPositions.Max() - xPositions.Min();
        var height = 1 + yPositions.Max() - yPositions.Min();

        var smallestRectangleArea = width * height;
        
        var emptyGroundTileCount = smallestRectangleArea - elfCount;
        
        return emptyGroundTileCount;
    }

    public object GetPart2Solution(string[] input)
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
        
        var considerationIndices = Enumerable.Range(0, ConsiderationCount).ToArray();

        var round = -1;

        while (true)
        {
            round++;
            
            var elvesThatConsiderMoving = elfPositions
                .Where(elf => AdjacentCells
                    .Any(cellShift => elfPositions
                        .Contains(new ElfCoor(elf.X + cellShift.X, elf.Y + cellShift.Y))))
                .ToHashSet();

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

            var consideredNewPosByElfPos = new Dictionary<ElfCoor, ElfCoor>();

            // First half of round: Figure out where elves consider moving to
            foreach (var elf in elvesThatConsiderMoving)
            {
                var firstFreeDirection = orderedConsiderations
                    .FirstOrDefault(direction => direction
                        .All(cellShift => !elfPositions
                            .Contains(new ElfCoor(elf.X + cellShift.X, elf.Y + cellShift.Y))));

                if (firstFreeDirection == default)
                {
                    // Elf cannot move
                    continue;
                }

                var consideredMovement = firstFreeDirection.First();

                consideredNewPosByElfPos[elf] = new ElfCoor(elf.X + consideredMovement.X, elf.Y + consideredMovement.Y);
            }

            var elvesThatConsiderMovingThatActuallyCanMove = consideredNewPosByElfPos
                .GroupBy(kvp => kvp.Value)
                .Where(gr => !gr.Skip(1).Any())
                .ToDictionary(gr => gr.Single().Key, gr => gr.Key);

            // Second half of round: Move the elves that actually can move
            foreach (var movingElf in elvesThatConsiderMovingThatActuallyCanMove)
            {
                elfPositions.Remove(movingElf.Key);
                elfPositions.Add(movingElf.Value);
            }
        }
    }
}

internal record ElfCoor(int X, int Y);
