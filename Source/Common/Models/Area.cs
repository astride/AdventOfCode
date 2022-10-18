namespace Common.Models;

public class Area
{
    public Area(Coordinate min, Coordinate max)
    {
        Min = min;
        Max = max;
    }

    public Coordinate Min { get; }

    public Coordinate Max { get; }
}
