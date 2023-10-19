using MvCamCtrl.NET.CameraParams;
using IDust.Base;

namespace IDust.Vision.Camera;

public class CameraBase
{
    #region member
    protected CameraInitParma initParma;
    protected CameraLightParma lightParma;
    public event D_void_void_Parma ECameraIsRunned;
    public event D_void_int_string_int_Parma ECameraSendMessage;
    public event D_void_int_bool_Parma ECameraConnectStatusChanged;
    #endregion

    #region ctor
    public CameraBase(in CameraInitParma initParma,in CameraLightParma lightParma)
    {
        this.initParma = initParma;
        this.lightParma = lightParma;
        ECameraIsRunned += OnCameraIsRunned;
        ECameraSendMessage += OnCameraSendMessage;
        ECameraConnectStatusChanged += OnCameraConnectStatusChanged;
        SelectFloderPath = string.Empty;
        DeviceListAcqUserName = new List<string>();
    }

    public CameraBase(in CameraInitParma initParma)
    {
        this.initParma = initParma;
        lightParma = new CameraLightParma();
        ECameraIsRunned += OnCameraIsRunned;
        ECameraSendMessage += OnCameraSendMessage;
        ECameraConnectStatusChanged += OnCameraConnectStatusChanged;
        SelectFloderPath = string.Empty;
        DeviceListAcqUserName = new List<string>();
    }
    #endregion

    #region property
    public bool IsConnect { get; protected set; }
    public virtual bool IsLive { get; protected set; }
    public bool IsImageComplete { get; protected set; }
    public int PhotoTimeout { get; protected set; }
    public int OncePhotoTime { get; protected set; }
    public string CameraConnectionId { get { return initParma.ConnectionId; } protected set { initParma.ConnectionId = value; } }
    public string SelectFloderPath { get; protected set; }
    public virtual List<string> DeviceListAcqUserName { get; protected set; }
    #endregion

    #region event callback methods
    protected virtual void OnCameraIsRunned()
    {
        
    }

    protected virtual void OnCameraSendMessage(int type, string msg, int value)
    {
        
    }

    protected virtual void OnCameraConnectStatusChanged(int userData, bool value)
    {
        
    }
    #endregion

    #region virtual methods
    public virtual RunResult Connect()
    {
        throw new NotImplementedException();
    }

    public virtual RunResult Connect(string cameraConnectId)
    {
        throw new NotImplementedException();
    }

    public virtual RunResult Disconnect()
    {
        throw new NotImplementedException();
    }

    public virtual RunResult TaskPicture()
    {
        throw new NotImplementedException();
    }

    public virtual RunResult CameraStartVideo()
    {
        throw new NotImplementedException();
    }


    public virtual RunResult CameraStopVideo()
    {
        throw new NotImplementedException();
    }

    public virtual RunResult CameraSaveParma()
    {
        throw new NotImplementedException();
    }

    public virtual RunResult GetIoStatus(uint chanel,out bool value)
    {
        throw new NotImplementedException();
    }

    public virtual RunResult CameraSetParma(in CameraLightParma parma)
    {
        throw new NotImplementedException();
    }
    #endregion
}

