namespace IDust.Base;

/// <summary>
/// 错误代码
/// </summary>
public enum ErrorCode : short
{
    /// <summary>
    /// 正常
    /// </summary>
    Normal,
    Unkown,

    // Error
    PlcNotConnect = 100,
    PlcFailToReconnect,
    PlcFailToInit,
    PlcFailToRead,
    PlcFailToWrite,
    PlcFailToClearData,
    PlcFailToDisconnect,
    PlcSetStationFail,

    SerialPortFailToRead,
    SerialPortFailToWrite,

    LightControllerFailToConnect,
    LightControllerFailToDisconnect,

    CameraFailToConnect,
    CameraFailToDisconnect,
    CameraNotFound,
    CameraTakePhotoTimeout,
    CameraTakePhotoFail,
    CameraFailToStartVideo,
    CameraFailToStopVideo,
    CameraFailToGetIoStatus,
    CameraFailToSetParma,

    // Success
    PlcConnected = 1000,
    PlcReconnectSuccess,
    PlcInitSuccess,
    PlcReadSuccess,
    PlcWriteSuccess,
    PlcClearDataSuccess,
    PlcDisconnectSuccess,
    PlcSetStationSuccess,

    SerialPortReadSuccess,
    SerialPortWriteSuccess,

    LightControllerConnected,
    LightControllerDisconnected,

    CameraConnected,
    CameraDisconnected,
    CameraTakePhotoSuccess,
    CameraStartVideoSuccess,
    CameraStopVideoSuccess,
    CameraGetIoStatusSuccess,
    CameraSetParmaSuccess,
    CameraSaveParmaSuccess,
    CameraFailToSaveParma
}

public static class ErrorCodeExtension
{
    /// <summary>
    /// 转换为消息
    /// </summary>
    /// <param name="ec"></param>
    /// <returns></returns>
    public static string GetString(this ErrorCode ec)
    {
        return ec switch
        {
            ErrorCode.Normal => "运行正常.",
            ErrorCode.Unkown => "未知错误.",
            ErrorCode.PlcNotConnect => "PLC 无法连接.",
            ErrorCode.PlcFailToReconnect => "PLC 重连失败.",
            ErrorCode.PlcFailToInit => "PLC 初始化失败.",
            ErrorCode.PlcFailToRead => "PLC 读取数据失败.",
            ErrorCode.PlcFailToWrite => "PLC 写入数据失败.",
            ErrorCode.PlcFailToClearData => "PLC 清空数据失败.",
            ErrorCode.PlcFailToDisconnect => "PLC 断开连接失败.",
            ErrorCode.PlcSetStationFail => "PLC 设置站号失败.",

            ErrorCode.SerialPortFailToRead => "串口读取数据失败.",
            ErrorCode.SerialPortFailToWrite => "串口写入数据失败.",

            ErrorCode.LightControllerFailToConnect => "光源控制器连接失败.",

            ErrorCode.CameraFailToConnect => "相机连接失败.",
            ErrorCode.CameraFailToDisconnect => "相机断开连接失败.",
            ErrorCode.CameraNotFound => "相机未找到.",
            ErrorCode.CameraTakePhotoTimeout => "相机拍照超时.",
            ErrorCode.CameraTakePhotoFail => "相机拍照失败.",
            ErrorCode.CameraFailToStartVideo => "相机开启视频失败.",
            ErrorCode.CameraFailToStopVideo => "相机停止视频失败.",
            ErrorCode.CameraFailToGetIoStatus => "相机获取IO状态失败.",
            ErrorCode.CameraFailToSetParma => "相机设置参数失败.",
            ErrorCode.CameraFailToSaveParma => "相机保存参数失败.",

            ErrorCode.PlcConnected => "PLC 已连接.",
            ErrorCode.PlcReconnectSuccess => "PLC 重连成功.",
            ErrorCode.PlcInitSuccess => "PLC 初始化成功.",
            ErrorCode.PlcReadSuccess => "PLC 读取数据成功.",
            ErrorCode.PlcWriteSuccess => "PLC 写入数据成功.",
            ErrorCode.PlcClearDataSuccess => "PLC 清空数据成功.",
            ErrorCode.PlcDisconnectSuccess => "PLC 断开连接成功.",
            ErrorCode.PlcSetStationSuccess => "PLC 设置站号成功.",

            ErrorCode.SerialPortReadSuccess => "串口读取数据成功.",
            ErrorCode.SerialPortWriteSuccess => "串口写入数据成功.",

            ErrorCode.LightControllerConnected => "光源控制器连接成功.",

            ErrorCode.CameraConnected => "相机连接成功.",
            ErrorCode.CameraDisconnected => "相机断开连接成功.",
            ErrorCode.CameraTakePhotoSuccess => "相机拍照成功.",
            ErrorCode.CameraStartVideoSuccess => "相机开启视频成功.",
            ErrorCode.CameraStopVideoSuccess => "相机停止视频成功.",
            ErrorCode.CameraGetIoStatusSuccess => "相机获取IO状态成功.",
            ErrorCode.CameraSetParmaSuccess => "相机设置参数成功.",
            ErrorCode.CameraSaveParmaSuccess => "相机保存参数成功.",
            _ => "未知错误.",
        };
    }
}