using System;
using System.Collections.Generic;

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

		public CoordinateXYZ Abs => new CoordinateXYZ(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));

		public double Magnitude => Abs.X * Abs.Y * Abs.Z;

		private bool IsXYZMapping => X == 0 && Y == 1 && Z == 2;
		private bool IsXZYMapping => X == 0 && Y == 2 && Z == 1;
		private bool IsYXZMapping => X == 1 && Y == 0 && Z == 2;
		private bool IsYZXMapping => X == 1 && Y == 2 && Z == 0;
		private bool IsZXYMapping => X == 2 && Y == 0 && Z == 1;
		private bool IsZYXMapping => X == 2 && Y == 1 && Z == 0;

		public IEnumerable<CoordinateXYZ> GetPossibleMappingsFor(CoordinateXYZ other)
		{
			if (X == other.X && Y == other.Y && Z == other.Z) yield return new CoordinateXYZ(0, 1, 2);
			if (X == other.X && Y == other.Z && Z == other.Y) yield return new CoordinateXYZ(0, 2, 1);
			if (X == other.Y && Y == other.X && Z == other.Z) yield return new CoordinateXYZ(1, 0, 2);
			if (X == other.Y && Y == other.Z && Z == other.X) yield return new CoordinateXYZ(1, 2, 0);
			if (X == other.Z && Y == other.X && Z == other.Y) yield return new CoordinateXYZ(2, 0, 1);
			if (X == other.Z && Y == other.Y && Z == other.X) yield return new CoordinateXYZ(2, 1, 0);
		} 

		public CoordinateXYZ MappedWith(CoordinateXYZ mapping)
		{
			return mapping switch
			{
				{ } when mapping.IsXYZMapping => new CoordinateXYZ(X, Y, Z),
				{ } when mapping.IsXZYMapping => new CoordinateXYZ(X, Z, Y),
				{ } when mapping.IsYXZMapping => new CoordinateXYZ(Y, X, Z),
				{ } when mapping.IsYZXMapping => new CoordinateXYZ(Y, Z, X),
				{ } when mapping.IsZXYMapping => new CoordinateXYZ(Z, X, Y),
				{ } when mapping.IsZYXMapping => new CoordinateXYZ(Z, Y, X),
				_ => null
			};
		}

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
		public XY(int x, int y) => (X, Y) = (x, y);

		public int X { get; set; }

		public int Y { get; set; }
	}

	public class XYZ : XY
	{
		public XYZ(int x, int y, int z) : base(x, y) { Z = z; }

		public int Z { get; set; }
	}
}
