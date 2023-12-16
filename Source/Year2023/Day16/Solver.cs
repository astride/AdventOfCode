using Common.Interfaces;

namespace Year2023;

public class Day16Solver : IPuzzleSolver
{
	public string Title => "The Floor Will Be Lava";

	public object? Part1Solution { get; set; }
	public object? Part2Solution { get; set; }

	public object GetPart1Solution(string[] input, bool isExampleInput)
	{
		var rows = input.Length;
		var cols = input[0].Length;
		
		var energizedMirrorsAndSplitters = new HashSet<(int Row, int Col)>();
		var beamDirectionsByPosition = new Dictionary<(int Row, int Col), List<BeamDirection>>();

		var startingBeam = new Beam(0, 0, BeamDirection.Right);

		var activeBeams = new List<Beam> { startingBeam };

		beamDirectionsByPosition[(startingBeam.Row, startingBeam.Col)] = new List<BeamDirection> { startingBeam.Direction };

		bool BeamHasTraveledThisEmptySpaceBefore(int row, int col, BeamDirection direction)
		{
			var position = input[row][col];

			if (position != '.')
			{
				energizedMirrorsAndSplitters.Add((row, col));
				return false;
			}

			if (!beamDirectionsByPosition.TryGetValue((row, col), out var beamDirections))
			{
				beamDirectionsByPosition[(row, col)] = new List<BeamDirection> { direction };
				return false;
			}

			if (!beamDirections.Contains(direction))
			{
				beamDirections.Add(direction);
				return false;
			}

			return true;
		}

		while (activeBeams.Any())
		{
			var disappearingBeamIndices = new HashSet<int>();
			var emergingBeams = new List<Beam>();

			for (var iBeam = 0; iBeam < activeBeams.Count; iBeam++)
			{
				var beam = activeBeams[iBeam];

				switch (beam.Direction)
				{
					case BeamDirection.Up:
						if (beam.Row == 0 || BeamHasTraveledThisEmptySpaceBefore(beam.Row - 1, beam.Col, beam.Direction))
						{
							disappearingBeamIndices.Add(iBeam);
						}
						else
						{
							beam.Row--;
						}

						break;
					case BeamDirection.Right:
						if (beam.Col == cols - 1 || BeamHasTraveledThisEmptySpaceBefore(beam.Row, beam.Col + 1, beam.Direction))
						{
							disappearingBeamIndices.Add(iBeam);
						}
						else
						{
							beam.Col++;
						}

						break;
					case BeamDirection.Down:
						if (beam.Row == rows - 1 || BeamHasTraveledThisEmptySpaceBefore(beam.Row + 1, beam.Col, beam.Direction))
						{
							disappearingBeamIndices.Add(iBeam);
						}
						else
						{
							beam.Row++;
						}

						break;
					case BeamDirection.Left:
						if (beam.Col == 0 || BeamHasTraveledThisEmptySpaceBefore(beam.Row, beam.Col - 1, beam.Direction))
						{
							disappearingBeamIndices.Add(iBeam);
						}
						else
						{
							beam.Col--;
						}

						break;
				}

				var newPosition = input[beam.Row][beam.Col];

				if (beam.Direction == BeamDirection.Up)
				{
					switch (newPosition)
					{
						case '/':
							beam.Direction = BeamDirection.Right;
							continue;
						case '\\':
							beam.Direction = BeamDirection.Left;
							continue;
						case '-':
							beam.Direction = BeamDirection.Left;
							emergingBeams.Add(new Beam(beam.Row, beam.Col, BeamDirection.Right));
							continue;
					}
				}
				else if (beam.Direction == BeamDirection.Right)
				{
					switch (newPosition)
					{
						case '/':
							beam.Direction = BeamDirection.Up;
							continue;
						case '\\':
							beam.Direction = BeamDirection.Down;
							continue;
						case '|':
							beam.Direction = BeamDirection.Down;
							emergingBeams.Add(new Beam(beam.Row, beam.Col, BeamDirection.Up));
							continue;
					}
				}
				else if (beam.Direction == BeamDirection.Down)
				{
					switch (newPosition)
					{
						case '/':
							beam.Direction = BeamDirection.Left;
							continue;
						case '\\':
							beam.Direction = BeamDirection.Right;
							continue;
						case '-':
							beam.Direction = BeamDirection.Right;
							emergingBeams.Add(new Beam(beam.Row, beam.Col, BeamDirection.Left));
							continue;
					}
				}
				else if (beam.Direction == BeamDirection.Left)
				{
					switch (newPosition)
					{
						case '/':
							beam.Direction = BeamDirection.Down;
							continue;
						case '\\':
							beam.Direction = BeamDirection.Up;
							continue;
						case '|':
							beam.Direction = BeamDirection.Up;
							emergingBeams.Add(new Beam(beam.Row, beam.Col, BeamDirection.Down));
							continue;
					}
				}
			}

			foreach (var disappearingBeamIndex in disappearingBeamIndices.OrderByDescending(index => index))
			{
				activeBeams.RemoveAt(disappearingBeamIndex);
			}

			foreach (var emergingBeam in emergingBeams)
			{
				activeBeams.Add(emergingBeam);
			}
		}

		var energizedTileCount = energizedMirrorsAndSplitters.Count + beamDirectionsByPosition.Count;

		return energizedTileCount;
	}

	public object GetPart2Solution(string[] input, bool isExampleInput)
	{
		return 0;
	}

	private class Beam
	{
		public Beam(int row, int col, BeamDirection direction)
		{
			Row = row;
			Col = col;
			Direction = direction;
		}

		public int Row { get; set; }
		public int Col { get; set; }
		public BeamDirection Direction { get; set; }
	}

	private enum BeamDirection
	{
		Up,
		Right,
		Down,
		Left,
	}
}