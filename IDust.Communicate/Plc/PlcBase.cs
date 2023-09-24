using System;
using System.ComponentModel;
using IDust.Base;

namespace IDust.Communicate.Plc;

/// <summary>
/// Plc 的基类
/// </summary>
public class PlcBase
{
    #region member
    protected bool _connectStatus;
    protected bool _messageReadable;
    public PlcParma parma;

    public bool Status
    {
        get
        {
            return _connectStatus;
        }
        set
        {
            if (_connectStatus != value)
            {
                _connectStatus = value;
                PlcConnectStatusChanged?.Invoke(parma.UserDefineIndex, _connectStatus == true);
            }
        }
    }

    public event D_void_int_bool_Parma? PlcConnectStatusChanged;

    public event D_void_int_string_int_Parma? PlcShortStatusChanged;
    #endregion

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