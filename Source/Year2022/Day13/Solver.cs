using Common.Interfaces;

namespace Year2022;

public class Day13Solver : IPuzzleSolver
{
    public string Title => "Distress Signal";

    public object? Part1Solution { get; set; }
    public object? Part2Solution { get; set; }

    private const char ListOpeningChar = '[';
    private const char ListClosingChar = ']';
    private const string Ten = "10";

    public object GetPart1Solution(string[] input)
    {
        var packetPairs = input
            .Where(line => !string.IsNullOrEmpty(line))
            .Chunk(2)
            .ToList();

        var packetPairIndicesInCorrectOrder = new HashSet<int>();

        var left = string.Empty;
        var right = string.Empty;

        bool LeftPacketCanContain10(int i) => left.Length > i + 1;
        bool RightPacketCanContain10(int i) => right.Length > i + 1;
            
        string GetTwoCharsFromLeftPacket(int i) => left[i..(i + 2)];
        string GetTwoCharsFromRightPacket(int i) => right[i..(i + 2)];

        for (var iPacketPair = 0; iPacketPair < packetPairs.Count; iPacketPair++)
        {
            left = packetPairs[iPacketPair].First();
            right = packetPairs[iPacketPair].Last();

            var iLeft = -1;
            var iRight = -1;

            while (true)
            {
                iLeft++;
                iRight++;
                
                var leftChar = left[iLeft];
                var rightChar = right[iRight];
                
                #region Check for presence of number > 9 (more than one digit)

                // Hacky business here... (we only know that the packets can contain no greater value than 10 due to visual verification)
                var leftIsTen = LeftPacketCanContain10(iLeft) && GetTwoCharsFromLeftPacket(iLeft) == Ten;
                var rightIsTen = RightPacketCanContain10(iRight) && GetTwoCharsFromRightPacket(iRight) == Ten;

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
                    packetPairIndicesInCorrectOrder.Add(iPacketPair + 1);
                    break;
                }
                
                #endregion
                
                #region Check for char equality (number < 10, or non-numeric char)
                
                if (leftChar == rightChar)
                {
                    // Undetermined
                    continue;
                }
                
                #endregion

                #region Check for clashing list opening/closing
                
                if (leftChar == ListOpeningChar && rightChar == ListClosingChar)
                {
                    break;
                }
                
                if (leftChar == ListClosingChar && rightChar == ListOpeningChar)
                {
                    packetPairIndicesInCorrectOrder.Add(iPacketPair + 1);
                    break;
                }
                
                #endregion

                #region Check and adjust for incoherent list closing
                
                if (leftChar == ListClosingChar)
                {
                    packetPairIndicesInCorrectOrder.Add(iPacketPair + 1);
                    break;
                }

                if (rightChar == ListClosingChar)
                {
                    break;
                }
                
                #endregion
                
                #region Check and adjust for incoherent list opening
                
                if (leftChar == ListOpeningChar)
                {
                    var dataPrecedingRightChar = right[..iRight];
                    var dataFollowingRightChar = right[(iRight + 1)..];
                    
                    right = dataPrecedingRightChar + ListOpeningChar + rightChar + ListClosingChar + dataFollowingRightChar;
                    continue;
                }

                if (rightChar == ListOpeningChar)
                {
                    var dataPrecedingLeftChar = left[..iLeft];
                    var dataFollowingLeftChar = left[(iLeft + 1)..];
                    
                    left = dataPrecedingLeftChar + ListOpeningChar + leftChar + ListClosingChar + dataFollowingLeftChar;
                    continue;
                }
                
                #endregion

                #region Determine char inequality

                if (leftChar > rightChar)
                {
                    break;
                }

                if (leftChar < rightChar)
                {
                    packetPairIndicesInCorrectOrder.Add(iPacketPair + 1);
                    break;
                }

                #endregion
            }
        }
        
        return packetPairIndicesInCorrectOrder.Sum();
    }

    public object GetPart2Solution(string[] input)
    {
        return 0;
    }
}
