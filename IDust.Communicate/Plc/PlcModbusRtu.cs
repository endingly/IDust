using HslCommunication.ModBus;
using IDust.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace IDust.Communicate.Plc;

public class PlcModbusRtu : PlcBase, IPlcReadWriteable<ModbusRtu>, IConnectCloseable
{
    private ModbusRtu _coreClient;

    public IPlcReadWriteable<ModbusRtu> ReadWriteHandle => this;

    ModbusRtu IPlcReadWriteable<ModbusRtu>.CoreClient => _coreClient;

#pragma warning disable CS8618, CS8622
    public PlcModbusRtu(in PlcParma parma)
    {
        this.parma = parma;
        Init();
        this.timer.Elapsed += Reconnect;
    }
#pragma warning restore CS8618, CS8622

    #region override PlcBase
    protected override void Init()
    {
        if (_coreClient != null)
        {
            if (_coreClient.IsOpen())
            {
                _ = ConnectClose();
            }
            _coreClient.SerialPortInni(sp =>
            {
                sp.PortName = parma.serialPortParma.PortName;
                sp.BaudRate = parma.serialPortParma.BaudRate;
                sp.BreakState = parma.serialPortParma.BreakState;
                sp.DataBits = parma.serialPortParma.DataBits;
                sp.Parity = parma.serialPortParma.Parity;
            });
        }
        else
        {
            _coreClient = new ModbusRtu();
            _coreClient.IsStringReverse = parma.StrReverse;
            _coreClient.DataFormat = parma.DataFormat;
            _coreClient.Station = (byte)parma.Station;
            _coreClient.ReceiveTimeout = parma.ReceiveTimeOut;
            _coreClient.SerialPortInni(sp =>
            {
                sp.PortName = parma.serialPortParma.PortName;
                sp.BaudRate = parma.serialPortParma.BaudRate;
                sp.BreakState = parma.serialPortParma.BreakState;
                sp.DataBits = parma.serialPortParma.DataBits;
                sp.Parity = parma.serialPortParma.Parity;
            });
        }
    }

    protected override void Reconnect(object sender, ElapsedEventArgs args)
    {
        Init();
        ConnectOpen();
    }
    #endregion

    #region override IConnectCloseable
    public RunResult ConnectOpen()
    {
        var r = _coreClient.Open();
        ConnectStatus = r.IsSuccess;
        return r.IsSuccess ? new RunResult(ErrorCode.PlcConnected) : new RunResult(ErrorCode.PlcNotConnect);
    }

    public RunResult ConnectClose()
    {
        try
        {
            _coreClient.Close();
            ConnectStatus = false;
            return new RunResult(ErrorCode.PlcDisconnectSuccess);
        }
        catch(Exception ex)
        {
            return new RunResult(ErrorCode.PlcFailToDisconnect, ex);
        }
    }
    #endregion
}

