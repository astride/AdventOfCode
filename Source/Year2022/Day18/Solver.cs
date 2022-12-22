using Common.Interfaces;
using Common.Models;

namespace Year2022;

public class Day18Solver : IPuzzleSolver
{
    public string Title => "Boiling Boulders";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        Part1Solution = GetPart1Solution(input).ToString();
        Part2Solution = GetPart2Solution(input).ToString();
    }

    private static int GetPart1Solution(IEnumerable<string> input)
    {
        var cubes = input
            .Select(line => line.Split(',').Select(int.Parse).ToArray())
            .Select(coors => new XYZ(coors[0], coors[1], coors[2]))
            .ToList();
        
        var assessedCubes = new HashSet<XYZ>();

        var sharedSideCount = 0;

        foreach (var cube in cubes)
        {
            var surroundingCubes = new[]
            {
                cube.RightOf(),
                cube.LeftOf(),
                cube.Above(),
                cube.Below(),
                cube.InFrontOf(),
                cube.Behind(),
            };

            // Assuming all cubes are distinct
            sharedSideCount += surroundingCubes.Count(assessedCubes.Contains);

            assessedCubes.Add(cube);
        }

        PrintSlicewise(cubes);
        
        return 2 * (3 * cubes.Count - sharedSideCount);
    }

    private static int GetPart2Solution(IEnumerable<string> input)
    {
        return 0;
    }

    private static void PrintSlicewise(IReadOnlyList<XYZ> cubes)
    {
        var xCoors = cubes.Select(cube => cube.X).Distinct().OrderBy(x => x).ToList();
        var yCoors = cubes.Select(cube => cube.Y).Distinct().OrderBy(y => y).ToList();
        var zCoors = cubes.Select(cube => cube.Z).Distinct().OrderBy(z => z).ToList();

        var xMin = xCoors.Min();
        var xMax = xCoors.Max();

        var yMin = yCoors.Min();
        var yMax = yCoors.Max();

        var zMin = zCoors.Min();
        var zMax = zCoors.Max();

        Console.WriteLine("Slices in the Z plane:\r\n");

        for (var z = zMin; z <= zMax; z++)
        {
            Console.WriteLine("Z = " + z + "\r\n");
            
            for (var x = xMin; x <= xMax; x++)
            {
                for (var y = yMin; y <= yMax; y++)
                {
                    var ch = cubes.Contains(new XYZ(x, y, z))
                        ? '#'
                        : '.';
                    
                    Console.Write(ch);
                }
                Console.Write("\r\n");
            }

            Console.WriteLine();
        }
    }
}
