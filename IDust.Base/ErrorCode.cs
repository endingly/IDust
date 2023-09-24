namespace IDust.Base;

/// <summary>
/// 错误代码
/// </summary>
public enum ErrorCode
{
    Unkown,
    OK,
    Fail,
}

public static class ErrorCodeExtension
{
    /// <summary>
    /// 转换为消息
    /// </summary>
    /// <param name="ec"></param>
    /// <returns></returns>
    public static string ToMessage(this ErrorCode ec)
    {
        return ec switch
        {
            ErrorCode.Unkown => "未知错误",
            ErrorCode.OK => "成功",
            ErrorCode.Fail => "失败",
            _ => "未知错误",
        };
    }
}