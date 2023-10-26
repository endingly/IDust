using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDust.Calibrating;

/// <summary>
/// 2维线
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

    /// <summary>
    /// 初始化构造函数
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public Line(in Point start, in Point end)
    {
        this.start = start;
        this.end = end;
    }
}

