using System.Reflection.Metadata;

namespace IDust.Base;

/// <summary>
/// 各动作运行的结果
/// </summary>
public class RunResult
{
    #region members
    public bool isSuccess;
    public string message;
    public ErrorCode errorCode;
    #endregion

    #region ctor
    public RunResult()
    {
        isSuccess = false;
        message = ErrorCode.Unkown.GetString();
        errorCode = ErrorCode.Unkown;
    }

    public RunResult(ErrorCode er)
    {
        message = er.GetString();
        errorCode = er;
        if (er >= ErrorCode.PlcConnected)
        {
            isSuccess = true;
        }
        else
        {
            isSuccess = false;
        }
    }

    public RunResult(ErrorCode er, Exception ex)
    {
        message = er.GetString() + "[" + ex.Message + "]";
        errorCode = er;
        if (er >= ErrorCode.PlcConnected)
        {
            isSuccess = true;
        }
        else
        {
            isSuccess = false;
        }
    }

    public RunResult(ErrorCode er, string msg)
    {
        message = er.GetString() + "->" + msg;
        errorCode = er;
        if (er >= ErrorCode.PlcConnected)
        {
            isSuccess = true;
        }
        else
        {
            isSuccess = false;
        }
    }

    public RunResult(ErrorCode er, string msg, System.Exception ex)
    {
        message = $"{er.GetString()} -> {msg} [{ex.Message}]";
        errorCode = er;
        if (er >= ErrorCode.PlcConnected)
        {
            isSuccess = true;
        }
        else
        {
            isSuccess = false;
        }
    }
    #endregion

    #region public method
    public void Reset(ErrorCode er)
    {
        message = er.GetString();
        errorCode = er;
        if (er >= ErrorCode.PlcConnected)
        {
            isSuccess = true;
        }
        else
        {
            isSuccess = false;
        }
    }

    public void Reset(ErrorCode er, Exception ex)
    {
        message = er.GetString() + "[" + ex.Message + "]";
        errorCode = er;
        if (er >= ErrorCode.PlcConnected)
        {
            isSuccess = true;
        }
        else
        {
            isSuccess = false;
        }
    }

    public void Reset(ErrorCode er, string msg)
    {
        message = er.GetString() + "->" + msg;
        errorCode = er;
        if (er >= ErrorCode.PlcConnected)
        {
            isSuccess = true;
        }
        else
        {
            isSuccess = false;
        }
    }

    public void Reset(ErrorCode er, string msg, System.Exception ex)
    {
        message = $"{er.GetString()} -> {msg} [{ex.Message}]";
        errorCode = er;
        if (er >= ErrorCode.PlcConnected)
        {
            isSuccess = true;
        }
        else
        {
            isSuccess = false;
        }
    }
    #endregion

    #region operator override
    #endregion
}

public class RunResult<T> : RunResult
{
    public T? Content;

    public RunResult(ErrorCode errcode) : base(errcode)
    {
        Content = default;
    }

    public RunResult(ErrorCode errcode, T? c) : base(errcode)
    {
        Content = c;
    }

    public RunResult(ErrorCode errcode, System.Exception ex) : base(errcode, ex)
    {
        Content = default;
    }

    public RunResult(ErrorCode errcode, string msg, System.Exception ex) : base(errcode, msg, ex)
    {
        Content = default;
    }

    public RunResult(ErrorCode errcode, string msg) : base(errcode, msg)
    {
        Content = default;
    }
}
