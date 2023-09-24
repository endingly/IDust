using System;
using System.ComponentModel;
using IDust.Base;

namespace IDust.Communicate.Plc;

/// <summary>
/// 连接状态
/// </summary>
public enum ConnectStatus
{
    /// <summary>
    /// 未知
    /// </summary>
    unkown,
    /// <summary>
    /// 已连接
    /// </summary>
    connected,
    /// <summary>
    /// 未连接
    /// </summary>
    disconnected
}

/// <summary>
/// Plc 的基类
/// </summary>
public class PlcBase
{
    public PlcParma parma;

    public ConnectStatus status;

    #region virtual methods
    public virtual RunResult ConnectServer()
    {
        throw new NotImplementedException();
    }

    public virtual RunResult ConnectShort()
    {
        throw new NotImplementedException();
    }

    public virtual RunResult ConnectClose()
    {
        throw new NotImplementedException();
    }

    public virtual RunResult SetStation(int station)
    {
        parma.Station = station;
        return RunResult.CreateSuccessResult();
    }

    public virtual RunResult ReadValue<T>(string address, out T value) where T : struct
    {
        throw new NotImplementedException();
    }

    public virtual RunResult ReadValue<T>(string address, int length, out T[] value) where T : struct
    {
        throw new NotImplementedException();
    }

    public virtual RunResult ReadValue(string address, string value)
    {
        throw new NotImplementedException();
    }

    public virtual RunResult ReadUnicodeString(string address, string value)
    {
        throw new NotImplementedException();
    }

    public virtual RunResult WriteValue<T>(string address, out T value) where T : struct
    {
        throw new NotImplementedException();
    }

    public virtual RunResult WriteValue<T>(string address, int length, out T[] value) where T : struct
    {
        throw new NotImplementedException();
    }

    public virtual RunResult WriteValue(string address, string value)
    {
        throw new NotImplementedException();
    }

    public virtual RunResult WriteUnicodeString(string address, string value)
    {
        throw new NotImplementedException();
    }
    #endregion
}