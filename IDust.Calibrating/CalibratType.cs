namespace IDust.Calibrating;

/// <summary>
/// 相机的标定类型
/// </summary>
public enum CalibrateType
{
    OnlyNine,

    /// <summary>
    /// 中心固定
    /// </summary>
    FixCenter,

    /// <summary>
    /// 中心移动
    /// </summary>
    MoveCenter,

    /// <summary>
    /// 旋转中心固定
    /// </summary>
    FixRotate,

    /// <summary>
    /// 旋转中心移动
    /// </summary>
    MoveRotate,

    /// <summary>
    /// 固定相机移动
    /// </summary>
    FixCamera,

    /// <summary>
    /// 移动相机标定
    /// </summary>
    MoveCamera,

    /// <summary>
    /// 全部
    /// </summary>
    All
}

/// <summary>
/// 对于标定的应用方法
/// </summary>
public enum CalibrateApplyType
{
    /// <summary>
    /// 原模板的仿射变换
    /// </summary>
    Source_HomMat,

    /// <summary>
    /// 新入图像的仿射变换
    /// </summary>
    New_HomMat,

    /// <summary>
    /// 原仿射变化加偏移量
    /// </summary>
    Source_HomMat_Offset,

    /// <summary>
    /// 新仿射变化加偏移量
    /// </summary>
    New_HomMat_Offset
}