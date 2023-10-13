using HslCommunication.Profinet.Delta;
using IDust.Base;
using System.Timers;

namespace IDust.Communicate.Plc;

public class PlcDeltaModbusTcp : PlcBase, IPlcReadWriteable<DeltaTcpNet>, IConnectCloseable
{
    private DeltaTcpNet client;

    public IPlcReadWriteable<DeltaTcpNet> ReadWriteHandle => this;

    DeltaTcpNet IPlcReadWriteable<DeltaTcpNet>.CoreClient => client;

#pragma warning disable CS8618, CS8622
    public PlcDeltaModbusTcp(in PlcParma parma)
    {
        this.parma = parma;
        Init();
        timer.Elapsed += Reconnect;
    }
#pragma warning restore CS8618, CS8622

    public RunResult ConnectClose()
    {
        if (parma.IsShortConnect)
        {
            return new RunResult(ErrorCode.PlcDisconnectSuccess);
        }
        if (client.ConnectClose().IsSuccess)
        {
            ConnectStatus = false;
            return new RunResult(ErrorCode.PlcDisconnectSuccess);
        }
        else
        {
            ConnectStatus = true;
            return new RunResult(ErrorCode.PlcFailToDisconnect);
        }
    }

    public RunResult ConnectOpen()
    {
        if (parma.IsShortConnect)
        {
            return new RunResult(ErrorCode.PlcConnected);
        }
        if (client.ConnectServer().IsSuccess)
        {
            ConnectStatus = true;
            return new RunResult(ErrorCode.PlcConnected);
        }
        else
        {
            ConnectStatus = false;
            return new RunResult(ErrorCode.PlcNotConnect);
        }
    }

    #region override plcbase
    protected override void Init()
    {
        if (client != null)
        {
            ConnectClose();
            client.IpAddress = parma.IpAddress;
            client.Port = parma.Port;
            client.ReceiveTimeOut = parma.ReceiveTimeOut;
            client.ConnectTimeOut = parma.ConnectTimeOut;
            client.DataFormat = parma.DataFormat;
            client.IsStringReverse = parma.StrReverse;
            client.Station = (byte)parma.Station;
            client.Series = parma.plc_type.ToAdapte();
        }
        else
        {
            client = new DeltaTcpNet(parma.IpAddress, parma.Port);
            client.ReceiveTimeOut = parma.ReceiveTimeOut;
            client.ConnectTimeOut = parma.ConnectTimeOut;
            client.DataFormat = parma.DataFormat;
            client.IsStringReverse = parma.StrReverse;
            client.Station = (byte)parma.Station;
            client.Series = parma.plc_type.ToAdapte();
        }

    }

    protected override void Reconnect(object sender, ElapsedEventArgs args)
    {
        Init();
        ConnectOpen();
    }
    #endregion
}

