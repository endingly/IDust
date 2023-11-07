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

// /// <summary>
// /// 扫码类型
// /// </summary>
// public enum CodeType
// {
//     /// <summary>
//     /// 二维码
//     /// </summary>
//     QRCode,
//     /// <summary>
//     /// 条形码
//     /// </summary>
//     BarCode,
//     /// <summary>
//     /// 混合类型
//     /// </summary>
//     Mixed,
// }

/// <summary>
/// 扫码工具的参数类,包含了所有的扫码动作
/// </summary>
public class ScanCodeToolParma : IDisposable
{
    /// <summary>
    /// 工具的名字
    /// </summary>
    public string ToolName { get; set; } = "DefaultToolName";
    /// <summary>
    /// 区域个数
    /// </summary>
    public int RegionCount { get; set; } = 1;
    /// <summary>
    /// 扫码动作列表
    /// </summary>
    public List<CodeActionBase> Actions { get; set; }
    /// <summary>
    /// 是否启用至少寻找条码数量
    /// </summary>
    public bool UseFindLeastEnable { get; set; } = false;

    /// <summary>
    /// 默认构造函数，默认扫描一维码
    /// </summary>
    public ScanCodeToolParma()
    {
        Actions = [new BarCodeAction()];
    }

    public void Dispose()
    {
        Actions.ForEach(x => x.Dispose());
        Actions.Clear();
    }
}