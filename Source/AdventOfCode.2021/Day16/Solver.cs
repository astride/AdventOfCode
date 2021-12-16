using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day16Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var input = rawInput.Single();

			Part1Solution = SolvePart1(input).ToString();
		}

		private static int SolvePart1(string bitsTransmission)
		{
			var versionNumberSum = bitsTransmission.GetVersionNumberSum();

			return versionNumberSum;
		}
	}

	public static class Day16Helpers
	{
		private static IDictionary<char, string> BinaryOfHexadecimal = new Dictionary<char, string>
		{
			['0'] = "0000",
			['1'] = "0001",
			['2'] = "0010",
			['3'] = "0011",
			['4'] = "0100",
			['5'] = "0101",
			['6'] = "0110",
			['7'] = "0111",
			['8'] = "1000",
			['9'] = "1001",
			['A'] = "1010",
			['B'] = "1011",
			['C'] = "1100",
			['D'] = "1101",
			['E'] = "1110",
			['F'] = "1111"
		};

		private static IDictionary<string, int> IntOf3BitBinary = new Dictionary<string, int>
		{
			["000"] = 0,
			["001"] = 1,
			["010"] = 2,
			["011"] = 3,
			["100"] = 4,
			["101"] = 5,
			["110"] = 6,
			["111"] = 7,
		};

		private static List<int> PacketVersions = new List<int>();
		
		private const int PacketVersionLength = 3;
		private const int PacketTypeIdLength = 3;
		private const int PacketHeaderLength = PacketVersionLength + PacketTypeIdLength;

		private const int LiteralValuePacketTypeId = 4;
        private const int LiteralValueGroupLength = 5;
        private const int LiteralValuePartLength = 4;
        private const char LiteralValueLastGroupChar = '0';

        private const int BitCountOfNumberRepresentingBitCountInSubPackets = 15;
		private const int BitCountOfNumberRepresentingSubPacketCount = 11;

		public static int GetVersionNumberSum(this string transmission)
        {
			var decodedTransmission = transmission.Decode();

			decodedTransmission.UnwrapPackageAndReturnPackageContentLength();

			return PacketVersions.Sum();
        }

		private static int UnwrapPackageAndReturnPackageContentLength(this string transmission, int packetIndex = 0)
        {
			// This method should not know about siblings!

			// LVPs are always the innermost packets(?)
			// OPs and LVPs are never siblings(?)

			// adding packet version number
			PacketVersions.Add(IntOf3BitBinary[transmission.Substring(packetIndex, PacketVersionLength)]);

			// checking packet type ID
			if (IntOf3BitBinary[transmission.Substring(packetIndex + PacketVersionLength, PacketTypeIdLength)] == LiteralValuePacketTypeId)
			{
				return transmission.GetLiteralValuePacketLength(packetIndex);
            }
			else
            {
				var representingBitsIndex = packetIndex + PacketHeaderLength + 1;

				// checking length type ID of operator packet
				if (transmission[packetIndex + PacketHeaderLength] == '0')
				{
					var totalSubPacketBitsLengthEncoded = transmission
						.Substring(representingBitsIndex, BitCountOfNumberRepresentingBitCountInSubPackets);

					var totalSubPacketBitsLength = totalSubPacketBitsLengthEncoded.DecodeBinary();

					var unwrappedLength = 0;

					// checking sub packets
					while (unwrappedLength < totalSubPacketBitsLength)
                    {
						unwrappedLength += transmission
							.UnwrapPackageAndReturnPackageContentLength(representingBitsIndex + BitCountOfNumberRepresentingBitCountInSubPackets + unwrappedLength);
                    }

					return unwrappedLength;
				}
				else
				{
					var subPacketCountEncoded = transmission
						.Substring(representingBitsIndex, BitCountOfNumberRepresentingSubPacketCount);

					var subPacketCount = subPacketCountEncoded.DecodeBinary();
					var unwrappedLength = 0;

					// checking sub packets
					foreach (var _ in Enumerable.Range(1, subPacketCount))
                    {
						unwrappedLength += transmission
							.UnwrapPackageAndReturnPackageContentLength(representingBitsIndex + BitCountOfNumberRepresentingSubPacketCount + unwrappedLength);
					}

					return unwrappedLength;
				}
			}
		}

        private static int GetLiteralValuePacketLength(this string transmission, int packetIndex)
        {
            var groupCount = 0;
            var isLastGroup = false;

            var indexer = PacketHeaderLength;

            while (!isLastGroup)
            {
                groupCount++;

                if (transmission[packetIndex + indexer] == LiteralValueLastGroupChar)
                {
                    isLastGroup = true;
                }

                indexer += LiteralValueGroupLength;
            }

            var packetLength = PacketHeaderLength + (groupCount * LiteralValueGroupLength);
            //var paddedPacketLength = packetLength + (LiteralValuePartLength - (packetLength % LiteralValuePartLength));

            return packetLength;
        }

        private static string Decode(this string encodedTransmission)
        {
			var decodedTransmission = string.Concat(encodedTransmission
				.Select(ch => BinaryOfHexadecimal[ch]));

			return decodedTransmission;
        }

		private static int DecodeBinary(this string binaryString)
        {
			if (binaryString.All(ch => ch == '0')) return 0;

			var binary = binaryString.TrimStart('0');

			var decoded = int.Parse(binary.Last().ToString());

			foreach (var i in Enumerable.Range(1, binary.Length - 1))
            {
				decoded += (int)Math.Pow(2 * int.Parse(binary[binary.Length - (i + 1)].ToString()), i);
            }

			return decoded;
        }
	}
}
