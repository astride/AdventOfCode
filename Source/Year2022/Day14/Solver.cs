using Common.Interfaces;

namespace Year2022;

public class Day14Solver : IPuzzleSolver
{
    public string Title => "Regolith Reservoir";

    public object? Part1Solution { get; set; }
    public object? Part2Solution { get; set; }

    private const int SandStartingPointX = 500;
    private const int SandStartingPointY = 0;

    public object GetPart1Solution(string[] input, bool isExampleInput)
    {
        var rockPathPoints = GetRockPathPoints(input);
        var berockedCoordinates = GetBerockedCoordinates(rockPathPoints);

        var rockCount = berockedCoordinates.Count;

        var coordinateIsAvailable = GetCoordinateIsAvailableFunc();
        var sandWillFallIntoTheEndlessVoid = GetSandWillFallIntoTheEndlessVoidFunc(rockPathPoints);

        var occupiedCoordinatesCount = PourSandAndGetOccupiedCoordinatesCount(
            berockedCoordinates,
            sandCanMoveToCoordinate: coordinateIsAvailable,
            stopPouringSand: sandWillFallIntoTheEndlessVoid);

        return occupiedCoordinatesCount - rockCount;
    }

    public object GetPart2Solution(string[] input, bool isExampleInput)
    {
        var rockPathPoints = GetRockPathPoints(input);
        var berockedCoordinates = GetBerockedCoordinates(rockPathPoints);

        var rockCount = berockedCoordinates.Count;

        var coordinateIsAvailableAndNotOnFloor = GetCoordinateIsAvailableAndNotOnFloorFunc(rockPathPoints);
        var sandIsBlockingTheSource = GetSandIsBlockingTheSourceFunc();

        var occupiedCoordinatesCount = PourSandAndGetOccupiedCoordinatesCount(
            berockedCoordinates,
            sandCanMoveToCoordinate: coordinateIsAvailableAndNotOnFloor,
            stopPouringSand: sandIsBlockingTheSource);

        return occupiedCoordinatesCount - rockCount;
    }

    private static List<List<(int X, int Y)>> GetRockPathPoints(IEnumerable<string> input)
    {
        return input
            .Select(line => line
                .Split(" -> ")
                .Select(AsCoordinate)
                .ToList())
            .ToList();
    }

    private static (int X, int Y) AsCoordinate(string coorString)
    {
        var split = coorString
            .Split(',')
            .Select(int.Parse)
            .ToList();

        return (split[0], split[1]);
    }

    private static HashSet<(int, int)> GetBerockedCoordinates(List<List<(int X, int Y)>> rockPathPoints)
    {
        var berockedCoordinates = new HashSet<(int, int)>();
        
        // Populate with rocks
        foreach (var rockPathPoint in rockPathPoints)
        {
            for (var pathPointIndex = 1; pathPointIndex < rockPathPoint.Count; pathPointIndex++)
            {
                var sourcePoint = rockPathPoint[pathPointIndex - 1];
                var targetPoint = rockPathPoint[pathPointIndex];

                var rockSegmentPoint = (sourcePoint.X, sourcePoint.Y);

                berockedCoordinates.Add((rockSegmentPoint.X, rockSegmentPoint.Y));
                
                (int X, int Y) rockSegmentLengthXY = (targetPoint.X - sourcePoint.X, targetPoint.Y - sourcePoint.Y);

                var rockSegmentLength = Math.Max(Math.Abs(rockSegmentLengthXY.X), Math.Abs(rockSegmentLengthXY.Y));
                
                (int X, int Y) pathIncrease = (rockSegmentLengthXY.X / rockSegmentLength, rockSegmentLengthXY.Y / rockSegmentLength);

                foreach (var segmentPoint in Enumerable.Range(0, rockSegmentLength))
                {
                    rockSegmentPoint = (rockSegmentPoint.X + pathIncrease.X, rockSegmentPoint.Y + pathIncrease.Y);
                    berockedCoordinates.Add((rockSegmentPoint.X, rockSegmentPoint.Y));
                }
            }
        }

        return berockedCoordinates;
    }

    private static Func<HashSet<(int, int)>, int, int, bool> GetCoordinateIsAvailableFunc()
    {
        return (occupiedSpots, x, y) => !occupiedSpots.Contains((x, y));
    }

    private static Func<HashSet<(int, int)>, int, int, bool> GetCoordinateIsAvailableAndNotOnFloorFunc(
        List<List<(int X, int Y)>> formations)
    {
        var allCoors = GetAllRockPathPoints(formations);
        var yMax = GetYMax(allCoors);

        var yFloor = yMax + 2;

        return (occupiedSpots, x, y) => !occupiedSpots.Contains((x, y)) &&
                                        y < yFloor;
    }

    private static Func<int, int, bool> GetSandWillFallIntoTheEndlessVoidFunc(List<List<(int X, int Y)>> formations)
    {
        var allCoordinates = GetAllRockPathPoints(formations);

        var xAll = allCoordinates.Select(coordinate => coordinate.X).ToList();

        var xMin = xAll.Min();
        var xMax = xAll.Max();
        
        var yMax = GetYMax(allCoordinates);

        return (x, y) => x < xMin || x > xMax || y > yMax;
    }

    private static Func<int, int, bool> GetSandIsBlockingTheSourceFunc()
    {
        return (_, y) => y == SandStartingPointY;
    }

    private static List<(int X, int Y)> GetAllRockPathPoints(List<List<(int X, int Y)>> rockPaths)
    {
        return rockPaths
            .SelectMany(line => line)
            .ToList();
    }

    private static int GetYMax(List<(int X, int Y)> coordinates)
    {
        return coordinates.Select(coors => coors.Y).Max();
    }

    private static int PourSandAndGetOccupiedCoordinatesCount(
        HashSet<(int X, int Y)> occupiedCoordinates,
        Func<HashSet<(int X, int Y)>, int, int, bool> sandCanMoveToCoordinate,
        Func<int, int, bool> stopPouringSand)
    {
        (int X, int Y) shiftDownwards = (0, 1);
        (int X, int Y) shiftLeftwards = (-1, 1);
        (int X, int Y) shiftRightwards = (1, 1);
        
        var shiftsToTryForEachCoordinate = new[] { shiftDownwards, shiftLeftwards, shiftRightwards };
        
        var sandUnitX = SandStartingPointX;
        var sandUnitY = SandStartingPointY;

        var stopPouring = false;

        bool TryMovingSandUnitAndGetShouldStopShifting((int X, int Y) shift)
        {
            var simulatedSandUnitX = sandUnitX + shift.X;
            var simulatedSandUnitY = sandUnitY + shift.Y;

            if (stopPouringSand(simulatedSandUnitX, simulatedSandUnitY))
            {
                stopPouring = true;
                return true;
            }

            if (sandCanMoveToCoordinate(occupiedCoordinates, simulatedSandUnitX, simulatedSandUnitY))
            {
                sandUnitX = simulatedSandUnitX;
                sandUnitY = simulatedSandUnitY;

                return true;
            }

            return false;
        }

        while (!stopPouring)
        {
            var stopShifting = false;
            
            foreach (var shift in shiftsToTryForEachCoordinate)
            {
                stopShifting = TryMovingSandUnitAndGetShouldStopShifting(shift);

                if (stopShifting)
                {
                    break;
                }
            }

            if (stopPouring)
            {
                break;
            }

            if (stopShifting)
            {
                continue;
            }

            // Let unit of sand lie
            occupiedCoordinates.Add((sandUnitX, sandUnitY));

            if (stopPouringSand(sandUnitX, sandUnitY))
            {
                break;
            }
            
            // Start with next unit of sand
            sandUnitX = SandStartingPointX;
            sandUnitY = SandStartingPointY;
        }

        return occupiedCoordinates.Count;
    }
}
