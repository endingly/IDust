using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDust.Calibrating;
/// <summary>
/// 2维向量
/// </summary>
public struct Vector
{
    /// <summary>
    /// x 分量
    /// </summary>
    public double x;

    /// <summary>
    /// x 分量
    /// </summary>
    public double y;

    /// <summary>
    /// 模
    /// </summary>
    public readonly double Magnitude => Math.Sqrt(x * x + y * y);

    /// <summary>
    /// 初始化构造函数
    /// </summary>
    public Vector(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// 以两个点构造向量
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public Vector(in Point start, in Point end)
    {
        x = end.x - start.x;
        y = end.y - start.y;
    }
}

