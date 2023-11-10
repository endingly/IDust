namespace IDust.Vision.Camera;

public static class CameraFactory
{
    public static T? CreatCamera<T>(CameraInitParma parma) where T : CameraBase
    {
        return parma.CameraType switch
        {
            CameraType.HikiVision => new HikiCamera(parma) as T,
            _ => new HikiCamera(parma) as T,
        };
    }
}

