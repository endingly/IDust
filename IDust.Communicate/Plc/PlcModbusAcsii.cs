using HslCommunication;
using HslCommunication.ModBus;
using IDust.Base;
using IDust.Communicate.Plc;
using System.Timers;

namespace IDust.Communicate;

public class PlcModbusAscii : PlcBase, IPlcReadWriteable<ModbusAscii>, IConnectCloseable
{
    #region Members
    private ModbusAscii client;

    ModbusAscii IPlcReadWriteable<ModbusAscii>.CoreClient => client;

    public IPlcReadWriteable<ModbusAscii> ReadWriteHandle => this;
    #endregion

    #region ctor
#pragma warning disable CS8618, CS8622
    public PlcModbusAscii(in PlcParma parma)
    {
        this.parma = parma;
        Init();
        timer.Elapsed += Reconnect;
        var r = this.client.LogNet;
    }
#pragma warning restore CS8618, CS8622
    #endregion

    #region private methods
    protected override void Init()
    {
        if (client != null)
        {
            client.Close();
        }
        else
        {
            client = new ModbusAscii((byte)parma.Station);
        }
        client.ReceiveTimeout = parma.ReceiveTimeOut;
        client.DataFormat = parma.DataFormat;
        client.SerialPortInni(sp =>
        {
            sp.PortName = parma.serialPortParma.PortName;
            sp.BaudRate = parma.serialPortParma.BaudRate;
            sp.DataBits = parma.serialPortParma.DataBits;
            sp.StopBits = parma.serialPortParma.StopBits;
            sp.Parity = parma.serialPortParma.Parity;
        });
    }
    #endregion

    #region override methods
    protected override void Reconnect(object sender, ElapsedEventArgs e)
    {
        Init();
        if (ConnectOpen().isSuccess)
        {
            this.ConnectStatus = true;
            client.LogNet.WriteInfo(ErrorCode.PlcReconnectSuccess.GetString());
        }
        else
        {
            this.ConnectStatus = false;
            client.LogNet.WriteError(ErrorCode.PlcFailToReconnect.GetString());
        }
    }

    public RunResult ConnectOpen()
    {
        if (!parma.IsShortConnect)
        {
            try
            {
                client.Open();
                ConnectStatus = client.IsOpen();
                if (ConnectStatus)
                {
                    client.LogNet.WriteInfo(ErrorCode.PlcConnected.GetString());
                    return new RunResult(ErrorCode.PlcConnected);
                }
                else
                {
                    client.LogNet.WriteError(ErrorCode.PlcNotConnect.GetString());
                    return new RunResult(ErrorCode.PlcNotConnect);
                }
            }
            catch (Exception ex)
            {
                return new RunResult(ErrorCode.PlcNotConnect, ex);
            }
        }
        return new RunResult(ErrorCode.PlcConnected);
    }

    public RunResult ConnectClose()
    {
        try
        {
            client.Close();
            ConnectStatus = client.IsOpen();
            return new RunResult(ErrorCode.PlcDisconnectSuccess);
        }
        catch (Exception ex)
        {
            return new RunResult(ErrorCode.PlcFailToDisconnect, ex);
        }
    }

    #endregion
}
