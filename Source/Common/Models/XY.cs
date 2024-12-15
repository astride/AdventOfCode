namespace Common.Models;

public class XY
{
    protected XY(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }

    public int Y { get; }

    public override string ToString() => $"({X}, {Y})";
}
