using Common.Interfaces;

namespace Year2022;

public class Day25Solver : IPuzzleSolver
{
    public string Title => "Full of Hot Air";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;
    
    private static readonly Dictionary<char, int> DecimalDigitBySnafuDigit = new()
    {
        ['2'] = 2,
        ['1'] = 1,
        ['0'] = 0,
        ['-'] = -1,
        ['='] = -2,
    }; 

    public void SolvePuzzle(string[] input)
    {
        Part1Solution = GetPart1Solution(input).ToString();
        Part2Solution = GetPart2Solution(input).ToString();
    }

    private static string GetPart1Solution(IEnumerable<string> input)
    {
        var fuelNeededPerHotAirBalloon = new List<double>(input.Count());

        foreach (var line in input)
        {
            double fuelNeededForHotAirBalloon = 0;

            for (var i = 0; i < line.Length; i++)
            {
                var snafuPower = Math.Pow(5, i);
                var snafuDigit = line[line.Length - 1 - i];
                
                fuelNeededForHotAirBalloon += snafuPower * DecimalDigitBySnafuDigit[snafuDigit];
            }
            
            fuelNeededPerHotAirBalloon.Add(fuelNeededForHotAirBalloon);
        }

        var decimalSum = fuelNeededPerHotAirBalloon.Sum();

        Console.WriteLine("Decimal sum is: " + decimalSum);

        var snafuSum = CalculateSnafuNumber(decimalSum);
        
        return snafuSum;
    }

    private static string CalculateSnafuNumber(double decimalNumber)
    {
        // TODO

        return decimalNumber.ToString();
    }

    private static int GetPart2Solution(IEnumerable<string> input)
    {
        return 0;
    }
}
