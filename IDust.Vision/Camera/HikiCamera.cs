using HalconDotNet;
using MvCamCtrl.NET;

namespace IDust.Vision.Camera;

public class HikiCamera : CameraBase
{
    private MvCamera camera;
    private MvCamera.cbOutputdelegate output;
    private MvCamera.cbExceptiondelegate exception;
    private HObject image;
    private MvCamera.MV_CC_DEVICE_INFO cameraInfo;

    public HikiCamera(in CameraInitParma initParma) : base(initParma)
    {
        camera = new MvCamera();
    }

    private bool Init(IntPtr ptr, 
                      ref MvCamera.MV_FRAME_OUT_INFO_EX frameInfo, 
                      IntPtr ptr1)
    {
        if ((int)frameInfo.enPixelType == 17301505)
        {
            HOperatorSet.GenImage1(out image, "byte", initParma.ImageHeight, initParma.ImageWidth, ptr);
        }

        // 创建相机对象
        var ret= camera.MV_CC_CreateDevice_NET(ref cameraInfo);
        if (ret != MvCamera.MV_OK)
        {
            return false;
        }
        // 打开相机


        return false;
    }
}

