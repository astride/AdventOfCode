using AdventOfCode.Common;
using AdventOfCode.Common.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021
{
	public class Day19Solver : IPuzzleSolver
	{
		public string Part1Solution { get; set; }
		public string Part2Solution { get; set; }

		public void SolvePuzzle(string[] rawInput)
		{
			var beaconPositionsRelativeToScanner = new List<List<CoordinateXYZ>>();

			var filteredInput = rawInput
				.Where(entry => !entry.Contains("---"))
				.ToList();

			while (filteredInput.Any() &&
				filteredInput.Any(entry => !string.IsNullOrWhiteSpace(entry)))
			{
				beaconPositionsRelativeToScanner.Add(filteredInput
					.SkipWhile(entry => string.IsNullOrWhiteSpace(entry))
					.TakeWhile(entry => !string.IsNullOrWhiteSpace(entry))
					.Select(entry => entry.Split(','))
					.Select(coors => new CoordinateXYZ(
						int.Parse(coors[0]), 
						int.Parse(coors[1]), 
						int.Parse(coors[2])))
					.ToList());

				filteredInput = filteredInput
					.SkipWhile(entry => string.IsNullOrWhiteSpace(entry))
					.Skip(beaconPositionsRelativeToScanner.Last().Count())
					.ToList();
			}

			Part1Solution = SolvePart1(beaconPositionsRelativeToScanner).ToString();
		}

		private static int SolvePart1(List<List<CoordinateXYZ>> beaconPositionsRelativeToScanner)
		{
			return beaconPositionsRelativeToScanner.GetBeaconCount();
		}
	}

	public static class Day19Helpers
	{
		private const int OverlapRequirement = 12;

		private static List<Dictionary<int, List<CoordinateXYZ>>> BeaconPositionRelativeToBeaconDetectedByScanner;
		private static List<(int ScannerA, int BeaconA, int ScannerB, int BeaconB, CoordinateXYZ Mapping)> MatchingBeaconsOnOverlappingMaps;

		public static int GetBeaconCount(this List<List<CoordinateXYZ>> beaconPositionsRelativeToScanner)
		{
			CalculateAllRelativeBeaconPositions(beaconPositionsRelativeToScanner);

			var scannerCount = beaconPositionsRelativeToScanner.Count;

			MatchingBeaconsOnOverlappingMaps = new List<(int ScannerA, int BeaconA, int ScannerB, int BeaconB, CoordinateXYZ Mapping)>();

			foreach (var scanner in Enumerable.Range(0, scannerCount - 1))
			{
				foreach (var otherScanner in Enumerable.Range(scanner + 1, scannerCount - 1 - scanner))
				{
					// OK - Alle scannere sjekkes mot hverandre
					scanner.CheckIfOverlapsWith(otherScanner);
				}
			}

			// We now have all existing scanner mapping starting points

			//[i]: (S, B, S, B,  { X,Y,Z })
			//-----------------------------
			//[0]: (0, 0, 1, 3,  { 0,1,2 })
			//[1]: (1, 6, 3, 2,  { 0,1,2 })
			//[2]: (1, 2, 4, 4,  { 1,2,0 })
			//[3]: (2, 0, 4, 14, { 1,0,2 })

			return -1;
		}

		private static void CalculateAllRelativeBeaconPositions(List<List<CoordinateXYZ>> beaconPositionsRelativeToScanner)
		{
			BeaconPositionRelativeToBeaconDetectedByScanner = new List<Dictionary<int, List<CoordinateXYZ>>>();

			foreach (var scanner in Enumerable.Range(0, beaconPositionsRelativeToScanner.Count))
			{
				BeaconPositionRelativeToBeaconDetectedByScanner.Add(new Dictionary<int, List<CoordinateXYZ>>());

				foreach (var basePosition in Enumerable.Range(0, beaconPositionsRelativeToScanner[scanner].Count))
				{
					BeaconPositionRelativeToBeaconDetectedByScanner.Last()[basePosition] =
						beaconPositionsRelativeToScanner[scanner]
							.Select(relativePosition => new CoordinateXYZ(
								relativePosition.X - beaconPositionsRelativeToScanner[scanner][basePosition].X,
								relativePosition.Y - beaconPositionsRelativeToScanner[scanner][basePosition].Y,
								relativePosition.Z - beaconPositionsRelativeToScanner[scanner][basePosition].Z))
							.ToList();
				}
			}
		}

		private static void CheckIfOverlapsWith(this int scanner, int otherScanner)
		{
			var beaconPositionRelativeToBeacon = BeaconPositionRelativeToBeaconDetectedByScanner[scanner];
			var beaconPositionRelativeToOtherBeacon = BeaconPositionRelativeToBeaconDetectedByScanner[otherScanner];

			foreach (var beacon in Enumerable.Range(0, beaconPositionRelativeToBeacon.Count))
			{
				foreach (var otherBeacon in Enumerable.Range(0, beaconPositionRelativeToOtherBeacon.Count))
				{
					// OK - Alle beacons sjekkes mot hverandre
					if (beaconPositionRelativeToBeacon[beacon].OverlapsWith(beaconPositionRelativeToOtherBeacon[otherBeacon], out var mapping))
					{
						MatchingBeaconsOnOverlappingMaps.Add((scanner, beacon, otherScanner, otherBeacon, mapping));

						return;
					}
				}
			}
		}

		private static bool OverlapsWith(this IEnumerable<CoordinateXYZ> positionsA, IEnumerable<CoordinateXYZ> positionsB, out CoordinateXYZ mapping)
		{
			var overlappingMagnitudes = positionsA
				.Select(pos => pos.Magnitude)
				.Intersect(positionsB.Select(pos => pos.Magnitude));

			if (overlappingMagnitudes.Count() >= OverlapRequirement)
			{
				// Magnitudes suggest there might be overlapping
				// Find XYZ mapping to verify

				return positionsA
					.Where(pos => overlappingMagnitudes.Contains(pos.Magnitude))
					.OverlapsWithMapped(positionsB.Where(pos => overlappingMagnitudes.Contains(pos.Magnitude)), out mapping);
			}

			mapping = null;
			return false;
		}

		private static bool OverlapsWithMapped(this IEnumerable<CoordinateXYZ> positionsA, IEnumerable<CoordinateXYZ> positionsB, out CoordinateXYZ mapping)
		{
			// Map B in all ways possible (6), but stick to Abs values
			mapping = null;

			var uniqueMagnitude = positionsA
				.Select(pos => pos.Magnitude)
				.GroupBy(magn => magn)
				.First(gr => gr.Count() == 1)
				.Key;

			var positionAWithUniqueMagnitude = positionsA
				.Single(pos => pos.Magnitude == uniqueMagnitude);

			var positionBWithUniqueMagnitude = positionsB
				.Single(pos => pos.Magnitude == uniqueMagnitude);

			// Find all mappings that are seemingly applicable
			// Verify all and return first applicable mapping

			var possibleMappings = positionAWithUniqueMagnitude.GetPossibleMappingsFor(positionBWithUniqueMagnitude);

			if (possibleMappings == null || !possibleMappings.Any()) return false;

			// Check mapping for whole set
			foreach (var possibleMapping in possibleMappings)
			{
				if (positionsA
					.Select(pos => pos.Abs)
					.Intersect(positionsB.Select(pos => pos.MappedWith(possibleMapping).Abs))
					.Count() >= OverlapRequirement)
				{
					mapping = possibleMapping;
					return true;
				}
			}

			return false;
		}
	}
}
