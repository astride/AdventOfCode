namespace Common.Models;

public class Coordinate : XY
{
    public Coordinate(int x, int y) : base(x, y) { }

    public static Coordinate Origin => new(0, 0);

    public override bool Equals(object? obj)
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
