using System.Drawing;
using HalconDotNet;

namespace IDust.Vision;

/// <summary>
/// 条码扫码结果
/// </summary>
public record class CodeScanResult : IDisposable
{
    /// <summary>
    /// 条码字符串
    /// </summary>
    public string DataString;
    /// <summary>
    /// 图片上区域中心 x 坐标
    /// </summary>
    public double RegionCenter_x;

    /// <summary>
    /// 图片上区域中心 y 坐标
    /// </summary>
    public double RegionCenter_y;
    /// <summary>
    /// 区域
    /// </summary>
    public HRegion? Region;

    public CodeScanResult(string dataStrings,
                          double x,
                          double y,
                          HRegion region)
    {
        DataString = dataStrings;
        RegionCenter_x = x;
        RegionCenter_y = y;
        Region = region;
    }

    public CodeScanResult(double x,
                          double y,
                          HRegion region)
    {
        DataString = string.Empty;
        RegionCenter_x = x;
        RegionCenter_y = y;
        Region = region;
    }

    public CodeScanResult()
    {
        DataString = string.Empty;
        RegionCenter_x = 0;
        RegionCenter_y = 0;
        Region = null;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
