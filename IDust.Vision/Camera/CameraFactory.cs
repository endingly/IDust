namespace IDust.Vision.Camera;

public static class CameraFactory
{
    public static T? CreatCamera<T>(CameraInitParma parma) where T : CameraBase
    {
        switch (parma.CameraType)
        {
            case CameraType.HikiVision:
                return new HikiCamera(parma) as T;
            default:
                return new HikiCamera(parma) as T;
        }
    }
}

