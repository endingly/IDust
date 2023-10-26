using HalconDotNet;
namespace IDust.Calibrating;

/// <summary>
/// 2维点
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

    /// <summary>
    /// 初始化构造函数
    /// <summary>
    public Point(double x, double y)
    {
        this.x = x;
        this.y = y;
    }
}
