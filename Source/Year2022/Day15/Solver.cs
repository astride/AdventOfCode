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

            // If applicable: Add beacon to beacon presence dict
            if (beaconY == targetRow)
            {
                beaconPresenceByColAtTargetRow[beaconX] = true;
            }

            var manhattanOutreach = Math.Abs(beaconX - sensorX) + Math.Abs(beaconY - sensorY);

            var sensorIsAboveTargetRow = sensorY < targetRow;
            var sensorIsBelowTargetRow = sensorY > targetRow;

            var targetRowIsOutsideOfUpwardSensorReach = sensorY - manhattanOutreach > targetRow;
            var targetRowIsOutsideOfDownwardSensorReach = targetRow - manhattanOutreach > sensorY;

            if ((sensorIsBelowTargetRow && targetRowIsOutsideOfUpwardSensorReach) ||
                (sensorIsAboveTargetRow && targetRowIsOutsideOfDownwardSensorReach))
            {
                // Sensor does not reach to the target row; not relevant for the puzzle
                continue;
            }

            // If applicable: Add sensor to beacon presence dict
            if (sensorY == targetRow &&
                !beaconPresenceByColAtTargetRow.ContainsKey(sensorX))
            {
                beaconPresenceByColAtTargetRow.Add(sensorX, false);
            }

            var verticalDistanceBetweenSensorAndTargetRow = Math.Abs(targetRow - sensorY);
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
        return "Not solved yet";
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
