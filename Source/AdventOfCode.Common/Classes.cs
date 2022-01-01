namespace AdventOfCode.Common.Classes
{
	public class AreaXY
	{
		public AreaXY(CoordinateXY min, CoordinateXY max)
		{
			Min = min;
			Max = max;
		}

		public CoordinateXY Min { get; set; }

		public CoordinateXY Max { get; set; }
	}

	public class CoordinateXY : XY
	{
		public CoordinateXY(int x, int y) : base(x, y) { }

		public static CoordinateXY Origo => new CoordinateXY(0, 0);

		public override bool Equals(object obj)
        {
            if (obj is CoordinateXY other)
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

	public class CoordinateXYZ : XYZ
	{
		public CoordinateXYZ(int x, int y, int z) : base(x, y, z) { }

		public static CoordinateXYZ Origo => new CoordinateXYZ(0, 0, 0);

		public override bool Equals(object obj)
		{
			if (obj is CoordinateXYZ other)
			{
				return other.X == X && other.Y == Y && other.Z == Z;
			}

			return false;
		}

		public override int GetHashCode()
		{
			return (X + '_' + Y + '_' + Z).GetHashCode();
		}
	}

	public class VelocityXY : XY
	{
		public VelocityXY(int x, int y) : base(x, y) { }
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

	public class XYZ : XY
	{
		public XYZ(int x, int y, int z) : base(x, y)
		{
			Z = z;
		}

		public int Z { get; set; }
	}
}
