using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDust.Vision.Tools.CodeScan;

/// <summary>
/// 条形码特有参数
/// </summary>
public class ScanBarCodeParma : ScanCodeParmaBase
{
    /// <summary>
    /// 对比度
    /// </summary>
    public double BarMeasThresh;
    /// <summary>
    /// 绝对对比度
    /// </summary>
    public double BarMeasThreshAbs;
    /// <summary>
    /// 目标大小
    /// </summary>
    public double BarElementSize;
    /// <summary>
    /// 条码类型
    /// </summary>
    public BarCodeType BarCodeType;
}

/// <summary>
/// 条形码类型
/// </summary>
public enum BarCodeType
{
    auto,
    Code128,
}
