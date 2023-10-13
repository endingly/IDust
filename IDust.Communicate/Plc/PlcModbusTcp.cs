using HslCommunication;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.ModBus;
using HslCommunication.Core.IMessage;
using IDust.Base;
using System.Text;

namespace IDust.Communicate.Plc;

public class PlcModbusTcp : PlcBase, IPlcReadWriteable<ModbusTcpNet>, IConnectCloseable
{
    #region member
    private ModbusTcpNet modbusTcpNet;

    ModbusTcpNet IPlcReadWriteable<ModbusTcpNet>.CoreClient => modbusTcpNet;

    public IPlcReadWriteable<ModbusTcpNet> ReadWriteHandle => this;
    #endregion

    #region ctor
#pragma warning disable CS8618, CS8622
    public PlcModbusTcp(in PlcParma parma)
    {
        this.parma = parma;
        Init();
        timer.Elapsed += Reconnect;
    }
#pragma warning restore CS8618, CS8622
    #endregion

    #region private method
    protected override void Init()
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

    }

    /// <summary>
    /// 重新连接
    /// </summary>
    /// <returns></returns>
    protected override void Reconnect(object sender, System.Timers.ElapsedEventArgs e)
    {
        Init();
        if (ConnectOpen().isSuccess)
        {
            this.ConnectStatus = true;
            modbusTcpNet.LogNet.WriteInfo(ErrorCode.PlcReconnectSuccess.GetString());
        }
        else
        {
            this.ConnectStatus = false;
            modbusTcpNet.LogNet.WriteError(ErrorCode.PlcFailToReconnect.GetString());
        }
    }
    #endregion

    #region override method
    public RunResult ConnectOpen()
    {
        // 如果是短链接，则不需要调用 modbusTcpNet.ConnectServer()，可以直接读写
        if (!parma.IsShortConnect)
        {
            OperateResult connect_result = modbusTcpNet.ConnectServer();
            if (connect_result.IsSuccess)
            {
                this.ConnectStatus = true;
                return new RunResult(ErrorCode.PlcConnected);
            }
            else
            {
                this.ConnectStatus = false;
                return new RunResult(ErrorCode.PlcNotConnect);
            }
        }
        this.ConnectStatus = true;
        return new RunResult(ErrorCode.PlcConnected);
    }


    public RunResult ConnectClose()
    {
        if (parma.IsShortConnect)
        {
            ConnectStatus = false;
            return new RunResult(ErrorCode.PlcDisconnectSuccess);
        }
        var r = modbusTcpNet?.ConnectClose();
        if (r != null && r.IsSuccess)
        {
            ConnectStatus = false;     
            return new RunResult(ErrorCode.PlcDisconnectSuccess);
        }
        else
        {
            return new RunResult(ErrorCode.PlcFailToDisconnect);
        }
    }
    #endregion
}
