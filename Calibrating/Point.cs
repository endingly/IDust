using HalconDotNet;
namespace IDust.Calibrating;

public struct Point
{
    public double x;
    public double y;
}

public struct Line
{
    public Point start;
    public Point end;
}
