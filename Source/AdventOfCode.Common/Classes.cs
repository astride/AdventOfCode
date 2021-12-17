namespace AdventOfCode.Common.Classes
{
	public class Area
	{
		public Area(Coordinate min, Coordinate max)
		{
			Min = min;
			Max = max;
		}

		public Coordinate Min { get; set; }

		public Coordinate Max { get; set; }
	}

	public class Coordinate : XY
	{
		public Coordinate(int x, int y) : base(x, y) { }

		public static Coordinate Origo => new Coordinate(0, 0);

		public override bool Equals(object obj)
        {
            if (obj is Coordinate other)
            {
                return other.X == X && other.Y == Y;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (X + '_' + Y).GetHashCode();
        }
    }

	public class Velocity : XY
	{
		public Velocity(int x, int y) : base(x, y) { }
	}

	public class XY
	{
		public XY(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int X { get; set; }

		public int Y { get; set; }
	}
}
