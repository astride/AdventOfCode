namespace Common.Models;

public class XYZ : XY
{
	public XYZ(int x, int y, int z) : base(x, y)
	{
		Z = z;
	}

	public int Z { get; }

	public XYZ RightOf() => ShiftedAlongX(1);
	public XYZ LeftOf() => ShiftedAlongX(-1);
	public XYZ Above() => ShiftedAlongY(1);
	public XYZ Below() => ShiftedAlongY(-1);
	public XYZ InFrontOf() => ShiftedAlongZ(1);
	public XYZ Behind() => ShiftedAlongZ(-1);
	
	private XYZ ShiftedAlongX(int by) => new(X + by, Y, Z);
	private XYZ ShiftedAlongY(int by) => new(X, Y + by, Z);
	private XYZ ShiftedAlongZ(int by) => new(X, Y, Z + by);

	public override bool Equals(object? obj)
	{
		if (obj is XYZ other)
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
