using System.Numerics;
using Common.Interfaces;

namespace Year2022;

public class Day15Solver : IPuzzleSolver
{
    public string Title => "Beacon Exclusion Zone";

    public object? Part1Solution { get; set; }
    public object? Part2Solution { get; set; }

    private static readonly int CharCountBetweenXAndY = ", y=".Length;
    private static readonly int CharCountPriorToSensor = "Sensor at x=".Length;
    private static readonly int CharCountBetweenSensorAndBeacon = ": closest beacon is at x=".Length;

    public object GetPart1Solution(string[] input, bool isExampleInput)
    {
        const int targetRowForExampleInput = 10;
        const int targetRowForRealInput = 2000000;

        var targetRow = isExampleInput ? targetRowForExampleInput : targetRowForRealInput;

        var beaconPresenceByColAtTargetRow = new Dictionary<int, bool>();

        foreach (var line in input)
        {
            var charsToSkip = CharCountPriorToSensor;

            var sensorXAsString = GetNextNumberAsString(line, charsToSkip);

            charsToSkip += sensorXAsString.Length + CharCountBetweenXAndY;

            var sensorYAsString = GetNextNumberAsString(line, charsToSkip);

            charsToSkip += sensorYAsString.Length + CharCountBetweenSensorAndBeacon;

            var beaconXAsString = GetNextNumberAsString(line, charsToSkip);

            charsToSkip += beaconXAsString.Length + CharCountBetweenXAndY;

            var beaconYAsString = GetNextNumberAsString(line, charsToSkip);
            
            var sensorX = int.Parse(sensorXAsString);
            var sensorY = int.Parse(sensorYAsString);
            var beaconX = int.Parse(beaconXAsString);
            var beaconY = int.Parse(beaconYAsString);

            var proxyDelta = Math.Min(sensorY, beaconY); // want to shift the diamond shape on Y axis to work with smaller numbers

            var sensorYProxy = sensorY - proxyDelta;
            var beaconYProxy = beaconY - proxyDelta;
            var targetRowProxy = targetRow - proxyDelta;

            // If applicable: Add beacon to beacon presence dict
            if (beaconYProxy == targetRowProxy)
            {
                beaconPresenceByColAtTargetRow[beaconX] = true;
            }

            var manhattanOutreach = Math.Abs(beaconX - sensorX) + Math.Abs(beaconYProxy - sensorYProxy);

            var sensorIsAboveTargetRow = sensorYProxy < targetRowProxy;
            var sensorIsBelowTargetRow = sensorYProxy > targetRowProxy;

            if ((sensorIsBelowTargetRow && sensorYProxy - manhattanOutreach > targetRowProxy) ||
                (sensorIsAboveTargetRow && targetRowProxy - manhattanOutreach > sensorYProxy))
            {
                // Sensor does not reach to the target row; not relevant for the puzzle
                continue;
            }

            // If applicable: Add sensor to beacon presence dict
            if (sensorYProxy == targetRowProxy &&
                !beaconPresenceByColAtTargetRow.ContainsKey(sensorX))
            {
                beaconPresenceByColAtTargetRow.Add(sensorX, false);
            }

            var verticalDistanceBetweenSensorAndTargetRow = Math.Abs(targetRowProxy - sensorYProxy);
            var horizontalDistanceFromTheSensor = manhattanOutreach - verticalDistanceBetweenSensorAndTargetRow;

            var targetRowOutreachMinX = sensorX - horizontalDistanceFromTheSensor;
            var targetRowOutreach = 2 * horizontalDistanceFromTheSensor + 1; // 1 is sensor; 2 * h is outreach

            foreach (var col in Enumerable.Range(targetRowOutreachMinX, targetRowOutreach))
            {
                if (!beaconPresenceByColAtTargetRow.ContainsKey(col))
                {
                    beaconPresenceByColAtTargetRow.Add(col, false);
                }
            }
        }

        return beaconPresenceByColAtTargetRow.Values
            .Count(beaconIsPresent => !beaconIsPresent);
    }

    public object GetPart2Solution(string[] input, bool isExampleInput)
    {
        const int tuningFrequencyConstant = 4000000;
        const int posMin = 0;
        const int posMaxForExampleInput = 20;
        const int posMaxForRealInput = 4000000;

        var posMax = isExampleInput ? posMaxForExampleInput : posMaxForRealInput;
        
        var posCount = 1 + posMax - posMin;

        var relevantPos = Enumerable.Range(posMin, posCount).ToHashSet();

        var sensorPositionAndManhattanOutreachPairs = new List<((int X, int Y) Sensor, int Outreach)>();

        foreach (var line in input)
        {
            var charsToSkip = CharCountPriorToSensor;

            var sensorXAsString = GetNextNumberAsString(line, charsToSkip);

            charsToSkip += sensorXAsString.Length + CharCountBetweenXAndY;

            var sensorYAsString = GetNextNumberAsString(line, charsToSkip);

            charsToSkip += sensorYAsString.Length + CharCountBetweenSensorAndBeacon;

            var beaconXAsString = GetNextNumberAsString(line, charsToSkip);

            charsToSkip += beaconXAsString.Length + CharCountBetweenXAndY;

            var beaconYAsString = GetNextNumberAsString(line, charsToSkip);

            var sensorX = int.Parse(sensorXAsString);
            var sensorY = int.Parse(sensorYAsString);
            var beaconX = int.Parse(beaconXAsString);
            var beaconY = int.Parse(beaconYAsString);

            var manhattanOutreach = Math.Abs(beaconX - sensorX) + Math.Abs(beaconY - sensorY);

            if (sensorY - manhattanOutreach > posMax || sensorY + manhattanOutreach < posMin)
            {
                continue;
            }
            
            sensorPositionAndManhattanOutreachPairs.Add(((sensorX, sensorY), manhattanOutreach));
        }

        var targetCol = 0;
        var targetRow = 0;

        foreach (var row in relevantPos)
        {
            // Create full hashset for row (possible cols)
            var possibleColsInRow = relevantPos.ToHashSet();

            // Loop through all items in the pairs list
            foreach (var pair in sensorPositionAndManhattanOutreachPairs)
            {
                var yDelta = Math.Abs(pair.Sensor.Y - row);
                
                if (yDelta > pair.Outreach)
                {
                    // Sensor outreach does not cover current row
                    continue;
                }
                
                // We now know verticalDistance <= pair.Outreach
                // Find all cols that are scanned for given row, and remove them from hashset
                var horizontalOutreachForRelevantRow = pair.Outreach - yDelta;

                var xMin = pair.Sensor.X - horizontalOutreachForRelevantRow;
                var xCount = 1 + 2 * horizontalOutreachForRelevantRow;

                foreach (var col in Enumerable.Range(xMin, xCount))
                {
                    possibleColsInRow.Remove(col);
                }
            }

            if (possibleColsInRow.Any()) // If any; should be single item
            {
                targetRow = row;
                targetCol = possibleColsInRow.Single();
                break;
            }
        }

        var tuningFrequency = new BigInteger(tuningFrequencyConstant * targetCol + targetRow);
        
        return tuningFrequency;
    }

    private static string GetNextNumberAsString(string line, int charsToSkip)
    {
        return new string(line
            .Skip(charsToSkip)
            .TakeWhile(CharIsPartOfNumber)
            .ToArray());
    }

    private static bool CharIsPartOfNumber(char ch) => CharIsDigit(ch) || CharIsMinusSign(ch);
    private static bool CharIsDigit(char ch) => ch > 47 && ch < 58;
    private static bool CharIsMinusSign(char ch) => ch == 45;
}
