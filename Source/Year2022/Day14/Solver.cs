using Common.Interfaces;

namespace Year2022;

public class Day14Solver : IPuzzleSolver
{
    public string Title => "";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        Part1Solution = GetPart1Solution(input).ToString();
        Part2Solution = GetPart2Solution(input).ToString();
    }

    private static int GetPart1Solution(IEnumerable<string> input)
    {
        var rockFormations = input
            .Select(line => line
                .Split(" -> ")
                .Select(AsCoordinate)
                .ToList())
            .ToList();

        var allCoors = rockFormations
            .SelectMany(line => line)
            .ToList();

        var allX = allCoors
            .Select(coors => coors.X)
            .ToList();

        var xMin = allX.Min();
        var xMax = allX.Max();

        var yMax = allCoors
            .Select(coors => coors.Y)
            .Max();

        var blockedCoors = new HashSet<(int, int)>();
        
        // Populate blockedCoors with rocks
        foreach (var line in rockFormations)
        {
            for (var i = 1; i < line.Count; i++)
            {
                var sourceCoor = line[i - 1];
                var targetCoor = line[i];

                var blockedCoor = (sourceCoor.X, sourceCoor.Y);

                blockedCoors.Add((blockedCoor.X, blockedCoor.Y));
                
                (int X, int Y) coorDiff = (targetCoor.X - sourceCoor.X, targetCoor.Y - sourceCoor.Y);

                var diffSpan = Math.Max(Math.Abs(coorDiff.X), Math.Abs(coorDiff.Y));
                
                (int X, int Y) movementPerIndex = (coorDiff.X / diffSpan, coorDiff.Y / diffSpan);

                foreach (var diff in Enumerable.Range(0, diffSpan))
                {
                    blockedCoor = (blockedCoor.X + movementPerIndex.X, blockedCoor.Y + movementPerIndex.Y);
                    blockedCoors.Add((blockedCoor.X, blockedCoor.Y));
                }
            }
        }

        var coorsBlockedByRock = blockedCoors.Count;

        bool IsAvailable(int x, int y) => !blockedCoors.Contains((x, y));

        bool SandWillFallIntoTheEndlessVoid(int xSimulated, int ySimulated) => xSimulated < xMin || xSimulated > xMax || ySimulated > yMax;
        
        // Pour sand
        
        const int sandStartingPointX = 500;
        const int sandStartingPointY = 0;

        var sandUnitCoorX = sandStartingPointX;
        var sandUnitCoorY = sandStartingPointY;

        while (true)
        {
            // Simulate falling down
            var simulatedSandCoorX = sandUnitCoorX;
            var simulatedSandCoorY = sandUnitCoorY + 1;

            if (SandWillFallIntoTheEndlessVoid(simulatedSandCoorX, simulatedSandCoorY))
            {
                break;
            }
            
            if (IsAvailable(simulatedSandCoorX, simulatedSandCoorY))
            {
                sandUnitCoorX = simulatedSandCoorX;
                sandUnitCoorY = simulatedSandCoorY;
                continue;
            }

            // Simulate falling down to the left
            simulatedSandCoorX = sandUnitCoorX - 1;
            simulatedSandCoorY = sandUnitCoorY + 1;

            if (SandWillFallIntoTheEndlessVoid(simulatedSandCoorX, simulatedSandCoorY))
            {
                break;
            }
            
            if (IsAvailable(simulatedSandCoorX, simulatedSandCoorY))
            {
                sandUnitCoorX = simulatedSandCoorX;
                sandUnitCoorY = simulatedSandCoorY;
                
                continue;
            }

            // Simulate falling down to the right
            simulatedSandCoorX = sandUnitCoorX + 1;
            simulatedSandCoorY = sandUnitCoorY + 1;

            if (SandWillFallIntoTheEndlessVoid(simulatedSandCoorX, simulatedSandCoorY))
            {
                break;
            }

            if (IsAvailable(simulatedSandCoorX, simulatedSandCoorY))
            {
                sandUnitCoorX = simulatedSandCoorX;
                sandUnitCoorY = simulatedSandCoorY;
                
                continue;
            }

            // Let unit of sand lie
            blockedCoors.Add((sandUnitCoorX, sandUnitCoorY));
            
            // Start with next unit of sand
            sandUnitCoorX = sandStartingPointX;
            sandUnitCoorY = sandStartingPointY;
        }

        var coorsBlocked = blockedCoors.Count;
        
        return coorsBlocked - coorsBlockedByRock;
    }

    private static (int X, int Y) AsCoordinate(string coorString)
    {
        var split = coorString
            .Split(',')
            .Select(int.Parse)
            .ToList();

        return (split[0], split[1]);
    }

    private static int GetPart2Solution(IEnumerable<string> input)
    {
        return 0;
    }
}
