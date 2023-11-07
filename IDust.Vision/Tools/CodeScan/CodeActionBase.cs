using IDust.Base;
using HalconDotNet;
using System.Text.RegularExpressions;
using System.Numerics;

namespace IDust.Vision.Tools.CodeScan;
using ScanResultPackages = List<CodeScanResult>;

/// <summary>
/// 扫描参数：条形码类型
/// </summary>
public enum CodeType
{
    /// <summary>
    /// 自动
    /// </summary>
    Auto = 0b0000,

    /// <summary>
    /// 一维码：Code 128
    /// </summary>
    BarCode_Code128 = 0b0001,

    /// <summary>
    /// 二维码：Matrix Code
    /// </summary>
    QrCode_MatrixCode = 0b0010,
}

/// <summary>
/// 条码扫码参数的基类
/// </summary>
public class CodeActionBase : IDisposable
{
    /// <summary>
    /// 扫码结果排序类型
    /// </summary>
    public CodeSortType SortType { get; set; } = CodeSortType.None;
    /// <summary>
    /// 结果正则过滤表达式
    /// </summary>
    public string CodeRegex { get; set; } = string.Empty;
    /// <summary>
    /// 条码检测区域
    /// </summary>
    public HRegion? CheckCodeRegion { get; set; } = null;
    /// <summary>
    /// 至少需要寻找的条码数量
    /// </summary>
    public int FindLeastNumber { get; set; } = 1;

    /// <summary>
    /// 条码类型
    /// </summary>
    public CodeType CodeType { get; set; } = CodeType.Auto;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 执行一次动作，会修改传入的图像
    /// </summary>
    /// <param name="image">传入的图像</param>
    /// <returns></returns>
    public virtual RunResult<ScanResultPackages> Run(ref HImage image)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 一次动作之后的后处理
    /// </summary>
    protected void AfterRun(ScanResultPackages r, ref HImage image)
    {
        HObject? ResultImage = default;
        
        var green = new HTuple(0, 255, 0);
        var red = new HTuple(255, 0, 0);
        var yellow = new HTuple(255, 255, 0);
        HObject tmp = image;
        HTuple color;
        bool flag = false;
        // 1. 按照给定的正则表达式过滤，并绘制区域
        if (CodeRegex != null)
        {
            for (int i = 0; i < r.Count; i++)
            {
                flag = Regex.Match(r[i].DataString, CodeRegex).Success;
                if (!flag)
                    color = yellow;
                else
                    color = green;

                // 在图像上显示区域
                HOperatorSet.PaintRegion(r[i].Region,
                                         tmp,
                                         out ResultImage,
                                         color,
                                         "margin");
                // 绘制中心点
                HOperatorSet.GenCrossContourXld(out HObject cross,
                                                r[i].RegionCenter_y,
                                                r[i].RegionCenter_x,
                                                30, 0);
                HOperatorSet.PaintXld(cross,
                                      ResultImage,
                                      out ResultImage,
                                      color);
                tmp = new HImage(ResultImage);
                if (!flag)
                    r.RemoveAt(i);
            }
        }
        if (ResultImage != null)
            image = new HImage(ResultImage);

        // 按照给定的排序方式排序
        r = this.SortType switch
        {
            CodeSortType.DownToUp => r.OrderBy(x => x.RegionCenter_x).ThenByDescending(y => y.RegionCenter_y).ToList(),
            CodeSortType.RightToLeft => r.OrderByDescending(x => x.RegionCenter_x).ThenBy(y => y.RegionCenter_y).ToList(),
            _ => r.OrderBy(x => x.RegionCenter_x).ThenBy(y => y.RegionCenter_y).ToList(),
        };
    }
}

