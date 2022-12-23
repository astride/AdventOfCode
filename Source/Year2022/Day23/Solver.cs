using Common.Interfaces;
using Common.Models;

namespace Year2022;

public class Day23Solver : IPuzzleSolver
{
    public string Title => "Unstable Diffusion";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    private const char Elf = '#';
    
    // Due to input parsing:
    // North/South direction: X axis (North: -) (South: +)
    // East/West direction: Y axis (West: -) (East: +)
    private static readonly Coordinate North = new(-1, 0);
    private static readonly Coordinate South = new(1, 0);
    private static readonly Coordinate West = new(0, -1);
    private static readonly Coordinate East = new(0, 1);

    private static readonly Coordinate NorthWest = new(-1, -1);
    private static readonly Coordinate NorthEast = new(-1, 1);
    private static readonly Coordinate SouthWest = new(1, -1);
    private static readonly Coordinate SouthEast = new(1, 1);

    private static readonly Coordinate[] NorthConsiderations = { North, NorthWest, NorthEast };
    private static readonly Coordinate[] SouthConsiderations = { South, SouthWest, SouthEast };
    private static readonly Coordinate[] WestConsiderations = { West, NorthWest, SouthWest };
    private static readonly Coordinate[] EastConsiderations = { East, NorthEast, SouthEast };

    private static readonly Coordinate[][] Considerations =
    {
        NorthConsiderations,
        SouthConsiderations,
        WestConsiderations,
        EastConsiderations
    };

    private static readonly int ConsiderationCount = Considerations.Length;

    private static readonly Coordinate[] AdjacentCells =
    {
        North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest
    };
    
    public void SolvePuzzle(string[] input)
    {
        Part1Solution = GetPart1Solution(input).ToString();
        Part2Solution = GetPart2Solution(input).ToString();
    }

    private static int GetPart1Solution(IEnumerable<string> input)
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
            .Select(coordinate => new Coordinate(coordinate.Row, coordinate.Col))
            .ToList();
        
        var elfCount = elfPositions.Count;

        var considerationIndices = Enumerable.Range(0, ConsiderationCount).ToArray();

        for (var round = 0; round < rounds; round++)
        {
            var elvesThatConsiderMoving = elfPositions
                .Select((pos, iElf) => (pos, iElf))
                .Where(elf => AdjacentCells
                    .Any(cellShift => elfPositions
                        .Contains(new Coordinate(elf.pos.X + cellShift.X, elf.pos.Y + cellShift.Y))))
                .ToDictionary(elf => elf.iElf, elf => elf.pos);

            var orderedConsiderations = considerationIndices
                .Select(i => (round + i) % ConsiderationCount)
                .Select(iOrdered => Considerations[iOrdered])
                .ToList();

            var consideredMovementByElfIndex = new Dictionary<int, Coordinate>();

            // First half of round: Figure out where elves consider moving to
            foreach (var elf in elvesThatConsiderMoving)
            {
                var firstFreeDirection = orderedConsiderations
                    .FirstOrDefault(direction => direction
                        .All(cellShift => !elfPositions
                            .Contains(new Coordinate(elf.Value.X + cellShift.X, elf.Value.Y + cellShift.Y))));

                if (firstFreeDirection == default)
                {
                    // Elf cannot move
                    continue;
                }

                var consideredMovement = firstFreeDirection.First();

                consideredMovementByElfIndex[elf.Key] = new Coordinate(elf.Value.X + consideredMovement.X, elf.Value.Y + consideredMovement.Y);
            }

            var elvesThatConsiderMovingThatActuallyCanMove = consideredMovementByElfIndex
                .GroupBy(kvp => kvp.Value)
                .Where(gr => !gr.Skip(1).Any())
                .ToDictionary(gr => gr.Single().Key, gr => gr.Key);

            // Second half of round: Move the elves that actually can move
            foreach (var movingElf in elvesThatConsiderMovingThatActuallyCanMove)
            {
                elfPositions[movingElf.Key] = movingElf.Value;
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

    private static int GetPart2Solution(IEnumerable<string> input)
    {
        return 0;
    }
}
