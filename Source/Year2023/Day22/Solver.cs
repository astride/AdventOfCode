using Common.Interfaces;
using Common.Models;

namespace Year2023;

public class Day22Solver : IPuzzleSolver
{
	public string Title => "Sand Slabs";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var landedBricks = GetLandedBricks(input);

		var landedBricksByLowestOccupiedLevel = landedBricks
			.GroupBy(brick => brick.MinZ)
			.Where(group => group.Key > 1) // Disregard lowest level; the ground is supporting these bricks
			.ToDictionary(group => group.Key, group => group.ToList());

		var landedBricksByUppermostOccupiedLevel = landedBricks
			.GroupBy(brick => brick.MaxZ)
			.ToDictionary(group => group.Key, group => group.ToList());

		var bricksThatAloneSupportAnotherBrick = new HashSet<Brick>();

		foreach (var occupiedLevel in landedBricksByLowestOccupiedLevel.Keys)
		{
			var supportingLevel = occupiedLevel - 1;

			var occupyingBricks = landedBricksByLowestOccupiedLevel[occupiedLevel];
			var supportingBricks = landedBricksByUppermostOccupiedLevel[supportingLevel];

			foreach (var occupyingBrick in occupyingBricks)
			{
				var bricksSupportingOccupyingBrick = new HashSet<Brick>();

				for (var x = occupyingBrick.MinX; x <= occupyingBrick.MaxX; x++)
				{
					for (var y = occupyingBrick.MinY; y <= occupyingBrick.MaxY; y++)
					{
						var supportingBrick = supportingBricks
							.SingleOrDefault(brick => brick.OccupiesPosition(x, y, supportingLevel));

						if (supportingBrick != default)
						{
							bricksSupportingOccupyingBrick.Add(supportingBrick);
						}
					}
				}

				if (bricksSupportingOccupyingBrick.Count == 1)
				{
					bricksThatAloneSupportAnotherBrick.Add(bricksSupportingOccupyingBrick.Single());
				}
			}
		}

		var brickCountSafeToDisintegrate = landedBricks.Count - bricksThatAloneSupportAnotherBrick.Count;

		return brickCountSafeToDisintegrate;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		var landedBricks = GetLandedBricks(input);

		var landedBricksByLowestOccupiedLevel = landedBricks
			.GroupBy(brick => brick.MinZ)
			.Where(group => group.Key > 1) // Disregard lowest level; the ground is supporting these bricks
			.ToDictionary(group => group.Key, group => group.ToList());

		var landedBricksByUppermostOccupiedLevel = landedBricks
			.GroupBy(brick => brick.MaxZ)
			.ToDictionary(group => group.Key, group => group.ToList());

		var supportedBricksBySupportingBricks = new Dictionary<BrickCollection, List<Brick>>();

		foreach (var occupiedLevel in landedBricksByLowestOccupiedLevel.Keys)
		{
			var supportingLevel = occupiedLevel - 1;

			var occupyingBricks = landedBricksByLowestOccupiedLevel[occupiedLevel];
			var supportingBricks = landedBricksByUppermostOccupiedLevel[supportingLevel];

			foreach (var brick in occupyingBricks)
			{
				var bricksSupportingOccupyingBrick = new HashSet<Brick>();

				for (var x = brick.MinX; x <= brick.MaxX; x++)
				{
					for (var y = brick.MinY; y <= brick.MaxY; y++)
					{
						var supportingBrick = supportingBricks
							.SingleOrDefault(brick => brick.OccupiesPosition(x, y, supportingLevel));

						if (supportingBrick != default)
						{
							bricksSupportingOccupyingBrick.Add(supportingBrick);
						}
					}
				}

				if (bricksSupportingOccupyingBrick.Count < 1)
				{
					continue;
				}

				var supportingBrickCollection = new BrickCollection(bricksSupportingOccupyingBrick);

				if (supportedBricksBySupportingBricks.TryGetValue(supportingBrickCollection, out var supportedBricks))
				{
					supportedBricks.Add(brick);
				}
				else
				{
					supportedBricksBySupportingBricks[supportingBrickCollection] = new List<Brick> { brick };
				}
			}
		}

		int GetFallingBrickCountWhenDisintegratingBrick(Brick brick)
		{
			var brickCollection = new BrickCollection(new[] { brick });

			if (!supportedBricksBySupportingBricks.TryGetValue(brickCollection, out var supportedBricks))
			{
				return 0;
			}

			return GetFallingBricksWhenDisintegratingBricks(supportedBricks.ToHashSet()).Count;
		}

		HashSet<Brick> GetFallingBricksWhenDisintegratingBricks(HashSet<Brick> bricks)
		{
			var fallingBricks = supportedBricksBySupportingBricks
				.Where(item => !item.Key.Bricks.Except(bricks).Any())
				.SelectMany(item => item.Value)
				.Distinct()
				.ToList();

			if (!fallingBricks.Any() || !fallingBricks.Except(bricks).Any())
			{
				return bricks;
			}

			foreach (var fallingBrick in fallingBricks)
			{
				bricks.Add(fallingBrick);
			}

			return GetFallingBricksWhenDisintegratingBricks(bricks);
		}

		var sum = landedBricks.Sum(GetFallingBrickCountWhenDisintegratingBrick);

		return sum;
	}

	private static List<Brick> GetLandedBricks(IEnumerable<string> brickDescriptions)
	{
		var originalBricks = brickDescriptions
			.Select(Brick.DescribedBy)
			.OrderBy(brick => brick.MinZ)
			.ToList();
		
		var bricksByOriginalDistanceToTheGround = originalBricks
			.GroupBy(brick => brick.MinZ)
			.ToDictionary(group => group.Key, group => group.ToList());
		
		var maxX = originalBricks.Max(brick => brick.MaxX);
		var maxY = originalBricks.Max(brick => brick.MaxY);

		var landedBricks = new List<Brick>();
		
		var uppermostOccupiedPositions = new int[maxX + 1][];

		for (var x = 0; x <= maxX; x++)
		{
			var newRow = new int[maxY + 1];
			
			Array.Fill(newRow, 0);
			
			uppermostOccupiedPositions[x] = newRow;
		}

		int GetLandingLevel(Brick brick)
		{
			if (brick.IsVertical)
			{
				return uppermostOccupiedPositions[brick.MaxX][brick.MaxY];
			}

			var uppermostOccupiedPosition = 0;

			for (var x = brick.MinX; x <= brick.MaxX; x++)
			{
				for (var y = brick.MinY; y <= brick.MaxY; y++)
				{
					uppermostOccupiedPosition = Math.Max(uppermostOccupiedPosition, uppermostOccupiedPositions[x][y]);
				}
			}

			return uppermostOccupiedPosition;
		}

		foreach (var originalDistanceToTheGround in bricksByOriginalDistanceToTheGround.Keys)
		{
			var bricksWithLowestEndOnThisLevel = bricksByOriginalDistanceToTheGround[originalDistanceToTheGround];

			// Register landed bricks
			foreach (var brick in bricksWithLowestEndOnThisLevel)
			{
				var fallDistance = brick.MinZ - GetLandingLevel(brick) - 1;

				var landedBrick = brick.AfterFalling(fallDistance);

				landedBricks.Add(landedBrick);

				if (landedBrick.IsVertical)
				{
					uppermostOccupiedPositions[landedBrick.MinX][landedBrick.MinY] = landedBrick.MaxZ;
				}
				else
				{
					for (var x = landedBrick.MinX; x <= landedBrick.MaxX; x++)
					{
						for (var y = landedBrick.MinY; y <= landedBrick.MaxY; y++)
						{
							uppermostOccupiedPositions[x][y] = landedBrick.MaxZ;
						}
					}
				}
			}
		}

		return landedBricks;
	}

	private class Brick
	{
		private Brick(XYZ endA, XYZ endB)
		{
			_endA = endA;
			_endB = endB;
		}

		private readonly XYZ _endA;
		private readonly XYZ _endB;

		public int MinX => Math.Min(_endA.X, _endB.X);
		public int MinY => Math.Min(_endA.Y, _endB.Y);
		public int MinZ => Math.Min(_endA.Z, _endB.Z);
		public int MaxX => Math.Max(_endA.X, _endB.X);
		public int MaxY => Math.Max(_endA.Y, _endB.Y);
		public int MaxZ => Math.Max(_endA.Z, _endB.Z);

		public bool IsVertical => _endA.Z != _endB.Z;

		public bool OccupiesPosition(int x, int y, int z) => OccupiesX(x) && OccupiesY(y) && OccupiesZ(z);

		public Brick AfterFalling(int distance) => new Brick(
			new XYZ(_endA.X, _endA.Y, _endA.Z - distance),
			new XYZ(_endB.X, _endB.Y, _endB.Z - distance));

		public override string ToString()
		{
			var ends = new[] { _endA, _endB }
				.OrderBy(end => end.X)
				.ThenBy(end => end.Y)
				.ThenBy(end => end.Z)
				.Select(end => $"{end.X},{end.Y},{end.Z}");

			return string.Join('~', ends);
		}

		public static Brick DescribedBy(string description)
		{
			var ends = description
				.Split('~')
				.Select(end => end.Split(',').Select(int.Parse).ToList())
				.Select(endCoordinates => new XYZ(endCoordinates[0], endCoordinates[1], endCoordinates[2]))
				.ToList();

			return new Brick(ends[0], ends[1]);
		}

		private bool OccupiesX(int x) => x >= MinX && x <= MaxX;
		private bool OccupiesY(int y) => y >= MinY && y <= MaxY;
		private bool OccupiesZ(int z) => z >= MinZ && z <= MaxZ;
	}

	private class BrickCollection
	{
		public BrickCollection(IEnumerable<Brick> bricks)
		{
			Bricks = new HashSet<Brick>();

			foreach (var brick in bricks)
			{
				Bricks.Add(brick);
			}
		}

		public HashSet<Brick> Bricks { get; }

		public override bool Equals(object? obj)
		{
			if (obj is BrickCollection other)
			{
				return other.Bricks.SequenceEqual(Bricks);
			}

			return false;
		}

		public override int GetHashCode()
		{
			return string.Join('_', Bricks.Select(brick => brick.ToString())).GetHashCode();
		}
	}
}