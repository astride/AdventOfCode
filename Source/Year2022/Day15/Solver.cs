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

    private static int GetPart1Solution(IEnumerable<string> input)
    {
        // const int targetRow = 10; // For example input
        const int targetRow = 2000000; // For real input

        var beaconPresenceByColAtTargetRow = new Dictionary<int, bool>();

        foreach (var line in input)
        {
            var sensorX = int.Parse(new string(line
                .Skip("Sensor at x=".Length)
                .TakeWhile(ch => ch == 45 || ch > 47 && ch < 58)
                .ToArray()));

            var sensorY = int.Parse(new string(line
                .Skip("Sensor at x=".Length + sensorX.ToString().Length + ", y=".Length)
                .TakeWhile(ch => ch == 45 || ch > 47 && ch < 58)
                .ToArray()));

            var beaconX = int.Parse(new string(line
                .Skip("Sensor at x=".Length + sensorX.ToString().Length + ", y=".Length + sensorY.ToString().Length + ": closest beacon is at x=".Length)
                .TakeWhile(ch => ch == 45 || ch > 47 && ch < 58)
                .ToArray()));

            var beaconY = int.Parse(new string(line
                .Skip("Sensor at x=".Length + sensorX.ToString().Length + ", y=".Length + sensorY.ToString().Length +  ": closest beacon is at x=".Length + beaconX.ToString().Length + ", y=".Length)
                .TakeWhile(ch => ch == 45 || ch > 47 && ch < 58)
                .ToArray()));

            var proxyDelta = Math.Min(sensorY, beaconY); // want to shift the diamond shape on Y axis to work with smaller numbers

            var sensorYProxy = sensorY - proxyDelta;
            var beaconYProxy = beaconY - proxyDelta;
            var targetRowProxy = targetRow - proxyDelta;

            if (beaconYProxy == targetRowProxy)
            {
                // Add beacon to dict, or update current value
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

            // Add sensor to dict (if applicable)
            if (sensorYProxy == targetRowProxy &&
                !beaconPresenceByColAtTargetRow.ContainsKey(sensorX))
            {
                beaconPresenceByColAtTargetRow.Add(sensorX, false);
            }

            var verticalDistanceBetweenSensorAndTargetRow = Math.Abs(targetRowProxy - sensorYProxy);
            var horizontalDistanceFromTheSensor = manhattanOutreach - verticalDistanceBetweenSensorAndTargetRow;

            foreach (var col in Enumerable
                         .Range(sensorX - horizontalDistanceFromTheSensor, (2 * horizontalDistanceFromTheSensor + 1)))
            {
                if (beaconPresenceByColAtTargetRow.TryGetValue(col, out _))
                {
                    continue; // Do not overwrite value
                }

                beaconPresenceByColAtTargetRow.Add(col, false);
            }
        }

        return beaconPresenceByColAtTargetRow
            .Values
            .Count(beaconIsPresent => !beaconIsPresent);
    }

    private static int GetPart2Solution(IEnumerable<string> input)
    {
        return 0;
    }
}
