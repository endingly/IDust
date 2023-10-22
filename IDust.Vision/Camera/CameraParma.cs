using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDust.Vision.Camera;

public enum CameraType
{
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

public struct CameraInitParma
{
    public CameraType CameraType;
    public string ShowName;
    public string ConnectionId;
    public int ImageWidth;
    public int ImageHeight;
    public int CustomIndex;
    public StrobeType StrobeType;
    public int PhotoDeley;
    public bool HardwareTriggerEnable;
    public bool IsColorCamera;

    public CameraInitParma()
    {
        CameraType = CameraType.HikiVision;
        ShowName = "相机1";
        ConnectionId = "Camera1";
        ImageWidth = 3072;
        ImageHeight = 2048;
        CustomIndex = 0;
        StrobeType = StrobeType.None;
        PhotoDeley = 0;
        HardwareTriggerEnable = false;
        IsColorCamera = false;
    }
}

public struct CameraParma
{
    public float CameraExposureTime;
    public float CameraGain;
    public float CameraFrameRate;

    public CameraParma()
    {
        CameraExposureTime = 5000;
        CameraGain = 0;
        CameraFrameRate = 27;
    }
}
