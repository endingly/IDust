using HalconDotNet;
using System.Reflection.Emit;

namespace IDust.Vision.Tools.CodeScan;

/// <summary>
/// 扫码结果排序
/// </summary>
public enum CodeSortType
{
    /// <summary>
    /// 不排序
    /// </summary>
    None,
    /// <summary>
    /// 从上到下
    /// </summary>
    UpToDown,
    /// <summary>
    /// 从下到上
    /// </summary>
    DownToUp,
    /// <summary>
    /// 从左到右
    /// </summary>
    LeftToRight,
    /// <summary>
    /// 从右到左
    /// </summary>
    RightToLeft
}

/// <summary>
/// 扫码类型
/// </summary>
public enum CodeType
{
    /// <summary>
    /// 二维码
    /// </summary>
    QRCode,
    /// <summary>
    /// 条形码
    /// </summary>
    BarCode,
    /// <summary>
    /// 混合类型
    /// </summary>
    Mixed,
}


/// <summary>
/// 扫码一次动作所需要的参数
/// </summary>
public class ScanCodeActionParma : IDisposable
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
    public HRegion CheckCodeRegion { get; set; }
    /// <summary>
    /// 至少需要寻找的条码数量
    /// </summary>
    public int FindLeastNumber { get; set; } = 1;
    /// <summary>
    /// 扫码动作后处理所需要的详细参数
    /// </summary>
    public ScanCodeParmaBase ScanCodeParma { get; set; }

    public ScanCodeActionParma() { }

    public ScanCodeActionParma(ScanCodeParmaBase scp, HRegion ccr)
    {
        CheckCodeRegion = ccr;
        ScanCodeParma = scp;
    }

    public void Dispose()
    {
        CheckCodeRegion.Dispose();
    }
}

/// <summary>
/// 扫码工具的参数类
/// </summary>
public class ScanCodeToolParma : IDisposable
{
    /// <summary>
    /// 工具的名字
    /// </summary>
    public string ToolName { get; set; } = string.Empty;
    /// <summary>
    /// 扫码类型
    /// </summary>
    public CodeType CodeType { get; set; } = CodeType.BarCode;
    /// <summary>
    /// 区域个数
    /// </summary>
    public int RegionCount { get; set; } = 1;
    /// <summary>
    /// 扫码动作列表
    /// </summary>
    public List<ScanCodeActionParma> ActionParmas { get; set; } = new List<ScanCodeActionParma>();
    /// <summary>
    /// 是否启用至少寻找条码数量
    /// </summary>
    public bool UseFindLeastEnable { get; set; } = false;

    public ScanCodeToolParma()
    {
        
    }

    public void Dispose()
    {
        ActionParmas.ForEach(x => x.Dispose());
        ActionParmas.Clear();
    }
}