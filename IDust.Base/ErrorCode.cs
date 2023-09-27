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

    // Success
    PlcConnected = 1000,
    PlcReconnectSuccess,
    PlcInitSuccess,
    PlcReadSuccess,
    PlcWriteSuccess,
    PlcClearDataSuccess,
    PlcDisconnectSuccess,
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

            ErrorCode.PlcConnected => "PLC 已连接.",
            ErrorCode.PlcReconnectSuccess => "PLC 重连成功.",
            ErrorCode.PlcInitSuccess => "PLC 初始化成功.",
            ErrorCode.PlcReadSuccess => "PLC 读取数据成功.",
            ErrorCode.PlcWriteSuccess => "PLC 写入数据成功.",
            ErrorCode.PlcClearDataSuccess => "PLC 清空数据成功.",
            ErrorCode.PlcDisconnectSuccess => "PLC 断开连接成功.",
            _ => "未知错误.",
        };
    }
}