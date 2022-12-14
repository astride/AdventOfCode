using Common.Interfaces;

namespace Year2022;

public class Day13Solver : IPuzzleSolver
{
    public string Title => "Distress Signal";

    public string Part1Solution { get; set; } = string.Empty;
    public string Part2Solution { get; set; } = string.Empty;

    public void SolvePuzzle(string[] input)
    {
        Part1Solution = GetPart1Solution(input).ToString();
        Part2Solution = GetPart2Solution(input).ToString();
    }

    private static int GetPart1Solution(IEnumerable<string> input)
    {
        var packetPairs = input
            .Where(line => !string.IsNullOrEmpty(line))
            .Chunk(2)
            .ToList();

        var packetPairIndicesInRightOrder = new HashSet<int>();

        for (var i = 0; i < packetPairs.Count; i++)
        {
            var left = packetPairs[i].First();
            var right = packetPairs[i].Last();

            var iLeft = -1;
            var iRight = -1;

            while (true)
            {
                iLeft++;
                iRight++;
                
                var leftChar = left[iLeft];
                var rightChar = right[iRight];

                // Hacky business here...
                var leftIsTen = left.Length > iLeft + 3 && left[iLeft..(iLeft + 2)] == "10";
                var rightIsTen = right.Length > iRight + 3 && right[iRight..(iRight + 2)] == "10";

                if (leftIsTen)
                {
                    iLeft++;
                }

                if (rightIsTen)
                {
                    iRight++;
                }

                if (leftIsTen && rightIsTen)
                {
                    continue;
                }

                if (leftIsTen)
                {
                    break;
                }

                if (rightIsTen)
                {
                    packetPairIndicesInRightOrder.Add(i + 1);
                    break;
                }
                
                if (leftChar == rightChar)
                {
                    continue;
                }

                if (leftChar == '[' && rightChar == ']')
                {
                    break;
                }
                
                if (leftChar == ']' && rightChar == '[')
                {
                    packetPairIndicesInRightOrder.Add(i + 1);
                    break;
                }

                if (leftChar == ']')
                {
                    packetPairIndicesInRightOrder.Add(i + 1);
                    break;
                }

                if (rightChar == ']')
                {
                    break;
                }
                
                if (leftChar == '[')
                {
                    right = right[..iRight] + '[' + rightChar + ']' + right[(iRight + 1)..];
                    continue;
                }

                if (rightChar == '[')
                {
                    left = left[..iLeft] + '[' + leftChar + ']' + left[(iLeft + 1)..];
                    continue;
                }
                
                if (leftChar > rightChar)
                {
                    break;
                }

                if (leftChar < rightChar)
                {
                    packetPairIndicesInRightOrder.Add(i + 1);
                    break;
                }
            }
        }
        
        return packetPairIndicesInRightOrder.Sum();
    }

    private static int GetPart2Solution(IEnumerable<string> input)
    {
        return 0;
    }
}
