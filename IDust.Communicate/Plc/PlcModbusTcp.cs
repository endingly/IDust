using HslCommunication;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.ModBus;
using HslCommunication.Core.IMessage;
using IDust.Base;
using System.Text;

namespace IDust.Communicate.Plc;

public class PlcModbusTcp : PlcBase, IPlcReadWriteable<ModbusTcpNet>
{
    #region member
    private ModbusTcpNet modbusTcpNet;

    ModbusTcpNet IPlcReadWriteable<ModbusTcpNet>.CoreClient
    {
        get
        {
            return modbusTcpNet;
        }
    }

    public IPlcReadWriteable<ModbusTcpNet> ReadWriteHandle => this;
    #endregion

    #region ctor
#pragma warning disable CS8618, CS8622
    public PlcModbusTcp(in PlcParma parma)
    {
        this.parma = parma;
        Init();
        timer.Elapsed += reConnect;
    }
#pragma warning restore CS8618
    #endregion

    #region public method
    public virtual string GetIpAddress()
    {
        return parma.IpAddress;
    }
    #endregion

    #region private method
    private void setPlcDataFormat()
    {
        modbusTcpNet.DataFormat = (DataFormat)parma.DataFormat;
    }

    private RunResult Init()
    {
        try
        {
            if (modbusTcpNet != null)
            {
                modbusTcpNet.ConnectClose();
                modbusTcpNet.IpAddress = parma.IpAddress;
                modbusTcpNet.Port = parma.Port;
                modbusTcpNet.Station = (byte)parma.Station;
                modbusTcpNet.ConnectTimeOut = parma.ConnectTimeOut;
                modbusTcpNet.ReceiveTimeOut = parma.ReceiveTimeOut;
                modbusTcpNet.IsStringReverse = parma.StrReverse;
                modbusTcpNet.DataFormat = parma.DataFormat;
            }
            else
            {
                modbusTcpNet = new ModbusTcpNet(parma.IpAddress, parma.Port, (byte)parma.Station)
                {
                    ConnectTimeOut = parma.ConnectTimeOut,
                    ReceiveTimeOut = parma.ReceiveTimeOut,
                    IsStringReverse = parma.StrReverse,
                    DataFormat = parma.DataFormat,
                };
            }
            return new RunResult(ErrorCode.PlcInitSuccess);
        }
        catch (Exception ex)
        {
            return new RunResult(ErrorCode.PlcFailToInit, ex);
        }
    }

    /// <summary>
    /// 重新连接
    /// </summary>
    /// <returns></returns>
    protected override void reConnect(object sender, System.Timers.ElapsedEventArgs e)
    {
        modbusTcpNet.IpAddress = parma.IpAddress;
        modbusTcpNet.Port = parma.Port;
        var r = SetStation(parma.Station);
        if (!r)
        {
            return;
        }
        modbusTcpNet.ConnectTimeOut = parma.ConnectTimeOut;
        modbusTcpNet.IsStringReverse = parma.StrReverse;
        var or = modbusTcpNet.ConnectServer();
        if (or.IsSuccess)
        {
            this.Status = true;
            modbusTcpNet.LogNet.WriteInfo(ErrorCode.PlcReconnectSuccess.GetString());
        }
        else
        {
            this.Status = false;
            modbusTcpNet.LogNet.WriteError(ErrorCode.PlcFailToReconnect.GetString());
        }
    }
    #endregion

    #region override method
    public override bool SetStation(int station)
    {
        if (modbusTcpNet != null)
        {
            modbusTcpNet.Station = (byte)parma.Station;
            return true;
        }
        return false;
    }

    public override RunResult ConnectServer()
    {
        RunResult init_result = Init();
        // 如果是短链接，则不需要调用 modbusTcpNet.ConnectServer()，可以直接读写
        if (init_result.isSuccess && !parma.IsShortConnect)
        {
            OperateResult connect_result = modbusTcpNet.ConnectServer();
            if (connect_result.IsSuccess)
            {
                this.Status = true;
                init_result.Reset(ErrorCode.PlcConnected);
            }
            else
            {
                this.Status = false;
                init_result.Reset(ErrorCode.PlcNotConnect);
            }
            return init_result;
        }
        return init_result;
    }

    public override RunResult ConnectClose()
    {
        if (parma.IsShortConnect)
        {
            Status = false;
            return new RunResult(ErrorCode.PlcDisconnectSuccess);
        }
        var r = modbusTcpNet?.ConnectClose();
        if (r != null && r.IsSuccess)
        {
            Status = false;     // 触发事件
            return new RunResult(ErrorCode.PlcDisconnectSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToDisconnect);
        }
    }
    #endregion
}