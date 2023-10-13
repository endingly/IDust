using System;
using System.ComponentModel;
using System.Timers;
using HslCommunication.Core;
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
    private bool _reConnectEnable;
    public PlcParma parma;
    /// <summary>
    /// 此定时器用于自动重连
    /// </summary>
    protected System.Timers.Timer timer = new System.Timers.Timer(3000);
    #endregion

    #region Property
    /// <summary>
    /// 状态属性获取与设置，当状态改变时触发事件
    /// </summary>
    public bool ConnectStatus
    {
        get
        {
            return _connectStatus;
        }
        protected set
        {
            if (_connectStatus != value)
            {
                _connectStatus = value;
                PlcConnectStatusChanged?.Invoke(parma.UserDefineIndex, value);
                if (value == true)
                {
                    _ = IsReConnectEnable ? (this.timer.Enabled = false) : false;
                }
                else if (value == false)
                {
                    _ = IsReConnectEnable ? (this.timer.Enabled = true) : false;
                }
            }
        }
    }
    
    public bool IsReConnectEnable
    {
        get
        {
            return _reConnectEnable;
        }
        protected set
        {
            _reConnectEnable = value;
        }
    }
    #endregion

    #region event
    public event D_void_int_bool_Parma? PlcConnectStatusChanged;

    //public event D_void_int_string_int_Parma? PlcShortStatusChanged;
    #endregion

    #region virtual method
    protected virtual void Init()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 自动重新连接的逻辑，需要子类实现且挂载到定时器的Elapsed事件
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected virtual void Reconnect(object sender, ElapsedEventArgs args)
    {
        throw new NotImplementedException();
    }
    #endregion
}