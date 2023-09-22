using HalconDotNet;
namespace IDust.Calibrating;

/// <summary>
/// 点
/// </summary>
public struct Point
{
    /// <summary>
    /// x 坐标
    /// </summary>
    public double x;

    /// <summary>
    /// y 坐标
    /// </summary>
    public double y;
}

/// <summary>
/// 线
/// </summary>
public struct Line
{
    /// <summary>
    /// 起点
    /// </summary>
    public Point start;

    /// <summary>
    /// 终点
    /// </summary>
    public Point end;
}
