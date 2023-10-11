using HslCommunication;
using HslCommunication.ModBus;
using IDust.Base;
using IDust.Communicate.Plc;
using System.Timers;

namespace IDust.Communicate;

public class PlcModbusAscii : PlcBase, IPlcReadWriteable<ModbusAscii>
{
    #region Members
    private ModbusAscii client;

    ModbusAscii IPlcReadWriteable<ModbusAscii>.CoreClient => client;

    public IPlcReadWriteable<ModbusAscii> ReadWriteHandle => this;
    #endregion

    #region ctor
#pragma warning disable CS8618, CS8602
    public PlcModbusAscii(in PlcParma parma)
    {
        this.parma = parma;
        var r = Init();
        if (r.isSuccess)
        {
            client.DataFormat = parma.DataFormat;
        }
    }
#pragma warning restore CS8618
    #endregion

    #region private methods
    private RunResult Init()
    {
        RunResult r = new RunResult();
        try
        {
            byte.TryParse(parma.Station.ToString(), out byte station);
            if (client != null)
            {
                client.Close();
            }
            else
            {
                client = new ModbusAscii(station);
            }
            client.ReceiveTimeout = parma.ReceiveTimeOut;
            client.SerialPortInni(sp =>
            {
                sp.PortName = parma.serialPortParma.PortName;
                sp.BaudRate = parma.serialPortParma.BaudRate;
                sp.DataBits = parma.serialPortParma.DataBits;
                sp.StopBits = parma.serialPortParma.StopBits;
                sp.Parity = parma.serialPortParma.Parity;
            });
            r.Reset(ErrorCode.PlcInitSuccess);
        }
        catch (Exception ex)
        {
            client.LogNet.WriteException(LogKeyWord.PLC.GetString(),
                                         ErrorCode.PlcFailToInit.GetString(),
                                         ex);
            r.Reset(ErrorCode.PlcFailToInit);
        }
        return r;
    }
    #endregion

    #region override methods
    public override bool SetStation(int station)
    {
        parma.Station = station;
        if (client != null)
        {
            client.Station = (byte)station;
            return true;
        }
        return false;
    }

    public override RunResult ConnectServer()
    {
        var r = Init();
        if (r.isSuccess && !parma.IsShortConnect)
        {
            try
            {
                client.Open();
                Status = client.IsOpen();
                if (Status)
                {
                    SetStation(parma.Station);
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
        return r;
    }

    protected override void reConnect(object sender, ElapsedEventArgs e)
    {
        var r = Init();
        if (r.isSuccess)
        {
            return;
        }
        var or = client.Open();
        if (or.IsSuccess)
        {
            this.Status = true;
            client.LogNet.WriteInfo(ErrorCode.PlcReconnectSuccess.GetString());
        }
        else
        {
            this.Status = false;
            client.LogNet.WriteError(ErrorCode.PlcFailToReconnect.GetString());
        }
    }

    public override RunResult ConnectClose()
    {
        try
        {
            client.Close();
            Status = client.IsOpen();
            return new RunResult(ErrorCode.PlcDisconnectSuccess);
        }
        catch (Exception ex)
        {
            return new RunResult(ErrorCode.PlcFailToDisconnect, ex);
        }
    }

    #endregion
}
