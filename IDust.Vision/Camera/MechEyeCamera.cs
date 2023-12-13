using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using IDust.Base;
using MMind.Eye;

namespace IDust.Vision.Camera;

public class MechEyeCamera : CameraBase
{
    MMind.Eye.Camera _camera;
    List<CameraInfo> camerainfos;
    public HImage? image;

    public MechEyeCamera(in CameraInitParma initParma) : base(initParma)
    {
        _camera = new MMind.Eye.Camera();
        camerainfos = MMind.Eye.Camera.DiscoverCameras();
        if (camerainfos.Count == 0)
        {
            Log.GetLogger(LogKeyword.Camera).Error(IDust.Base.ErrorCode.CameraNotFound.GetString());
        }
    }

    public MechEyeCamera(in CameraInitParma initParma, in CameraParma lightParma) : base(initParma, lightParma)
    {
        _camera = new MMind.Eye.Camera();
        camerainfos = MMind.Eye.Camera.DiscoverCameras();
        if (camerainfos.Count == 0)
        {
            Log.GetLogger(LogKeyword.Camera).Error(IDust.Base.ErrorCode.CameraNotFound.GetString());
        }
    }

    public override RunResult Connect()
    {
        if (camerainfos.Count != 0)
        {
            var error = _camera.Connect(camerainfos[0]);
            if (error.IsOK())
                return new RunResult(Base.ErrorCode.CameraConnected);
            else
                return new RunResult(Base.ErrorCode.CameraFailToConnect, error.ErrorDescription);
        }
        else
            return new RunResult(Base.ErrorCode.CameraNotFound);
    }

    /// <summary>
    /// 根据相机连接名进行连接
    /// </summary>
    /// <param name="cameraConnectId">连接名应当是IP字符串，不用包括端口号</param>
    /// <returns></returns>
    public override RunResult Connect(string cameraConnectId)
    {
        if (camerainfos.Count != 0)
        {
            CameraInfo? c = null;
            foreach (var item in camerainfos)
            {
                if (item.IpAddress == cameraConnectId)
                {
                    c = item;
                }
            }
            if (c == null)
                return new RunResult(Base.ErrorCode.CameraNotFound);
            else
            {
                var er = _camera.Connect(c);
                if (er.IsOK())
                    return new RunResult(Base.ErrorCode.CameraConnected);
                else
                    return new RunResult(Base.ErrorCode.CameraFailToConnect, er.ErrorDescription);
            }
        }
        else
            return new RunResult(Base.ErrorCode.CameraNotFound);
    }

    public override RunResult CameraSetParma(in CameraParma parma)
    {
        this.lightParma = parma;
        var parmaManager = _camera.CurrentUserSet();

        var err = parmaManager.SetFloatValue(MMind.Eye.Scanning2DSetting.ExposureTime.Name,
                                             lightParma.CameraExposureTime);
        // TODO: 最好确认一下这个相机是不是真的没有办法设置增益以及帧率
        if (err.IsOK())
            return new RunResult(Base.ErrorCode.CameraSetParmaSuccess);
        else
            return new RunResult(Base.ErrorCode.CameraFailToSetParma, err.ErrorDescription);
    }

    public override float ExposureTime 
    {
        get
        {
            double result = -1;
            var err = _camera.CurrentUserSet().GetFloatValue(MMind.Eye.Scanning2DSetting.ExposureTime.Name,
                                                             ref result);
            if (!err.IsOK())
                Base.Log.GetLogger(LogKeyword.Camera)
                    .Error(Base.ErrorCode.CameraFailToGetParma.GetString(),
                           err.ErrorDescription);
            return (float)result;
            
        }
        set
        {
            var err = _camera.CurrentUserSet().SetFloatValue(MMind.Eye.Scanning2DSetting.ExposureTime.Name,
                                                             value);
            if (!err.IsOK())
                Base.Log.GetLogger(LogKeyword.Camera)
                    .Error(Base.ErrorCode.CameraFailToSetParma.GetString(),
                           err.ErrorDescription);
        }
    }

    public override float Gain 
    { 
        get
        {
            double result = -1;
            var err = _camera.CurrentUserSet().GetFloatValue(MMind.Eye.Scanning3DSetting.Gain.Name,
                                                             ref result);
            if (!err.IsOK())
                Base.Log.GetLogger(LogKeyword.Camera)
                    .Error(Base.ErrorCode.CameraFailToGetParma.GetString(),
                           err.ErrorDescription);
            return (float)result;
        }
        set
        {
            var err = _camera.CurrentUserSet().SetFloatValue(MMind.Eye.Scanning3DSetting.Gain.Name,
                                                             value);
            if (!err.IsOK())
                Base.Log.GetLogger(LogKeyword.Camera)
                    .Error(Base.ErrorCode.CameraFailToSetParma.GetString(),
                           err.ErrorDescription);
        }
    }

    /// <summary>
    /// 此相机不支持帧率设置
    /// </summary>
    [Obsolete("the camera is not support set frame rate")]
    public override float FrameRate
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public override RunResult Disconnect()
    {
        _camera.Disconnect();
        return new RunResult(Base.ErrorCode.CameraDisconnected);
    }

    public override RunResult TakePicture()
    {
        Frame2D frame = new Frame2D();
        var er = _camera.Capture2D(ref frame);
        if (er.IsOK())
        {
            var i = frame.GetColorImage();
            HObject ri;
            HOperatorSet.GenImage1(out ri, default,
                                   i.Width(),
                                   i.Height(),
                                   i.Data());
            image = (HImage)ri;
            return new RunResult(Base.ErrorCode.CameraTakePhotoSuccess);
        }
        else
            return new RunResult(Base.ErrorCode.CameraTakePhotoFail, er.ErrorDescription);
    }

    protected override RunResult Reconnect()
    {
        camerainfos = MMind.Eye.Camera.DiscoverCameras();
        return Connect();
    }

    public RunResult<Frame3D> GetPointCloud()
    {
        Frame3D frame = new Frame3D();
        this._camera.Capture3D(ref frame);
        return new RunResult<Frame3D>(Base.ErrorCode.CameraTakePhotoSuccess, frame);
    }
}

