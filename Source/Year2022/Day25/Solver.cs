using Common.Interfaces;

namespace Year2022;

public class Day25Solver : IPuzzleSolver
{
    public string Title => "Full of Hot Air";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    private const int NumberSystemBase = 5;
    
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
                var snafuPower = Math.Pow(NumberSystemBase, i);
                var snafuDigit = line[line.Length - 1 - i];
                
                fuelNeededForHotAirBalloon += snafuPower * DecimalDigitBySnafuDigit[snafuDigit];
            }
            
            fuelNeededPerHotAirBalloon.Add(fuelNeededForHotAirBalloon);
        }

        var decimalSum = fuelNeededPerHotAirBalloon.Sum();

        var snafuSum = CalculateSnafuNumber(decimalSum);
        
        Console.WriteLine("Part 1");
        Console.WriteLine("------");
        Console.WriteLine("Decimal sum is: " + decimalSum);
        Console.WriteLine("SNAFU sum is: " + snafuSum);
        
        return snafuSum;
    }

    private static string CalculateSnafuNumber(double decimalNumber)
    {
        var highestNeededPotence = GetHighestNeededPotence(decimalNumber);

        // Account for the possibility that one higher potence needs to be used
        var highestDigitIndex = highestNeededPotence + 1;
        
        const int lowestDigitIndex = 0;

        var multiples = GetMultiples(decimalNumber, lowestDigitIndex, highestDigitIndex);

        var maxMultipleValue = DecimalDigitBySnafuDigit.Values.Max();

        // If necessary: Recalculate items of multiplePerDigitIndex (starting with i = 0)
        while (multiples.Any(mul => mul > maxMultipleValue))
        {
            var addendForNextDigitIndex = 0;

            for (var i = lowestDigitIndex; i <= highestDigitIndex; i++)
            {
                var currentMultiple = multiples[i];

                var multipleIsThree = currentMultiple == 3;
                var multipleIsFour = currentMultiple == 4;

                var locallyUpdatedMultiple = multipleIsFour
                    ? -1
                    : multipleIsThree
                        ? -2
                        : currentMultiple;

                var newMultiple = addendForNextDigitIndex + locallyUpdatedMultiple;

                if (newMultiple != currentMultiple)
                {
                    multiples[i] = newMultiple;
                }

                addendForNextDigitIndex = multipleIsThree || multipleIsFour ? 1 : 0;
            }
        }

        var snafuNumber = GetSnafuNumber(multiples);

        return snafuNumber;
    }

    private static int GetHighestNeededPotence(double decimalNumber)
    {
        var potence = 0;
        double accProduct = 1;

        while (true)
        {
            accProduct *= NumberSystemBase;

            if (accProduct > decimalNumber)
            {
                return potence;
            }

            potence++;
        }
    }

    private static int[] GetMultiples(double decimalNumber, int lowestDigitIndex, int highestDigitIndex)
    {
        var multiples = new int[highestDigitIndex + 1];

        var remainder = decimalNumber;

        // Populate multiples
        for (var i = highestDigitIndex; i >= lowestDigitIndex; i--)
        {
            var baseNumberForIndex = Math.Pow(NumberSystemBase, i);
            var multipleForIndex = (int)(remainder / baseNumberForIndex);
            
            multiples[i] = multipleForIndex;

            remainder -= multipleForIndex * baseNumberForIndex;
        }

        return multiples;
    }

    private static string GetSnafuNumber(int[] multiples)
    {
        var snafuDigitByDecimalDigit = DecimalDigitBySnafuDigit
            .ToDictionary(
                kvp => kvp.Value,
                kvp => kvp.Key);

        var snafuDigits = multiples
            // Order multiples by descending index order
            .Reverse()
            // Trim away the redundant multiples for the upper potences
            .SkipWhile(multiple => multiple == 0)
            // Map to SNAFU digits
            .Select(multiple => snafuDigitByDecimalDigit[multiple])
            .ToArray();

        return new string(snafuDigits);
    }

    private static string GetPart2Solution(IEnumerable<string> input)
    {
        return string.Empty;
    }
}
