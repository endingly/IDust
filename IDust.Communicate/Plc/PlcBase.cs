using System;
using System.ComponentModel;
using HslCommunication.ModBus;
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
    protected bool _reConnectEnable;

    /// <summary>
    /// 此定时器用于自动重连
    /// </summary>
    protected System.Timers.Timer timer = new System.Timers.Timer(3000);

    /// <summary>
    /// 状态属性获取与设置，当状态改变时触发事件
    /// </summary>
    protected bool Status
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
                PlcConnectStatusChanged?.Invoke(parma.UserDefineIndex, value);
                if (value == true)
                {
                    this.timer.Enabled = false;
                }
                else if (value == false && _reConnectEnable)
                {
                    this.timer.Enabled = true;
                }
            }
        }
    }

    public event D_void_int_bool_Parma? PlcConnectStatusChanged;

    public event D_void_int_string_int_Parma? PlcShortStatusChanged;
    #endregion

    #region ctor
    #endregion

    #region virtual methods
    protected virtual void reConnect(object sender,System.Timers.ElapsedEventArgs e)
    {
        throw new NotImplementedException();
    }

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

    public virtual bool SetStation(int station)
    {
        throw new NotImplementedException();
    }

    public virtual RunResult ReadValue<T>(string address, out T value) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public virtual RunResult ReadValue<T>(string address, int length, out T[] value) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public virtual RunResult ReadValue(string address, int length, out string value)
    {
        throw new NotImplementedException();
    }

    public virtual RunResult ReadUnicodeString(string address, int length, out string value)
    {
        throw new NotImplementedException();
    }

    public virtual RunResult WriteValue<T>(string address, T value) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public virtual RunResult WriteValue<T>(string address, int length, T[] value) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public virtual RunResult WriteValue(string address, int length, string value)
    {
        throw new NotImplementedException();
    }

    public virtual RunResult WriteUnicodeString(string address, int length, string value)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 清空某一段寄存器的值
    /// </summary>
    /// <param name="address">连续寄存器起始地址</param>
    /// <param name="length">长度，单位为字节</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public virtual RunResult ClearValue(string address, int length)
    {
        throw new NotImplementedException();
    }
    #endregion
}