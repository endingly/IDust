using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDust.Vision.Camera;

/// <summary>
/// 相机类型
/// </summary>
public enum CameraType
{
    /// <summary>
    /// 海康
    /// </summary>
    HikiVision
}

/// <summary>
/// 相机频闪类型
/// <para>[拍照]-[视频]</para>
/// </summary>
public enum StrobeType
{
    /// <summary>
    /// 无
    /// </summary>
    None,
    /// <summary>
    /// 拍照时自动，视频时手动打开
    /// </summary>
    Auto_HandMovement,
    /// <summary>
    /// 常亮
    /// </summary>
    Always,
    /// <summary>
    /// 拍照时手动打开，视频时自动
    /// </summary>
    HandMovement_Always
}

/// <summary>
/// 相机初始化参数
/// </summary>
public struct CameraInitParma
{
    /// <summary>
    /// 相机类型
    /// <para>默认值：<c>CameraType.HikiVision</c></para>
    /// </summary>
    public CameraType CameraType = CameraType.HikiVision;
    /// <summary>
    /// 显示名称
    /// <para>默认值：<c>"Camera1"</c></para>
    /// </summary>
    public string ShowName = "Camera1";
    /// <summary>
    /// 连接ID
    /// <para>默认值：<c>"Camera1"</c></para>
    /// </summary>
    public string ConnectionId = "Camera1";
    /// <summary>
    /// 拍摄图像的宽度
    /// <para>默认值：<c>3072</c></para>
    /// </summary>
    public int ImageWidth = 3072;
    /// <summary>
    /// 拍摄图像的高度
    /// <para>默认值：<c>2048</c></para>
    /// </summary>
    public int ImageHeight = 2048;
    /// <summary>
    /// 用户自定义索引，可选
    /// </summary>
    public int CustomIndex = 0;
    /// <summary>
    /// 频闪类型
    /// <para>默认值：<c>StrobeType.None</c></para>
    /// </summary>
    public StrobeType StrobeType = StrobeType.None;
    /// <summary>
    /// 拍摄延时
    /// <para>默认值：<c>500 ms</c></para>
    /// </summary>
    public int PhotoDeley = 500;
    /// <summary>
    /// 硬件触发使能
    /// <para>默认值：<c>false</c></para>
    /// </summary>
    public bool HardwareTriggerEnable = false;
    /// <summary>
    /// 是否彩色相机
    /// <para>默认值：<c>false</c></para>
    /// </summary>
    public bool IsColorCamera = false;

    public CameraInitParma()
    {
    }
}

/// <summary>
/// 相机使用时候的常用参数
/// </summary>
public struct CameraParma
{
    /// <summary>
    /// 曝光时间
    /// <para>默认值：<c>5000 ms</c></para>
    /// </summary>
    public float CameraExposureTime = 5000;
    /// <summary>
    /// 增益
    /// <para>默认值：<c>0</c></para>
    /// </summary>
    public float CameraGain = 0;
    /// <summary>
    /// 帧率
    /// <para>默认值：<c>27 fps</c></para>
    /// </summary>
    public float CameraFrameRate = 27;

    public CameraParma() { }
}
