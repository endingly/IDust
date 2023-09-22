namespace IDust.Calibrating;

/// <summary>
/// 相机的标定类型
/// </summary>
public enum CalibratType
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