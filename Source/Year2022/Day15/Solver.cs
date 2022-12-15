using Common.Interfaces;

namespace Year2022;

public class Day15Solver : IPuzzleSolver
{
    public string Title => "Beacon Exclusion Zone";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        Part1Solution = GetPart1Solution(input).ToString();
        Part2Solution = GetPart2Solution(input).ToString();
    }

    private static readonly int CharCountBetweenXAndY = ", y=".Length;
    private static readonly int CharCountPriorToSensor = "Sensor at x=".Length;
    private static readonly int CharCountBetweenSensorAndBeacon = ": closest beacon is at x=".Length;

    private static int GetPart1Solution(IEnumerable<string> input)
    {
        // const int targetRow = 10; // For example input
        const int targetRow = 2000000; // For real input

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

    private static int GetPart2Solution(IEnumerable<string> input)
    {
        return 0;
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
