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
		var originalBricks = input
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
		return 0;
	}

	private class Brick
	{
		private Brick(XYZ endA, XYZ endB)
		{
			Ends = new[] { endA, endB };
		}

		public XYZ[] Ends { get; }

		public int MinX => Math.Min(Ends[0].X, Ends[1].X);
		public int MinY => Math.Min(Ends[0].Y, Ends[1].Y);
		public int MinZ => Math.Min(Ends[0].Z, Ends[1].Z);
		public int MaxX => Math.Max(Ends[0].X, Ends[1].X);
		public int MaxY => Math.Max(Ends[0].Y, Ends[1].Y);
		public int MaxZ => Math.Max(Ends[0].Z, Ends[1].Z);

		public bool IsVertical => Ends[0].Z != Ends[1].Z;

		public bool OccupiesPosition(int x, int y, int z) => OccupiesX(x) && OccupiesY(y) && OccupiesZ(z);

		public Brick AfterFalling(int distance) => new Brick(
			new XYZ(Ends[0].X, Ends[0].Y, Ends[0].Z - distance),
			new XYZ(Ends[1].X, Ends[1].Y, Ends[1].Z - distance));

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
}