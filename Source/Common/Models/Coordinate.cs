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

    public Coordinate Add(Coordinate other)
    {
        return new Coordinate(X + other.X, Y + other.Y);
    }

    public Coordinate Subtract(Coordinate other)
    {
        return new Coordinate(X - other.X, Y - other.Y);
    }

    public Coordinate[] GetOrthogonalNeighbors() => new[]
    {
        new Coordinate(X - 1, Y),
        new Coordinate(X + 1, Y),
        new Coordinate(X, Y + 1),
        new Coordinate(X, Y - 1),
    };
}
