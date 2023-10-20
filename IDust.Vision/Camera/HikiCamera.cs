using HalconDotNet;
using IDust.Base;
using MvCamCtrl.NET;
using MvCamCtrl.NET.CameraParams;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace IDust.Vision.Camera;

public class HikiCamera : CameraBase
{
    private CCamera? camera;
    private cbOutputdelegate? output;
    private cbExceptiondelegate? exception;
    private HObject? image;
    private MV_CC_DEVICE_INFO cameraInfo;
    private Logger logger = Log.GetLogger(LogKeyword.Camera);

    public HikiCamera(in CameraInitParma initParma) : base(initParma)
    {
        
    }

    private bool Init(IntPtr ptr, 
                      ref MvCamera.MV_FRAME_OUT_INFO_EX frameInfo, 
                      IntPtr ptr1)
    {
        
    }

    public override RunResult Connect()
    {
        List<CCameraInfo> deviceInfoList = new List<CCameraInfo>();
        // 枚举相机
        int rvalue = CSystem.EnumDevices(MvCamera.MV_GIGE_DEVICE | MvCamera.MV_USB_DEVICE, ref deviceInfoList);
        if (camera != null && camera.IsDeviceConnected())
        {
            // TODO
            camera.CloseDevice();
        }
        if (rvalue == CErrorDefine.MV_OK)
        {
            string str = string.Empty;
            for (int i = 0; i < deviceInfoList.Count; i++)
            {
                switch (deviceInfoList[i].nTLayerType)
                {
                    case CSystem.MV_GIGE_DEVICE:
                        var device = (CGigECameraInfo)deviceInfoList[i];
                        str = device.UserDefinedName != "" ? device.chSerialNumber : device.UserDefinedName;
                        if (str == this.CameraConnectionId)
                        {
                            camera.CreateHandleWithoutLog(ref deviceInfoList[i]);
                        }
                        break;
                    case CSystem.MV_USB_DEVICE:
                        var device1 = (CUSBCameraInfo)deviceInfoList[i];
                        str = device1.UserDefinedName != "" ? device1.chSerialNumber : device1.UserDefinedName;
                        break;
                    default:
                        break;
                }
            }
        }
            
        

    }
}

