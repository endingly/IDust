using HalconDotNet;
using IDust.Base;
using MvCamCtrl.NET;
using MvCamCtrl.NET.CameraParams;
using System.Runtime.InteropServices;
using System.Text;

namespace IDust.Vision.Camera;

public enum HikiCommandType
{
    Command,
    Bool,
    Int,
    Float,
    Enum
}

/// <summary>
/// 此结构体由于精细化了分布
/// <para><c>bool/lonog/float/uint</c> 共用一段内存</para>
/// <para>故而它总长度是 <c>8 byte</c></para>
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public struct HikiCommandValue
{
    [FieldOffset(0)]
    public bool BoolValue;
    [FieldOffset(0)]
    public long LongValue;
    [FieldOffset(0)]
    public float FloatValue;
    [FieldOffset(0)]
    public uint EnumValue;

    public HikiCommandValue()
    {
        this.LongValue = 0;
    }

    public HikiCommandValue(bool value)
    {
        this.BoolValue = value;
    }

    public HikiCommandValue(long value)
    {
        this.LongValue = value;
    }

    public HikiCommandValue(float value)
    {
        this.FloatValue = value;
    }

    public HikiCommandValue(uint value)
    {
        this.EnumValue = value;
    }

    public static implicit operator bool(HikiCommandValue value)
    {
        return value.BoolValue;
    }

    public static implicit operator long(HikiCommandValue value)
    {
        return value.LongValue;
    }

    public static implicit operator uint(HikiCommandValue value)
    {
        return value.EnumValue;
    }

    public static implicit operator float(HikiCommandValue value)
    {
        return value.FloatValue;
    }
}

/// <summary>
/// 海康相机的高级参数设置结构体
/// </summary>
public struct HikiCameraExpertParma
{
    /// <summary>
    /// 命令类型
    /// </summary>
    public HikiCommandType CommandType;
    /// <summary>
    /// 命令名称 <c>key</c>
    /// </summary>
    public string CommandName;
    /// <summary>
    /// 命令值 <c>value</c>
    /// </summary>
    public HikiCommandValue CommandValue;
}

public class HikiCamera : CameraBase
{
    /// <summary>
    /// 相机控制句柄本体
    /// </summary>
    private CCamera? camera = new();
    private cbOutputExdelegate? output;
    private cbExceptiondelegate? exception;
    /// <summary>
    /// 拍摄所得到图片
    /// </summary>
    private HImage? image;
    private Logger logger = Log.GetLogger(LogKeyword.Camera);
    /// <summary>
    /// 是否设置频闪
    /// </summary>
    private bool IsSetStorbe = false;

    /// <summary>
    /// 高级参数列表，应构造函数中提供
    /// </summary>
    private Dictionary<string, HikiCameraExpertParma> expertParmas = new Dictionary<string, HikiCameraExpertParma>();

    public HikiCamera(in CameraInitParma initParma) : base(initParma)
    {
        output = OutputCallback;
        exception = ExceptionCallback;
    }

    #region private methods
    private void OutputCallback(IntPtr pData, ref MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
    {
        switch (pFrameInfo.enPixelType)
        {
            case MvGvspPixelType.PixelType_Gvsp_Mono8:
                var tmp = new HObject();
                HOperatorSet.GenImage1(out tmp, "byte", pFrameInfo.nWidth, pFrameInfo.nHeight, pData);
                image = (HImage)tmp;
                break;
            default:
                HOperatorSet.GenImageInterleaved(out tmp,
                                                 pData,
                                                 "bgr",
                                                 pFrameInfo.nWidth, pFrameInfo.nHeight,
                                                 -1,
                                                 "byte",
                                                 pFrameInfo.nWidth, pFrameInfo.nHeight,
                                                 0, 0, -1, 0);
                image = (HImage)tmp;
                break;
        }
        IsImageComplete = true;
        // TODO: display image
        // TODO: callback -> IsRunned()
    }
    private void ExceptionCallback(uint nMsgType, nint pUser)
    {
        this.IsConnect = false;
        // TODO: camera will send message
        var t = new Thread(() =>
        {
            this.Disconnect();
            RunResult r;
            do
            {
                r = Connect();
                switch (r.isSuccess)
                {
                    case true:
                        // TODO: camera will send message
                        break;
                    case false:
                        Thread.Sleep(1000);
                        break;
                }
            } while (r.isSuccess && Core.IsApplicationRunning);
        });
        t.IsBackground = true;
        t.Start();
    }
    #endregion

    #region override base
    public override RunResult Connect()
    {
        List<CCameraInfo> deviceInfoList = new List<CCameraInfo>();
        // 枚举相机
        int rvalue = CSystem.EnumDevices(CSystem.MV_GIGE_DEVICE | CSystem.MV_USB_DEVICE, ref deviceInfoList);
        bool isFind = false;
        if (camera != null && camera.IsDeviceConnected())
        {
            // TODO
            Disconnect();
        }
        if (rvalue == CErrorDefine.MV_OK)
        {
            string str = string.Empty;
            CCameraInfo tmp2image = new CCameraInfo();
            for (int i = 0; i < deviceInfoList.Count; i++)
            {
                switch (deviceInfoList[i].nTLayerType)
                {
                    case CSystem.MV_GIGE_DEVICE:
                        var device = (CGigECameraInfo)deviceInfoList[i];
                        tmp2image = deviceInfoList[i];
                        str = device.UserDefinedName != "" ? device.chSerialNumber : device.UserDefinedName;
                        break;
                    case CSystem.MV_USB_DEVICE:
                        var device1 = (CUSBCameraInfo)deviceInfoList[i];
                        tmp2image = deviceInfoList[i];
                        str = device1.UserDefinedName != "" ? device1.chSerialNumber : device1.UserDefinedName;
                        break;
                }
                if (str == this.CameraConnectionId && camera != null)
                {
                    if (camera.CreateHandleWithoutLog(ref tmp2image) == CErrorDefine.MV_OK)
                    {
                        if (camera.OpenDevice() == CErrorDefine.MV_OK)
                        {
                            camera.SetEnumValue("AcquisitionMode", (uint)MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
                            camera.SetEnumValue("TriggerMode", (uint)MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
                            // 设置触发源
                            this.output = new cbOutputExdelegate(OutputCallback);
                            camera.RegisterImageCallBackEx(output, IntPtr.Zero);
                            this.exception = new cbExceptiondelegate(ExceptionCallback);
                            camera.RegisterExceptionCallBack(exception, 1);
                            if (initParma.HardwareTriggerEnable)
                                camera.SetEnumValue("TriggerSource", (uint)MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
                            else
                                camera.SetEnumValue("TriggerSource", (uint)MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
                            camera.SetEnumValue("ExposureAuto", (uint)MV_CAM_EXPOSURE_AUTO_MODE.MV_EXPOSURE_AUTO_MODE_OFF);
                            camera.SetEnumValue("GainAuto", (uint)MV_CAM_GAIN_MODE.MV_GAIN_MODE_OFF);
                            camera.StartGrabbing();

                            camera.SetIntValue("GevHeartbeatTimeout", 3000U);
                            // TODO: a bool flag
                            switch (initParma.StrobeType)
                            {
                                case StrobeType.Auto_HandMovement:
                                    camera.SetEnumValue("LineSelector", (uint)MV_CAM_GAMMA_SELECTOR.MV_GAMMA_SELECTOR_USER);
                                    camera.SetBoolValue("StrobeEnable", true);
                                    camera.SetBoolValue("LineInverter", false);
                                    break;
                                case StrobeType.Always:
                                    camera.SetEnumValue("LineSelector", (uint)MV_CAM_GAMMA_SELECTOR.MV_GAMMA_SELECTOR_USER);
                                    camera.SetBoolValue("StrobeEnable", false);
                                    camera.SetBoolValue("LineInverter", true);
                                    break;
                                case StrobeType.HandMovement_Always:
                                    camera.SetEnumValue("LineSelector", (uint)MV_CAM_GAMMA_SELECTOR.MV_GAMMA_SELECTOR_USER);
                                    camera.SetBoolValue("StrobeEnable", false);
                                    camera.SetBoolValue("LineInverter", false);
                                    break;
                            }
                            this.IsConnect = true;
                            this.IsLive = true;
                            // TODO: Trigger a Connected event
                            return new RunResult(ErrorCode.CameraConnected);
                        }
                        else
                        {
                            logger.Error(ErrorCode.CameraFailToConnect.GetString());
                            camera.DestroyHandle();
                            IsConnect = false;
                            IsLive = false;
                            return new RunResult(ErrorCode.CameraFailToConnect);
                        }
                    }
                    isFind = true;
                }
            }
        }
        if (!isFind)
            return new RunResult(ErrorCode.CameraNotFound);
        else
            return new RunResult(ErrorCode.CameraFailToConnect);
    }
    public override RunResult Connect(string cameraConnectId)
    {
        this.CameraConnectionId = cameraConnectId;
        return Connect();
    }
    public override List<string> DeviceListAcqUserName
    {
        get
        {
            List<string> result = new List<string>();
            List<CCameraInfo> cameraInfos = new List<CCameraInfo>();
            int rvalue = CSystem.EnumDevices(CSystem.MV_GIGE_DEVICE | CSystem.MV_USB_DEVICE, ref cameraInfos);
            if (rvalue == CErrorDefine.MV_OK)
            {
                for (int i = 0; i < cameraInfos.Count; i++)
                {
                    CCameraInfo c = cameraInfos[i];
                    switch (c.nTLayerType)
                    {
                        case CSystem.MV_GIGE_DEVICE:
                            var device = (CGigECameraInfo)c;
                            if (device.UserDefinedName != "")
                                result.Add(device.UserDefinedName);
                            else
                                result.Add(device.chSerialNumber);
                            break;
                        case CSystem.MV_USB_DEVICE:
                            var device1 = (CUSBCameraInfo)c;
                            if (device1.UserDefinedName != "")
                                result.Add(device1.UserDefinedName);
                            else
                                result.Add(device1.chSerialNumber);
                            break;
                    }
                }
            }
            return result;
        }
    }
    public override RunResult Disconnect()
    {
        DateTime start = DateTime.Now;
        if (camera != null)
        {
            switch (initParma.StrobeType)
            {
                case StrobeType.Auto_HandMovement:
                    camera.SetEnumValue("LineSelector", 1U);
                    camera.SetBoolValue("StrobeEnable", true);
                    camera.SetBoolValue("LineInverter", false);
                    break;
                case StrobeType.Always:
                    camera.SetEnumValue("LineSelector", 1U);
                    camera.SetBoolValue("StrobeEnable", false);
                    camera.SetBoolValue("LineInverter", false);
                    break;
                case StrobeType.HandMovement_Always:
                    camera.SetEnumValue("LineSelector", 1U);
                    camera.SetBoolValue("StrobeEnable", false);
                    camera.SetBoolValue("LineInverter", false);
                    break;
            }
            camera.SetEnumValue("TriggerMode", (uint)MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
            int rflag;
            do
            {
                rflag = camera.StopGrabbing();
            } while (rflag != CErrorDefine.MV_OK && (DateTime.Now - start).TotalMilliseconds < 3000);
            if (camera.CloseDevice() != CErrorDefine.MV_OK)
                return new RunResult(ErrorCode.CameraFailToDisconnect);
            else if (camera.DestroyHandle() != CErrorDefine.MV_OK)
                return new RunResult(ErrorCode.CameraFailToDisconnect, "Fail ti Unregister camera.");
            else
            {
                IsConnect = false;
                IsLive = false;
                camera = null;
                // TODO: 两个事件出发
                // CameraLiveStatuesChangedFun CameraConnectStatuesChangedFun
                return new RunResult(ErrorCode.CameraDisconnected);
            }
        }
        else
            return new RunResult(ErrorCode.CameraDisconnected, "Camera point is null.");
    }
    public override RunResult TakePicture()
    {
        DateTime start = DateTime.Now;
        var result = new RunResult();
        if (camera != null)
        {
            this.IsImageComplete = false;
            try
            {
                if (!IsSetStorbe)
                {
                    camera.SetEnumValue("TriggerMode", (uint)MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
                    if (initParma.StrobeType == StrobeType.Auto_HandMovement)
                    {
                        camera.SetEnumValue("LineSelector", 1U);
                        camera.SetBoolValue("StrobeEnable", true);
                        camera.SetBoolValue("LineInverter", false);
                    }
                    // TODO: live status changed call back
                    IsLive = false;
                    IsSetStorbe = true;
                }
                if (initParma.StrobeType == StrobeType.HandMovement_Always)
                {
                    camera.SetEnumValue("LineSelector", 1U);
                    camera.SetBoolValue("StrobeEnable", false);
                    camera.SetBoolValue("LineInverter", true);
                    Thread.Sleep(30);
                }
                if (initParma.HardwareTriggerEnable)
                {
                    camera.SetEnumValue("TriggerSource", (uint)MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
                }
                Thread.Sleep(initParma.PhotoDeley);
                camera.SetCommandValue("TriggerSoftware");
                var useTime = (DateTime.Now - start).TotalMilliseconds;
                while (!IsImageComplete && useTime <= 3000)
                {
                    //TODO: 循环采集，直到成功
                }
                this.OncePhotoTime = useTime;
                if (useTime > 3000)
                {
                    result.Reset(ErrorCode.CameraTakePhotoTimeout);
                }
                else
                {
                    result.Reset(ErrorCode.CameraTakePhotoSuccess);
                }
                if (initParma.StrobeType == StrobeType.HandMovement_Always)
                {
                    camera.SetEnumValue("LineSelector", 1U);
                    camera.SetBoolValue("StrobeEnable", false);
                    camera.SetBoolValue("LineInverter", false);
                }
                if (initParma.HardwareTriggerEnable)
                {
                    camera.SetEnumValue("TriggerSource", (uint)MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ErrorCode.CameraTakePhotoFail.GetString(), ex);
                if (initParma.HardwareTriggerEnable)
                    camera.SetEnumValue("TriggerSource", (uint)MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
                result.Reset(ErrorCode.CameraTakePhotoFail);
            }

        }
        else
        {
            result.Reset(ErrorCode.CameraNotFound);
        }
        return result;
    }
    public override RunResult CameraStartVideo()
    {
        var result = new RunResult();
        if (camera != null)
        {
            switch (initParma.StrobeType)
            {
                case StrobeType.Auto_HandMovement:
                    camera.SetEnumValue("LineSelector", 1U);
                    camera.SetBoolValue("StrobeEnable", false);
                    camera.SetBoolValue("LineInverter", true);
                    break;
                case StrobeType.HandMovement_Always:
                    camera.SetEnumValue("LineSelector", 1U);
                    camera.SetBoolValue("StrobeEnable", false);
                    camera.SetBoolValue("LineInverter", true);
                    break;
            }
            var r = camera.SetEnumValue("TriggerMode", (uint)MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
            if (r == CErrorDefine.MV_OK)
            {
                IsLive = true;
                // TODO: a flag name is unkown
                // TODO: 相机状态改变事件
                result.Reset(ErrorCode.CameraStartVideoSuccess);
            }
            else
            {
                result.Reset(ErrorCode.CameraFailToStartVideo);
                logger.Error(ErrorCode.CameraFailToStartVideo.GetString());
            }

        }
        else
        {
            result.Reset(ErrorCode.CameraNotFound);
        }
        return result;
    }
    public override RunResult CameraStopVideo()
    {
        var result = new RunResult();
        if (camera != null)
        {
            var r = camera.SetEnumValue("TriggerMode", (uint)MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
            switch (initParma.StrobeType)
            {
                case StrobeType.Auto_HandMovement:
                    camera.SetEnumValue("LineSelector", 1U);
                    camera.SetBoolValue("StrobeEnable", true);
                    camera.SetBoolValue("LineInverter", false);
                    break;
                case StrobeType.HandMovement_Always:
                    camera.SetEnumValue("LineSelector", 1U);
                    camera.SetBoolValue("StrobeEnable", false);
                    camera.SetBoolValue("LineInverter", false);
                    break;
            }
            if (r == CErrorDefine.MV_OK)
            {
                IsLive = false;
                // TODO: a flag name is unkown
                // TODO: 相机状态改变事件
                result.Reset(ErrorCode.CameraStopVideoSuccess);
            }
            else
            {
                result.Reset(ErrorCode.CameraFailToStopVideo);
                logger.Error(ErrorCode.CameraFailToStopVideo.GetString());
            }
        }
        else
        {
            result.Reset(ErrorCode.CameraNotFound);
        }
        return result;
    }
    public override RunResult GetIoStatus(uint chanel, out bool value)
    {
        var result = new RunResult();
        value = false;
        try
        {
            if (camera != null)
            {
                camera.SetEnumValue("LineSelector", chanel);
                if (camera.GetBoolValue("LineStatus", ref value) == CErrorDefine.MV_OK)
                    result.Reset(ErrorCode.CameraGetIoStatusSuccess);
                else
                {
                    result.Reset(ErrorCode.CameraFailToGetIoStatus);
                    logger.Error(ErrorCode.CameraFailToGetIoStatus.GetString());
                }
            }
            else
            {
                result.Reset(ErrorCode.CameraNotFound);
            }
        }
        catch (Exception ex)
        {
            result.Reset(ErrorCode.CameraFailToGetIoStatus, ex);
            logger.Error(ErrorCode.CameraFailToGetIoStatus.GetString(), ex);
        }
        return result;
    }
    public override float ExposureTime
    {
        get
        {
            if (camera != null)
            {
                CFloatValue r = new CFloatValue();
                if (camera.GetFloatValue("ExposureTime", ref r) == CErrorDefine.MV_OK)
                {
                    return r.CurValue;
                }
            }
            return float.MinValue;
        }
        set
        {
            if (camera != null)
            {
                if (camera.SetFloatValue("ExposureTime", value) != CErrorDefine.MV_OK)
                {
                    logger.Error(ErrorCode.CameraFailToSetParma.GetString());
                }
            }
            else
            {
                logger.Error(ErrorCode.CameraNotFound.GetString());
            }
        }
    }
    public override float Gain
    {
        get
        {
            if (camera != null)
            {
                CFloatValue r = new CFloatValue();
                if (camera.GetFloatValue("Gain", ref r) == CErrorDefine.MV_OK)
                {
                    return r.CurValue;
                }
            }
            return float.MinValue;
        }
        set
        {
            if (camera != null)
            {
                if (camera.SetFloatValue("Gain", value) != CErrorDefine.MV_OK)
                {
                    logger.Error(ErrorCode.CameraFailToSetParma.GetString());
                }
            }
            else
            {
                logger.Error(ErrorCode.CameraNotFound.GetString());
            }
        }
    }
    public override float FrameRate
    {
        get
        {
            if (camera != null)
            {
                CFloatValue r = new CFloatValue();
                if (camera.GetFloatValue("AcquisitionFrameRate", ref r) == CErrorDefine.MV_OK)
                {
                    return r.CurValue;
                }
            }
            return float.MinValue;
        }
        set
        {
            if (camera != null)
            {
                if (camera.SetFloatValue("AcquisitionFrameRate", value) != CErrorDefine.MV_OK)
                {
                    logger.Error(ErrorCode.CameraFailToSetParma.GetString());
                }
            }
            else
            {
                logger.Error(ErrorCode.CameraNotFound.GetString());
            }
        }
    }
    public override RunResult CameraSaveParma()
    {
        if (camera?.SetCommandValue("UserSetSave") == CErrorDefine.MV_OK)
            return new RunResult(ErrorCode.CameraSaveParmaSuccess);
        else
            return new RunResult(ErrorCode.CameraFailToSaveParma);
    }
    public override RunResult CameraSetParma(in CameraParma parma)
    {
        RunResult flag = new RunResult();
        if (camera != null && IsConnect)
        {
            lightParma = parma;
            ExposureTime = parma.CameraExposureTime;
            Gain = parma.CameraGain;
            FrameRate = parma.CameraFrameRate;
            // TODO: set expert parma
            flag.Reset(ErrorCode.CameraSetParmaSuccess);
        }
        else
            flag.Reset(ErrorCode.CameraFailToSetParma);
        return flag;
    }
    #endregion
    public RunResult CameraSetExpertParma(in HikiCameraExpertParma parma)
    {
        RunResult result = new();
        if (camera == null)
            result.Reset(ErrorCode.CameraNotFound);
        else
        {
            camera.StopGrabbing();
            switch (parma.CommandType)
            {
                case HikiCommandType.Command:
                    if (camera.SetCommandValue(parma.CommandName) == CErrorDefine.MV_OK)
                    {
                        result.Reset(ErrorCode.CameraSetParmaSuccess);
                        expertParmas.Add(parma.CommandName, parma);
                    }
                    else
                        result.Reset(ErrorCode.CameraFailToSetParma);
                    break;
                case HikiCommandType.Bool:
                    if (camera.SetBoolValue(parma.CommandName, parma.CommandValue) == CErrorDefine.MV_OK)
                    {
                        result.Reset(ErrorCode.CameraSetParmaSuccess);
                        expertParmas.Add(parma.CommandName, parma);
                    }
                    else
                        result.Reset(ErrorCode.CameraFailToSetParma);
                    break;
                case HikiCommandType.Int:
                    if (camera.SetIntValue(parma.CommandName, parma.CommandValue) == CErrorDefine.MV_OK)
                    {
                        result.Reset(ErrorCode.CameraSetParmaSuccess);
                        expertParmas.Add(parma.CommandName, parma);
                    }
                    else
                        result.Reset(ErrorCode.CameraFailToSetParma);
                    break;
                case HikiCommandType.Float:
                    if (camera.SetFloatValue(parma.CommandName, parma.CommandValue) == CErrorDefine.MV_OK)
                    {
                        result.Reset(ErrorCode.CameraSetParmaSuccess);
                        expertParmas.Add(parma.CommandName, parma);
                    }
                    else
                        result.Reset(ErrorCode.CameraFailToSetParma);
                    break;
                case HikiCommandType.Enum:
                    if (camera.SetEnumValue(parma.CommandName, parma.CommandValue) == CErrorDefine.MV_OK)
                    {
                        result.Reset(ErrorCode.CameraSetParmaSuccess);
                        expertParmas.Add(parma.CommandName, parma);
                    }
                    else
                        result.Reset(ErrorCode.CameraFailToSetParma);
                    break;
                default:
                    result.Reset(ErrorCode.CameraFailToSetParma);
                    break;
            }
            camera.StartGrabbing();
        }
        return result;
    }
    public RunResult CameraGetExpertParma(string commandName, out HikiCameraExpertParma parma)
    {
        if (expertParmas.TryGetValue(commandName, out parma))
            return new RunResult(ErrorCode.CameraGetParmaSuccess);
        else
            return new RunResult(ErrorCode.CameraFailToGetParma);
    }

    protected override void OnCameraConnectStatusChanged(int userData, bool value)
    {
        if (value == false)
        {
            Reconnect();
        }
    }
}

